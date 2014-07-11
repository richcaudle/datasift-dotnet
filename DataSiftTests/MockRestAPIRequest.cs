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
using System.Net;
using DataSift;


namespace DataSiftTests
{
    public class MockRestAPIRequest : IRestAPIRequest
    {
        public RestAPIResponse Request(string endpoint, dynamic parameters = null, Method method = Method.GET)
        {
            string response = null;
            RestAPIResponse result = new RestAPIResponse();

            List<Parameter> prms = new List<Parameter>();
            if(parameters != null) prms = APIHelpers.ParseParameters(parameters);

            switch (endpoint)
            {
                case "validate":
                    response = MockAPIResponses.Default.Validate;
                    result.StatusCode = HttpStatusCode.OK;
                    break;
                case "compile":
                    response = MockAPIResponses.Default.Compile;
                    result.StatusCode = HttpStatusCode.OK;
                    break;
                case "usage":
                    response = MockAPIResponses.Default.Usage;
                    result.StatusCode = HttpStatusCode.OK;
                    break;
                case "dpu":
                    response = MockAPIResponses.Default.DPU;
                    result.StatusCode = HttpStatusCode.OK;
                    break;
                case "balance":
                    response = MockAPIResponses.Default.Balance;
                    result.StatusCode = HttpStatusCode.OK;
                    break;

                case "historics/get":

                    if (prms.Exists(p => p.Name == "id"))
                        response = MockAPIResponses.Default.HistoricsGetById;
                    else if (prms.Exists(p => p.Name == "max"))
                        response = MockAPIResponses.Default.HistoricsGetMax1;
                    else if (prms.Exists(p => p.Name == "with_estimate"))
                        response = MockAPIResponses.Default.HistoricsGetWithCompletion;
                    else
                        response = MockAPIResponses.Default.HistoricsGet;

                    result.StatusCode = HttpStatusCode.OK;
                    break;

                case "historics/prepare":
                    response = MockAPIResponses.Default.HistoricsPrepare;
                    result.StatusCode = HttpStatusCode.OK;
                    break;

                case "historics/delete":
                    result.StatusCode = HttpStatusCode.NoContent;
                    break;

                case "historics/status":
                    response = MockAPIResponses.Default.HistoricsStatus;
                    result.StatusCode = HttpStatusCode.OK;
                    break;

                case "historics/update":
                    result.StatusCode = HttpStatusCode.NoContent;
                    break;

                case "push/get":

                    
                    if (prms.Exists(p => p.Name == "id"))
                        response = MockAPIResponses.Default.PushGetById;
                    //else if (prms.Exists(p => p.Name == "max"))
                    //    response = MockAPIResponses.Default.HistoricsGetMax1;
                    //else if (prms.Exists(p => p.Name == "with_estimate"))
                    //    response = MockAPIResponses.Default.HistoricsGetWithCompletion;
                    else
                        response = MockAPIResponses.Default.PushGet;

                    result.StatusCode = HttpStatusCode.OK;
                    break;
            }

            if(response != null)
            {
                result.Data = APIHelpers.DeserializeResponse(response);
            }
            
            return result;
            
        }


    }
}
