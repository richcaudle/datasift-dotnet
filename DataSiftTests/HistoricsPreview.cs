using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Collections.Generic;
using DataSift.Rest;

namespace DataSiftTests
{
    [TestClass]
    public class HistoricsPreview : TestBase
    {
        public List<HistoricsPreviewParameter> DummyCreateParams
        {
            get
            {
                var prms = new List<HistoricsPreviewParameter>();
                prms.Add(new HistoricsPreviewParameter() { Target = "interaction.author.link", Analysis = "targetVol", Argument = "hour" });
                return prms;
            }
        }

        #region Create

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Sources_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", null, DummyCreateParams, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Sources_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Hash_Fails()
        {
            Client.HistoricsPreview.Create(null, new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Hash_Fails()
        {
            Client.HistoricsPreview.Create("", new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Bad_Format_Hash_Fails()
        {
            Client.HistoricsPreview.Create("hash", new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Start_Too_Early_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, DummyCreateParams, new DateTimeOffset(2009, 12, 31, 23, 59, 59, TimeSpan.Zero));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Start_Too_Late_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddHours(-1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Start_After_End_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-3));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_End_Too_Late_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Params_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, null, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Params_Fails()
        {
            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, new List<HistoricsPreviewParameter>(), DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_More_Than_Twenty_Params_Fails()
        {
            var prms = new List<HistoricsPreviewParameter>();

            for (int i = 0; i <= 20; i++)
            {
                prms.Add(new HistoricsPreviewParameter() { Target = "interaction.author.link", Analysis = "targetVol", Argument = "hour" });
            }

            Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, prms, DateTimeOffset.Now.AddDays(-2));
        }

        [TestMethod]
        public void Create_Correct_Args_Succeeds()
        {
            var response = Client.HistoricsPreview.Create("2459b03a13577579bca76471778a5c3d", new string[] { "twitter" }, DummyCreateParams, DateTimeOffset.Now.AddDays(-2));
            Assert.AreEqual("3ddb72ca02389dbf3b46", response.Data.id);
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        }

        #endregion

        #region Get

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Get_Null_Id_Fails()
        {
            Client.HistoricsPreview.Get(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Empty_Id_Fails()
        {
            Client.HistoricsPreview.Get("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Bad_Format_Id_Fails()
        {
            Client.HistoricsPreview.Get("get");
        }

        [TestMethod]
        public void Get_Running_Succeeds()
        {
            var response = Client.HistoricsPreview.Get("e25d533cf287ec44fe66e8362running");
            Assert.AreEqual("running", response.Data.status);
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        }

        [TestMethod]
        public void Get_Finished_Succeeds()
        {
            var response = Client.HistoricsPreview.Get("e25d533cf287ec44fe66e8362finished");
            Assert.AreEqual("succeeded", response.Data.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

    }
}
