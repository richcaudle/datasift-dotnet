using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SuperSocket.ClientEngine;
using WebSocket4Net;
using Newtonsoft.Json;
using DataSift.Enum;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace DataSift.Streaming
{
    public class DataSiftStream
    {
        private Dictionary<string, OnMessageHandler> _messageHandlers = new Dictionary<string, OnMessageHandler>();
        private Dictionary<string, OnSubscribedHandler> _subscribedHandlers  = new Dictionary<string, OnSubscribedHandler>();
        private Dictionary<string, OnMessageHandler> _deleteHandlers = new Dictionary<string, OnMessageHandler>();

        private IStreamConnection _connection;
        private DataSiftClient.GetStreamConnectionDelegate _getConnection;

        private string _domain;

        #region Public Events

        public delegate void OnConnectHandler();
        public event OnConnectHandler OnConnect;

        public delegate void OnMessageHandler(string hash, dynamic message);
        public event OnMessageHandler OnMessage;

        public event OnMessageHandler OnDelete;

        public delegate void OnSubscribedHandler(string hash);
        public event OnSubscribedHandler OnSubscribed;

        public delegate void OnDataSiftMessageHandler(DataSiftMessageStatus status, string message);
        public event OnDataSiftMessageHandler OnDataSiftMessage;

        public delegate void OnErrorHandler(StreamAPIException ex);
        public event OnErrorHandler OnError;

        public event EventHandler OnClosed;

        #endregion

        #region Public Methods

        public DataSiftStream(DataSiftClient.GetStreamConnectionDelegate connectionCreator, string domain = "stream.datasift.com")
        {
            _domain = domain;

            if (connectionCreator == null)
                _getConnection = GetConnectionDefault;
            else
                _getConnection = connectionCreator;
        }

        internal void Connect(string username, string apikey, bool secure = true)
        {
            // TODO: Auto reconnect - do on websocket error & close events?
            // TODO: Auto reconnect - only do if connected once successfully?
            // TODO: Auto reconnect - with re-subscribe to streams
            // TODO: Auto-reconnect - backoff
            // TODO: Build long-running app to check connection remains

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
             
            var protocol = (secure) ? "wss" : "ws";
            var url = String.Format("{0}://{1}/multi?statuses=true&username={2}&api_key={3}", protocol, _domain, username, apikey);

            _connection = GetConnection(url);
            _connection.Opened += _connection_Opened;
            _connection.Error += _connection_Error;
            _connection.Closed += _connection_Closed;
            _connection.MessageReceived += _connection_MessageReceived;
            _connection.Open();
        }

        public void Subscribe(string hash, OnMessageHandler messageHandler = null, OnSubscribedHandler subscribedHandler = null, OnMessageHandler deleteHandler = null)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            Contract.Requires<ArgumentException>(hash.Trim().Length > 0);
            Contract.Requires<ArgumentException>(Constants.STREAM_HASH_FORMAT.IsMatch(hash), Messages.INVALID_STREAM_HASH);

            if(messageHandler != null)
            {
                if (_messageHandlers.ContainsKey(hash))
                    _messageHandlers[hash] = messageHandler;
                else
                    _messageHandlers.Add(hash, messageHandler);
            }

            if (subscribedHandler != null)
            {
                if (_subscribedHandlers.ContainsKey(hash))
                    _subscribedHandlers[hash] = subscribedHandler;
                else
                    _subscribedHandlers.Add(hash, subscribedHandler);
            }

            if (deleteHandler != null)
            {
                if (_deleteHandlers.ContainsKey(hash))
                    _deleteHandlers[hash] = deleteHandler;
                else
                    _deleteHandlers.Add(hash, deleteHandler);
            }
            
            var message = new { action = "subscribe", hash = hash };
            _connection.Send(JsonConvert.SerializeObject(message));
        }

        #endregion


        #region Mocking / Faking

        private IStreamConnection GetConnectionDefault(string url)
        {
            return new StreamConnection(url);
        }

        internal IStreamConnection GetConnection(string url)
        {
            return _getConnection(url);
        }

        #endregion

        #region Internal Methods

        private void _connection_Error(object sender, ErrorEventArgs e)
        {
            if (OnError != null)
                OnError(new StreamAPIException("A connection error has occurred",e.Exception));
        }

        private void _connection_Opened(object sender, EventArgs e)
        {
            if (OnConnect != null)
                OnConnect();
        }

        private void _connection_Closed(object sender, EventArgs e)
        {
            if (OnClosed != null)
                OnClosed(this, e);
        }


        private void _connection_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var message = APIHelpers.DeserializeResponse(e.Message);

            if (APIHelpers.HasAttr(message, "status"))
            {
                DataSiftMessageStatus status;

                if (System.Enum.TryParse<DataSiftMessageStatus>(message.status, true, out status))
                {
                    // Special case when subscription has succeeded
                    if (status == DataSiftMessageStatus.Success && APIHelpers.HasAttr(message, "hash"))
                    {
                        // Fire message at subscription level
                        if (_subscribedHandlers.ContainsKey(message.hash))
                            _subscribedHandlers[message.hash](message.hash);

                        // Fire message at connection level
                        if (OnSubscribed != null)
                            OnSubscribed(message.hash);
                    }
                    else
                    {
                        if (OnDataSiftMessage != null)
                            OnDataSiftMessage(status, message.message);
                    }
                }
            }
            else if (APIHelpers.HasAttr(message, "hash"))
            {
                // Check for deletes
                if (APIHelpers.HasAttr(message, "data"))
                {
                    if(APIHelpers.HasAttr(message.data, "deleted"))
                    {
                        // Fire delete at subscription level
                        if (_deleteHandlers.ContainsKey(message.hash))
                            _deleteHandlers[message.hash](message.hash, message.data);

                        // Fire delete at connection level
                        if (OnDelete != null)
                            OnDelete(message.hash, message.data);

                        return;
                    }
                }

                // Otherwise normal interaction

                // Fire message at subscription level
                if (_messageHandlers.ContainsKey(message.hash))
                    _messageHandlers[message.hash](message.hash, message.data);

                // Fire message at connection level
                if (OnMessage != null)
                    OnMessage(message.hash, message.data);
            }


        }

        #endregion

    }
}
