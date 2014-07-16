using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SuperSocket.ClientEngine;
using WebSocket4Net;

namespace DataSift.Streaming
{
    public class DataSiftStream
    {
        public delegate void OnConnectHandler(object sender, EventArgs e);
        public event OnConnectHandler OnConnect;


        private WebSocket _websocket;

        internal void Connect(string username, string apikey, bool secure = true)
        {
            // TODO : Auto reconnect
            // TODO: Check ping / pong works both ways?
            
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback((s, ce, ch, ssl) => true);
            
            var protocol = (secure) ? "wss" : "ws";
            var url = String.Format("{0}://websocket.datasift.com/c80bf0d1e236209dd3bc8afe23ebfcee?username={1}&api_key={2}", protocol, username, apikey);
            Console.WriteLine("Connecting to: " + url);

            _websocket = new WebSocket(url);
            _websocket.EnableAutoSendPing = true;
            _websocket.Opened +=_websocket_Opened;
            _websocket.Error +=_websocket_Error;
            _websocket.Closed +=_websocket_Closed;
            _websocket.MessageReceived +=_websocket_MessageReceived;
            _websocket.Open();
        }

        void _websocket_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.Exception.StackTrace);
        }

        void _websocket_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
            //OnConnect(this, null);
        }

        void _websocket_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Closed");
        }

        void _websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Message: " + e.Message);
        }


    }
}
