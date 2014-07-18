using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSift.Enum;

namespace DataSift.Streaming
{
    public delegate void OnConnectHandler(object sender, EventArgs e);
    public delegate void OnMessageHandler(string hash, dynamic message);
    public delegate void OnSubscribedHandler(string hash);
    public delegate void OnDataSiftMessageHandler(DataSiftMessageStatus status, string message);

    public interface IInteractionStream
    {
        event OnMessageHandler OnMessage;
        event OnConnectHandler OnConnect;
        event OnSubscribedHandler OnSubscribed;
        event OnDataSiftMessageHandler OnDataSiftMessage;

        void Connect(string username, string apikey, bool secure = true);
        void Subscribe(string hash, OnMessageHandler messageHandler = null, OnSubscribedHandler subscribedHandler = null);

    }
}
