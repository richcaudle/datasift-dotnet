using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataSiftTests
{
    [TestClass]
    public class Core : TestBase
    {
        
        [TestMethod]
        public void Validate()
        {
            var response = Client.Validate("interaction.content contains \"music\"");
            Assert.AreEqual("0.1", response.dpu);
        }

        [TestMethod]
        public void Compile()
        {
            var response = Client.Compile("interaction.content contains \"music\"");
            Assert.AreEqual("0.1", response.dpu);
        }

        [TestMethod]
        public void Usage()
        {
            var response = Client.Usage();

            var streams = (IDictionary<string, object>)response.streams;
            var stream = (dynamic)streams["693f5134c73a62ed85ef271040bf266b"];

            Assert.AreEqual(3600, stream.seconds);
        }

        [TestMethod]
        public void DPU()
        {
            var response = Client.DPU("9fe133a7ee1bd2757f1e26bd78342458");
            Assert.AreEqual(2, response.detail.contains.count);
            Assert.AreEqual(0.2, response.detail.contains.dpu);
        }

        [TestMethod]
        public void Balance()
        {
            var response = Client.Balance();
            Assert.AreEqual("Platinum", response.balance.plan);
            Assert.AreEqual(249993.7, response.balance.remaining_dpus);
        }
    }
}
