using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSift.Rest;
using DataSift.Streaming;

namespace DataSiftTests
{
    [TestClass]
    public class TestBase
    {
        private DataSift.DataSift _client;

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new DataSift.DataSift(Run.Default.username, Run.Default.apikey, requestCreator: GetRequestMock, connectionCreator: GetStreamConnectionMock);
        }

        protected DataSift.DataSift Client { get { return _client; } }

        public IRestAPIRequest GetRequestMock(string username, string apikey)
        {
            return new MockRestAPIRequest();
        }

        public IStreamConnection GetStreamConnectionMock(string url)
        {
            return new MockStreamConnection(url);
        }

    }
}
