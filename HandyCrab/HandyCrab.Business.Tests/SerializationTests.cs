using System.Collections.Generic;
using HandyCrab.Common.Entitys;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace HandyCrab.Business.Tests
{
    [TestClass]
    public class SerializationTests
    {

        [TestMethod]
        public void StringsWithinBarrierAndSolutionSerializesCorrectly()
        {
            var json =
                "[{\"_id\": \"5ea91df7bcfd422f07270cfc\",\"userId\": \"5e8ef9daf4924d7b1d256ace\",\"title\": \"Title\",\"longitude\": 0,\"latitude\": 0,\"description\": \"Desc\",\"postcode\": \"0000\",\"solutions\": [{\"_id\": \"5ea91df7bcfd422f07270cfb\",\"userId\": \"5e8ef9daf4924d7b1d256ace\",\"text\": \"Solution\",\"upVotes\": 0,\"downVotes\": 0,\"vote\": \"NONE\"}],\"upVotes\": 0,\"downVotes\": 0,\"vote\": \"NONE\"}]";
            var result = JsonConvert.DeserializeObject<List<Barrier>>(json);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.IsNotNull(result[0]);
            Assert.AreEqual("Title", result[0].Title);
            Assert.AreEqual("Desc", result[0].Description);
            Assert.IsNotNull(result[0].Solutions);
            Assert.IsNotNull(result[0].Solutions[0]);
            Assert.AreEqual("Solution", result[0].Solutions[0].Text);
        }
    }
}