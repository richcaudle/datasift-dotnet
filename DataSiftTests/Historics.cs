using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataSiftTests
{
    [TestClass]
    public class Historics : TestBase
    {
        [TestMethod]
        public void HistoricsGet()
        {
            var response = Client.Historics.Get();
            Assert.AreEqual("twitter", response.data[0].name);
        }
    }
}
