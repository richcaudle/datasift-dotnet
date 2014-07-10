using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System.ComponentModel;


namespace DataSift.Rest
{
    internal class RestAPIRequest : IRestAPIRequest
    {
        private RestClient _client;

        internal RestAPIRequest(string username, string apikey)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            _client = new RestClient("https://api.datasift.com/v1");
            _client.Authenticator = new HttpBasicAuthenticator(username, apikey);

            // TODO: Get version from external file
            _client.UserAgent = "DataSift/v1 Dotnet/v0.1";
        }

        public RestAPIResponse Request(string endpoint, dynamic parameters = null, RestSharp.Method method = Method.GET)
        {
            var request = new RestRequest(endpoint, method);
            if (parameters != null) request.Parameters.AddRange(ParseParameters(parameters));

            // TODO: Interpret return code 

            IRestResponse response = _client.Execute(request);
            var result = new RestAPIResponse() { RateLimit = new RateLimitInfo() };

            // Parse returned headers
            foreach (var header in response.Headers)
            {
                switch (header.Name)
                {
                    case "X-RateLimit-Limit":
                        result.RateLimit.Limit = int.Parse((string)header.Value);
                        break;
                    case "X-RateLimit-Remaining":
                        result.RateLimit.Remaining = int.Parse((string)header.Value);
                        break;
                    case "X-RateLimit-Cost":
                        result.RateLimit.Cost = int.Parse((string)header.Value);
                        break;
                }
            }
            
            if (!String.IsNullOrWhiteSpace(response.Content) && response.Content != "[]")
            {
                var converter = new ExpandoObjectConverter();
                result.Data = JsonConvert.DeserializeObject<ExpandoObject>(response.Content, converter);
            }

            return result;

        }

        private List<Parameter> ParseParameters(dynamic parameters)
        {
            List<Parameter> result = new List<Parameter>();

            foreach (var prop in parameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var val = prop.GetValue(parameters, null);
                if (val != null)
                {
                    if (val.GetType().IsEnum)
                        val = GetEnumDescription(val);

                    if (val.GetType().IsArray)
                        val = String.Join(",",val);

                    if (prop.PropertyType == typeof(DateTimeOffset))
                        val = ToUnixTime(val);

                    result.Add(new Parameter() { Name = prop.Name, Value = val, 
                        Type = ParameterType.GetOrPost
                    });
                }
            }

            return result;
        } 

        private string GetEnumDescription(dynamic enumerationValue)
        {
            Type type = enumerationValue.GetType();
           
            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        private int ToUnixTime(DateTimeOffset time)
        {
            return (int)(time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}
