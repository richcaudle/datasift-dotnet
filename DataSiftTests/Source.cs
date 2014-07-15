using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Collections.Generic;

namespace DataSiftTests
{
    [TestClass]
    public class Source : TestBase
    {
        public dynamic DummyParameters
        {
            get
            {
                return new
                {
                    likes = true,
                    posts_by_others = true,
                    comments = true,
                    page_likes = false
                };
            }
        }

        public dynamic DummyResources
        {
            get {

                var r = new[] {
                    new { 
                        parameters = new {
                            url = "http://www.facebook.com/theguardian",
                            title = "The Guardian",
                            id = 10513336322
                        }
                    }
                };

                return r;
            }
        }

        public dynamic DummyAuth
        {
            get
            {

                var r = new[] {
                    new { 
                        parameters = new {
                            value = "EZBXlFZBUgBYmjHkxc2pPmzLeJJYmAvQkwZCRdm0A1NAjidHy1h"
                        }
                    }
                };

                return r;
            }
        }

        #region Get

        [TestMethod]
        public void Get_No_Arguments_Succeeds()
        {
            var response = Client.Source.Get();
            Assert.AreEqual(2, response.Data.sources.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Empty_Source_Type_Fails()
        {
            Client.Source.Get(sourceType: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Page_Is_Less_Than_One_Fails()
        {
            Client.Source.Get(page: 0);
        }

        [TestMethod]
        public void Get_Page_Succeeds()
        {
            var response = Client.Source.Get(page: 1);
            Assert.AreEqual(2, response.Data.count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_Per_Page_Is_Less_Than_One_Fails()
        {
            Client.Source.Get(perPage: 0);
        }

        [TestMethod]
        public void Get_PerPage_Succeeds()
        {
            var response = Client.Source.Get(page: 1, perPage: 1);
            Assert.AreEqual(2, response.Data.count);
            Assert.AreEqual(1, response.Data.sources.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Id_Empty_Id_Fails()
        {
            Client.Source.Get(id: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_By_Id_Bad_Format_Id_Fails()
        {
            Client.Source.Get(id: "get");
        }

        [TestMethod]
        public void Get_By_Id_Complete_Succeeds()
        {
            var response = Client.Source.Get(id: "fa2e72e3a7ae40c2a6e86e96381d8165");
            Assert.AreEqual("fa2e72e3a7ae40c2a6e86e96381d8165", response.Data.id);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Create

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Source_Type_Fails()
        {
            Client.Source.Create(null, "Test source", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Source_Type_Fails()
        {
            Client.Source.Create("", "Test source", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Name_Fails()
        {
            Client.Source.Create("facebook_page", null, DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Create_Empty_Name_Fails()
        {
            Client.Source.Create("facebook_page", "", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        public void Create_Correct_Args_Succeeds()
        {
            var response = Client.Source.Create("facebook_page", "Test Source", DummyParameters, DummyResources, DummyAuth);
            Assert.AreEqual("da4f8df71a0f43698acf9240b5ad668f", response.Data.id);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        #endregion

        #region Update

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Source_Type_Fails()
        {
            Client.Source.Update(null, "Test source", "da4f8df71a0f43698acf9240b5ad668f", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Empty_Source_Type_Fails()
        {
            Client.Source.Update("", "Test source", "da4f8df71a0f43698acf9240b5ad668f", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Name_Fails()
        {
            Client.Source.Update("facebook_page", null, "da4f8df71a0f43698acf9240b5ad668f", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Empty_Name_Fails()
        {
            Client.Source.Update("facebook_page", "", "da4f8df71a0f43698acf9240b5ad668f", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null_Id_Fails()
        {
            Client.Source.Update("facebook_page", "Test source", null, DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Empty_Id_Fails()
        {
            Client.Source.Update("facebook_page", "Test source", "", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Update_Bad_Format_Id_Fails()
        {
            Client.Source.Update("facebook_page", "Test source", "update", DummyParameters, DummyResources, DummyAuth);
        }

        [TestMethod]
        public void Update_Correct_Args_Succeeds()
        {
            var response = Client.Source.Update("facebook_page", "news_source", "da4f8df71a0f43698acf9240b5ad668f", DummyParameters, DummyResources, DummyAuth);
            Assert.AreEqual("da4f8df71a0f43698acf9240b5ad668f", response.Data.id);
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        }

        #endregion

        #region Delete

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Delete_Null_Id_Fails()
        {
            Client.Source.Delete(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_Empty_Id_Fails()
        {
            Client.Source.Delete("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Delete_Bad_Format_Id_Fails()
        {
            Client.Source.Delete("delete");
        }

        [TestMethod]
        public void Delete_Correct_Args_Succeeds()
        {
            var response = Client.Source.Delete("e25d533cf287ec44fe66e8362f61961f");
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }

        #endregion

        #region Start

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Start_Null_Id_Fails()
        {
            Client.Source.Start(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Start_Empty_Id_Fails()
        {
            Client.Source.Start("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Start_Bad_Format_Id_Fails()
        {
            Client.Source.Start("delete");
        }

        [TestMethod]
        public void Start_Correct_Args_Succeeds()
        {
            var response = Client.Source.Start("e25d533cf287ec44fe66e8362f61961f");
            Assert.AreEqual("active", response.Data.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Stop

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Stop_Null_Id_Fails()
        {
            Client.Source.Stop(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Empty_Id_Fails()
        {
            Client.Source.Stop("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Stop_Bad_Format_Id_Fails()
        {
            Client.Source.Stop("delete");
        }

        [TestMethod]
        public void Stop_Correct_Args_Succeeds()
        {
            var response = Client.Source.Stop("e25d533cf287ec44fe66e8362f61961f");
            Assert.AreEqual("paused", response.Data.status);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Log

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Log_Null_Id_Fails()
        {
            Client.Source.Log(id:null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_Empty_Id_Fails()
        {
            Client.Source.Log(id: "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_Bad_Format_Id_Fails()
        {
            Client.Source.Log(id: "log");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_Page_Is_Less_Than_One_Fails()
        {
            Client.Source.Log(id: "fa2e72e3a7ae40c2a6e86e96381d8165", page: 0);
        }

        [TestMethod]
        public void Log_Page_Succeeds()
        {
            var response = Client.Source.Log(id: "fa2e72e3a7ae40c2a6e86e96381d8165", page: 1);
            Assert.AreEqual(20, response.Data.count);
            Assert.AreEqual(1, response.Data.log_entries.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Log_Per_Page_Is_Less_Than_One_Fails()
        {
            Client.Source.Log(id: "fa2e72e3a7ae40c2a6e86e96381d8165", perPage: 0);
        }

        [TestMethod]
        public void Log_PerPage_Succeeds()
        {
            var response = Client.Source.Log(id: "fa2e72e3a7ae40c2a6e86e96381d8165", page: 1, perPage: 1);
            Assert.AreEqual(20, response.Data.count);
            Assert.AreEqual(1, response.Data.log_entries.Count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public void Log_Correct_Args_Succeeds()
        {
            var response = Client.Source.Log(id: "fa2e72e3a7ae40c2a6e86e96381d8165");
            Assert.AreEqual(20, response.Data.count);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

    }
}
