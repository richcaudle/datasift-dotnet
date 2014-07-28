using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SuperSocket.ClientEngine;
using WebSocket4Net;
using Newtonsoft.Json;
using DataSift.Enum;

namespace DataSift.Streaming
{
    public class DataSiftStream
    {
        public delegate void OnConnectHandler(object sender, EventArgs e);
        public event OnConnectHandler OnConnect;

        public delegate void OnMessageHandler(string hash, dynamic message);
        public event OnMessageHandler OnMessage;

        public delegate void OnDataSiftMessageHandler(DataSiftMessageStatus status, string message);
        public event OnDataSiftMessageHandler OnDataSiftMessage;

        private Dictionary<string, OnMessageHandler> _messageHandlers = new Dictionary<string, OnMessageHandler>();

        private WebSocket _websocket;

        internal void Connect(string username, string apikey, bool secure = true)
        {
            // TODO: Validate args
            // TODO : Auto reconnect, with re-subscribe to streams?
            // TODO: Check ping / pong works both ways?
            // TODO: Allow subscribe to messages at global level
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
             
            var protocol = (secure) ? "wss" : "ws";
            var url = String.Format("{0}://stream.datasift.com/multi?statuses=true&username={1}&api_key={2}", protocol, username, apikey);
            
            _websocket = new WebSocket(url);
            _websocket.EnableAutoSendPing = true;
            _websocket.Opened +=_websocket_Opened;
            _websocket.Error +=_websocket_Error;
            _websocket.Closed +=_websocket_Closed;
            _websocket.MessageReceived +=_websocket_MessageReceived;
            _websocket.Open();
        }

        public void Subscribe(string hash, OnMessageHandler messageHandler = null)
        {
            // TODO: Check hash format

            if(messageHandler != null)
            {
                if (_messageHandlers.ContainsKey(hash))
                    _messageHandlers[hash] = messageHandler;
                else
                    _messageHandlers.Add(hash, messageHandler);
            }
            
            var message = new { action = "subscribe", hash = hash };
            _websocket.Send(JsonConvert.SerializeObject(message));
        }

        void _websocket_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.Exception.StackTrace);
        }

        void _websocket_Opened(object sender, EventArgs e)
        {
            OnConnect(this, null);
        }

        void _websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Closed");
        }

        void _websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = APIHelpers.DeserializeResponse(e.Message);

            if (message.status != null)
            {
                if (OnDataSiftMessage != null)
                {
                    DataSiftMessageStatus status;

                    if (System.Enum.TryParse<DataSiftMessageStatus>(message.status, true, out status))
                    {
                        OnDataSiftMessage(status, message.message);
                    }
                }
            }
            else if(message.hash != null)
            {
                // Fire message at subscription level
                if (_messageHandlers.ContainsKey(message.hash))
                    _messageHandlers[message.hash](message.hash, message.data);

                // Fire message at connection level
                if (OnMessage != null)
                    OnMessage(message.hash, message.data);
            }


        }


    }
}
