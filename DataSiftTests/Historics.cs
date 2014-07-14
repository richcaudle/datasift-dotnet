using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataSift.Enum;
using System.Net;

namespace DataSiftTests
{
    [TestClass]
    public class Historics : TestBase
    {
        #region Get

        [TestMethod]
        public void Get_No_Arguments_Succeeds()
        {
            var response = Client.Historics.Get();
            Assert.AreEqual("twitter", response.Data.data[0].name);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Id_Empty_Id_Fails()
        {
            Client.Historics.Get(id: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Id_Bad_Format_Id_Fails()
        {
            Client.Historics.Get(id: "historics");
        }

        [TestMethod]
        public void Get_By_Id_Complete_Succeeds()
        {
            var response = Client.Historics.Get(id: "9e97d9ac115e58fcb7ab");
            Assert.AreEqual("9e97d9ac115e58fcb7ab", response.Data.id);
            Assert.AreEqual("twitter", response.Data.sources[0]);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Max_Is_Less_Than_One_Fails()
        {
            Client.Historics.Get(max: 0);
        }

        [TestMethod]
        public void Get_Max_Succeeds()
        {
            var response = Client.Historics.Get(max: 1);
            Assert.AreEqual(1, response.Data.data.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Page_Is_Less_Than_One_Fails()
        {
            Client.Historics.Get(page: 0);
        }

        [TestMethod]
        public void Get_With_Estimate_Succeeds()
        {
            var response = Client.Historics.Get(withEstimate: true);
            Assert.AreEqual(1363287634, response.Data.estimated_completion);
            Assert.AreEqual(1363274434, response.Data.chunks[0].estimated_completion);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Prepare

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Prepare_Null_Hash_Fails()
        {
            Client.Historics.Prepare(null, DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), "Library test", new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Prepare_Empty_Hash_Fails()
        {
            Client.Historics.Prepare("", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), "Library test", new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Prepare_Bad_Format_Hash_Fails()
        {
            Client.Historics.Prepare("hash", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), "Library test", new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Prepare_Too_Late_End_Fails()
        {
            Client.Historics.Prepare("2459b03a13577579bca76471778a5c3d", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now, "Library test", new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Prepare_Start_After_End_Fails()
        {
            Client.Historics.Prepare("2459b03a13577579bca76471778a5c3d", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-3), "Library test", new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Prepare_Null_Sources_Fails()
        {
            Client.Historics.Prepare("2459b03a13577579bca76471778a5c3d", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), "Library test", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Prepare_Empty_Sources_Fails()
        {
            Client.Historics.Prepare("2459b03a13577579bca76471778a5c3d", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), "Library test", new string[] {});
        }

        [TestMethod]
        public void Prepare_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Prepare("2459b03a13577579bca76471778a5c3d", DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), "Library test", new string[] { "twitter" }, Sample.OneHundredPercent);
            Assert.AreEqual("4ef7c852a96d6352764f", response.Data.id);
            Assert.AreEqual(99, response.Data.availability.sources.twitter.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }


        #endregion

        #region Delete

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Null_Id_Fails()
        {
            Client.Historics.Delete(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_Empty_Id_Fails()
        {
            Client.Historics.Delete("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_Bad_Format_Id_Fails()
        {
            Client.Historics.Delete("id");
        }

        [TestMethod]
        public void Delete_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Delete("2490313575797478a5c3");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Status

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Status_Too_Late_End_Fails()
        {
            Client.Historics.Status(DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(1), new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Status_Start_After_End_Fails()
        {
            Client.Historics.Status(DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-3), new string[] { "twitter" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Status_Null_Sources_Fails()
        {
            Client.Historics.Status(DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Status_Empty_Sources_Fails()
        {
            Client.Historics.Status(DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), new string[] { });
        }

        [TestMethod]
        public void Status_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Status(DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(-1), new string[] { "twitter" });
            Assert.AreEqual(100, response.Data[0].sources.twitter.augmentations.klout);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Update

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Id_Fails()
        {
            Client.Historics.Update(null, "new name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Empty_Id_Fails()
        {
            Client.Historics.Update("", "new name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Bad_Format_Id_Fails()
        {
            Client.Historics.Update("id", "new name");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Name_Fails()
        {
            Client.Historics.Update("2490313575797478a5c3", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Empty_Name_Fails()
        {
            Client.Historics.Update("2490313575797478a5c3", "");
        }

        [TestMethod]
        public void Update_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Update("2490313575797478a5c3", "new name");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
         
        #endregion

        #region Start

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Start_Null_Id_Fails()
        {
            Client.Historics.Start(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Start_Empty_Id_Fails()
        {
            Client.Historics.Start("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Start_Bad_Format_Id_Fails()
        {
            Client.Historics.Stop("start");
        }

        [TestMethod]
        public void Start_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Start("2490313575797478a5c3");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Stop

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Stop_Null_Id_Fails()
        {
            Client.Historics.Stop(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Empty_Id_Fails()
        {
            Client.Historics.Stop("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Bad_Format_Id_Fails()
        {
            Client.Historics.Stop("stop");
        }

        [TestMethod]
        public void Stop_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Stop("2490313575797478a5c3");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Pause

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Pause_Null_Id_Fails()
        {
            Client.Historics.Pause(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pause_Empty_Id_Fails()
        {
            Client.Historics.Pause("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pause_Bad_Format_Id_Fails()
        {
            Client.Historics.Pause("delete");
        }

        [TestMethod]
        public void Pause_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Pause("2490313575797478a5c3");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Resume

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resume_Null_Id_Fails()
        {
            Client.Historics.Resume(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Resume_Empty_Id_Fails()
        {
            Client.Historics.Resume("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Resume_Bad_Format_Id_Fails()
        {
            Client.Historics.Resume("delete");
        }

        [TestMethod]
        public void Resume_Correct_Args_Succeeds()
        {
            var response = Client.Historics.Resume("2490313575797478a5c3");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

    }
}
