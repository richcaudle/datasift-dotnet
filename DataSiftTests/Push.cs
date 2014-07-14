using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using DataSift.Enum;

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
            var response = Client.Push.Get(hash: "13e9347e7da32f19fcdb08e297019d2e");
            Assert.AreEqual("13e9347e7da32f19fcdb08e297019d2e", response.Data.subscriptions[0].hash);
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
            var response = Client.Push.Get(historicsId: "6cd38099f4c1e0f1ac31");
            Assert.AreEqual("3a5c2546136a037d4b2df0b8b8836f3e", response.Data.subscriptions[0].id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Page_Is_Less_Than_One_Fails()
        {
            Client.Push.Get(page: 0);
        }

        [TestMethod]
        public void Get_Page_Succeeds()
        {
            var response = Client.Push.Get(page: 1);
            Assert.AreEqual(2, response.Data.count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Per_Page_Is_Less_Than_One_Fails()
        {
            Client.Push.Get(perPage: 0);
        }

        [TestMethod]
        public void Get_PerPage_Succeeds()
        {
            var response = Client.Push.Get(page: 1, perPage: 1);
            Assert.AreEqual(2, response.Data.count);
            Assert.AreEqual(1, response.Data.subscriptions.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Validate

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_Null_Type_Fails()
        {
            Client.Push.Validate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_Empty_Type_Fails()
        {
            Client.Push.Validate("");
        }

        #endregion

        #region Create

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Name_Fails()
        {
            Client.Push.Create(null, "pull");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Name_Fails()
        {
            Client.Push.Create("", "pull");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Type_Fails()
        {
            Client.Push.Create("New subscription", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Type_Fails()
        {
            Client.Push.Create("New subscription", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_End_Before_Start_Fails()
        {
            Client.Push.Create("New subscription", "", start: DateTimeOffset.Now, end: DateTimeOffset.Now.AddHours(-1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Neither_Hash_Nor_HistoricsId_Fails()
        {
            Client.Push.Create("New subscription", "pull");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Both_Hash_And_HistoricsId_Fails()
        {
            Client.Push.Create("New subscription", "pull", hash: "2459b03a13577579bca76471778a5c3d", historicsId: "3ea6e1ca364f3b327e6f");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Hash_Fails()
        {
            Client.Push.Create("New subscription", "pull", hash: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Bad_Format_Hash_Fails()
        {
            Client.Push.Create("New subscription", "pull", hash: "hash");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_HistoricsId_Fails()
        {
            Client.Push.Create("New subscription", "pull", historicsId: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Bad_Format_HistoricsId_Fails()
        {
            Client.Push.Create("New subscription", "pull", historicsId: "historics");
        }

        [TestMethod]
        public void Create_Correct_Args_Succeeds()
        {
            var response = Client.Push.Create("New subscription", "pull", hash: "42d388f8b1db997faaf7dab487f11290", initialStatus: PushStatus.Active, start: DateTimeOffset.Now, end: DateTimeOffset.Now.AddHours(1));
            Assert.AreEqual("d468655cfe5f93741ddcd30bb309a8c7", response.Data.id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Delete

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Null_Id_Fails()
        {
            Client.Push.Delete(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_Empty_Id_Fails()
        {
            Client.Push.Delete("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_Bad_Format_Id_Fails()
        {
            Client.Push.Delete("delete");
        }

        [TestMethod]
        public void Delete_Correct_Args_Succeeds()
        {
            var response = Client.Push.Delete("08b923395b6ce8bfa4d96f57f863a1c3");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Stop

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Stop_Null_Id_Fails()
        {
            Client.Push.Stop(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Empty_Id_Fails()
        {
            Client.Push.Stop("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Bad_Format_Id_Fails()
        {
            Client.Push.Stop("stop");
        }

        [TestMethod]
        public void Stop_Correct_Args_Succeeds()
        {
            var response = Client.Push.Stop("d468655cfe5f93741ddcd30bb309a8c7");
            Assert.AreEqual("d468655cfe5f93741ddcd30bb309a8c7", response.Data.id);
            Assert.AreEqual("finishing", response.Data.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Pause

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Pause_Null_Id_Fails()
        {
            Client.Push.Pause(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pause_Empty_Id_Fails()
        {
            Client.Push.Pause("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Pause_Bad_Format_Id_Fails()
        {
            Client.Push.Pause("delete");
        }

        [TestMethod]
        public void Pause_Correct_Args_Succeeds()
        {
            var response = Client.Push.Pause("d468655cfe5f93741ddcd30bb309a8c7");
            Assert.AreEqual("d468655cfe5f93741ddcd30bb309a8c7", response.Data.id);
            Assert.AreEqual("paused", response.Data.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Resume

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Resume_Null_Id_Fails()
        {
            Client.Push.Resume(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Resume_Empty_Id_Fails()
        {
            Client.Push.Resume("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Resume_Bad_Format_Id_Fails()
        {
            Client.Push.Resume("delete");
        }

        [TestMethod]
        public void Resume_Correct_Args_Succeeds()
        {
            var response = Client.Push.Resume("d468655cfe5f93741ddcd30bb309a8c7");
            Assert.AreEqual("d468655cfe5f93741ddcd30bb309a8c7", response.Data.id);
            Assert.AreEqual("active", response.Data.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Log

        [TestMethod]
        public void Log_No_Arguments_Succeeds()
        {
            var response = Client.Push.Log();
            Assert.AreEqual(8740, response.Data.count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_By_Id_Empty_Id_Fails()
        {
            Client.Push.Log(id: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_By_Id_Bad_Format_Id_Fails()
        {
            Client.Push.Log(id: "push");
        }

        [TestMethod]
        public void Log_By_Id_Complete_Succeeds()
        {
            var response = Client.Push.Log(id: "d468655cfe5f93741ddcd30bb309a8c7");
            Assert.AreEqual("d468655cfe5f93741ddcd30bb309a8c7", response.Data.log_entries[0].subscription_id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_Page_Is_Less_Than_One_Fails()
        {
            Client.Push.Log(page: 0);
        }

        [TestMethod]
        public void Log_Page_Succeeds()
        {
            var response = Client.Push.Log(page: 1);
            Assert.AreEqual(182, response.Data.count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_Per_Page_Is_Less_Than_One_Fails()
        {
            Client.Push.Log(perPage: 0);
        }

        [TestMethod]
        public void Log_PerPage_Succeeds()
        {
            var response = Client.Push.Log(page: 1, perPage: 5, orderDirection: OrderDirection.Ascending);
            Assert.AreEqual(5, response.Data.log_entries.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Update

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Id_Fails()
        {
            Client.Push.Update(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Empty_Id_Fails()
        {
            Client.Push.Update("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Bad_Format_Id_Fails()
        {
            Client.Push.Update("update");
        }

        [TestMethod]
        public void Update_Correct_Args_Succeeds()
        {
            var response = Client.Push.Update("f4d4caee9acfd27faf88843d8d6191b1", "new name");
            Assert.AreEqual("new name", response.Data.name);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

    }
}
