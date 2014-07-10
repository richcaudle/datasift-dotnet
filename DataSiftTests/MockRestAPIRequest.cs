using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSift.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;


namespace DataSiftTests
{
    public class MockRestAPIRequest : IRestAPIRequest
    {
        public RestAPIResponse Request(string endpoint, dynamic parameters = null, Method method = Method.GET)
        {
            string response = null;

            switch (endpoint)
            {
                case "validate":
                    response = MockAPIResponses.Default.Validate;
                    break;
                case "compile":
                    response = MockAPIResponses.Default.Compile;
                    break;
                case "usage":
                    response = MockAPIResponses.Default.Usage;
                    break;
                case "dpu":
                    response = MockAPIResponses.Default.DPU;
                    break;
                case "balance":
                    response = MockAPIResponses.Default.Balance;
                    break;

                case "historics/get":
                    response = MockAPIResponses.Default.HistoricsGet;
                    break;
            }

            var converter = new ExpandoObjectConverter();
            dynamic result = JsonConvert.DeserializeObject<ExpandoObject>(response, converter);

            return new RestAPIResponse() { Data = result };
            
        }
    }
}
