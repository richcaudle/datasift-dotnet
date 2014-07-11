using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DataSiftTests
{
    [TestClass]
    public class Push : TestBase
    {
        #region Get

        [TestMethod]
        public void Get_No_Arguments_Succeeds()
        {
            var response = Client.Push.Get();
            Assert.AreEqual(2, response.Data.count);
            Assert.AreEqual("d668655cfe5f93741ddcd30bb309a8c7", response.Data.subscriptions[0].id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Id_Empty_Id_Fails()
        {
            Client.Push.Get(id: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Id_Bad_Format_Id_Fails()
        {
            Client.Push.Get(id: "push");
        }

        [TestMethod]
        public void Get_By_Id_Complete_Succeeds()
        {
            var response = Client.Push.Get(id: "d468655cfe5f93741ddcd30bb309a8c7");
            Assert.AreEqual("d468655cfe5f93741ddcd30bb309a8c7", response.Data.id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Hash_Empty_Hash_Fails()
        {
            Client.Push.Get(hash: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Hash_Bad_Format_Hash_Fails()
        {
            Client.Push.Get(hash: "push");
        }

        [TestMethod]
        public void Get_By_Hash_Complete_Succeeds()
        {
            var response = Client.Push.Get(hash: "");
            Assert.AreEqual("", response.Data.id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_HistoricsId_Empty_HistoricsId_Fails()
        {
            Client.Push.Get(historicsId: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_HistoricsId_Bad_Format_HistoricsId_Fails()
        {
            Client.Push.Get(historicsId: "push");
        }

        [TestMethod]
        public void Get_By_HistoricsId_Complete_Succeeds()
        {
            var response = Client.Push.Get(historicsId: "");
            Assert.AreEqual("", response.Data.id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

    }
}
