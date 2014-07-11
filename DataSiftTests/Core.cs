using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace DataSiftTests
{
    [TestClass]
    public class Core : TestBase
    {
        #region Validate

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_Null_CSDL_Fails()
        {
            Client.Validate(null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Validate_Empty_CSDL_Fails()
        {
            Client.Validate("");
        }

        [TestMethod]
        public void Validate_Complete_CSDL_Succeeds()
        {
            var response = Client.Validate("interaction.content contains \"music\"");
            Assert.AreEqual("0.1", response.Data.dpu);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Compile

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Compile_Null_CSDL_Fails()
        {
            Client.Compile(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Compile_Empty_CSDL_Fails()
        {
            Client.Compile("");
        }

        [TestMethod]
        public void Compile_Complete_CSDL_Succeeds()
        {
            var response = Client.Compile("interaction.content contains \"music\"");
            Assert.AreEqual("0.1", response.Data.dpu);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Usage

        [TestMethod]
        public void Usage()
        {
            var response = Client.Usage();

            var streams = (IDictionary<string, object>)response.Data.streams;
            var stream = (dynamic)streams["693f5134c73a62ed85ef271040bf266b"];

            Assert.AreEqual(3600, stream.seconds);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region DPU

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DPU_Null_Hash_Fails()
        {
            Client.DPU(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DPU_Empty_Hash_Fails()
        {
            Client.DPU("");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DPU_Bad_Format_Hash_Fails()
        {
            Client.DPU("hash");
        }

        [TestMethod]
        public void DPU_Complete_Hash_Succeeds()
        {
            var response = Client.DPU("9fe133a7ee1bd2757f1e26bd78342458");
            Assert.AreEqual(2, response.Data.detail.contains.count);
            Assert.AreEqual(0.2, response.Data.detail.contains.dpu);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region Balance

        [TestMethod]
        public void Balance()
        {
            var response = Client.Balance();
            Assert.AreEqual("Platinum", response.Data.balance.plan);
            Assert.AreEqual(249993.7, response.Data.balance.remaining_dpus);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

    }
}
