using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSift.Rest;

namespace DataSiftTests
{
    [TestClass]
    public class TestBase
    {
        private DataSift.DataSift _client;

        [TestInitialize]
        public void TestInitialize()
        {
            _client = new DataSift.DataSift(Run.Default.username, Run.Default.apikey, GetRequestMock);
        }

        protected DataSift.DataSift Client { get { return _client; } }

        public IRestAPIRequest GetRequestMock(string username, string apikey)
        {
            return new MockRestAPIRequest();
        }

    }
}
