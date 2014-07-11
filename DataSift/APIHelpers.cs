using DataSift.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataSift
{
    public class APIHelpers
    {
        public static dynamic DeserializeResponse(string data)
        {
            // Empty responses
            if (String.IsNullOrWhiteSpace(data)) return null;
            if (data.Trim() == "[]") return null;

            data = data.Trim();

            if(data.StartsWith("["))
            {
                var converter = new ExpandoObjectConverter();
                return JsonConvert.DeserializeObject<List<ExpandoObject>>(data, converter);
            }
            else 
            {
                var converter = new ExpandoObjectConverter();
                return JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
            }
        }

        public static bool HasAttr(dynamic expando, string key)
        {
            return ((IDictionary<string, object>)expando).ContainsKey(key);
        }

        public static RateLimitInfo ParseReturnedHeaders(IList<Parameter> headers)
        {
            var result = new RateLimitInfo();

            foreach (var header in headers)
            {
                switch (header.Name)
                {
                    case "X-RateLimit-Limit":
                        result.Limit = int.Parse((string)header.Value);
                        break;
                    case "X-RateLimit-Remaining":
                        result.Remaining = int.Parse((string)header.Value);
                        break;
                    case "X-RateLimit-Cost":
                        result.Cost = int.Parse((string)header.Value);
                        break;
                }
            }

            return result;
        }

        public static List<Parameter> ParseParameters(dynamic parameters)
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
                        val = String.Join(",", val);

                    if (prop.PropertyType == typeof(DateTimeOffset))
                        val = ToUnixTime(val);

                    result.Add(new Parameter()
                    {
                        Name = prop.Name,
                        Value = val,
                        Type = ParameterType.GetOrPost
                    });
                }
            }

            return result;
        }

        public static string GetEnumDescription(dynamic enumerationValue)
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

        public static int ToUnixTime(DateTimeOffset time)
        {
            return (int)(time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}
