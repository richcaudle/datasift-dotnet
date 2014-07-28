using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSift.Streaming;
using System.Threading;

namespace DataSiftTests
{
    [TestClass]
    public class Streaming : TestBase
    {
        AutoResetEvent _TestTrigger;

        #region Subscribe

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Subscribe_With_Null_Hash_Fails()
        {
            var stream = Client.Connect();
            stream.Subscribe(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Subscribe_With_Empty_Hash_Fails()
        {
            var stream = Client.Connect();
            stream.Subscribe("");
        }

        [TestMethod]
        public void Subscribe_Global_With_Valid_Hash_Succeeds()
        {
            var stream = Client.Connect(); 
            
            this._TestTrigger = new AutoResetEvent(false);

            stream.OnSubscribed += delegate(string hash)
            {
                Assert.AreEqual("b09z345fe2f1fed748c12268fd473662", hash); 
                this._TestTrigger.Set();
            };

            stream.Subscribe("b09z345fe2f1fed748c12268fd473662"); 
            this._TestTrigger.WaitOne();
        }

        [TestMethod]
        public void Subscribe_Local_With_Valid_Hash_Succeeds()
        {
            var stream = Client.Connect();
            this._TestTrigger = new AutoResetEvent(false);

            DataSift.Streaming.DataSiftStream.OnSubscribedHandler onSubscribed = (hash) =>
            {
                Assert.AreEqual("b09z345fe2f1fed748c12268fd473662", hash);
                this._TestTrigger.Set();
            };

            stream.Subscribe("b09z345fe2f1fed748c12268fd473662", subscribedHandler: onSubscribed);
            this._TestTrigger.WaitOne();
        }

        #endregion


        #region OnMessage

        [TestMethod]
        public void Receive_Message_Global()
        {
            var stream = Client.Connect();

            this._TestTrigger = new AutoResetEvent(false);

            stream.OnMessage += delegate(string hash, dynamic message)
            {
                Assert.AreEqual("b09z345fe2f1fed748c12268fd473662", hash);
                Assert.AreEqual("Test content", message.interaction.content);
                this._TestTrigger.Set();
            };

            stream.Subscribe("b09z345fe2f1fed748c12268fd473662");
            this._TestTrigger.WaitOne();
        }

        [TestMethod]
        public void Connect_Local_Succeeds()
        {
            var stream = Client.Connect();
            this._TestTrigger = new AutoResetEvent(false);

            DataSift.Streaming.DataSiftStream.OnMessageHandler onMessage = (hash, message) =>
            {
                Assert.AreEqual("b09z345fe2f1fed748c12268fd473662", hash);
                Assert.AreEqual("Test content", message.interaction.content);
                this._TestTrigger.Set();
            };

            stream.Subscribe("b09z345fe2f1fed748c12268fd473662", messageHandler: onMessage);
            this._TestTrigger.WaitOne();
        }

        #endregion

    }
}
