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
        public static dynamic DeserializeResponse(string data, string format = null)
        {
            // Empty responses
            if (String.IsNullOrWhiteSpace(data)) return null;
            if (data.Trim() == "[]") return null;

            var converter = new ExpandoObjectConverter();
            data = data.Trim();

            if(format != null)
            {
                // Data response (such as from Pull requests)
                switch(format)
                {
                    case "json_meta":
                        return JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
                    case "json_array":
                        return JsonConvert.DeserializeObject<List<ExpandoObject>>(data, converter);
                    case "json_new_line":

                        var items = new List<ExpandoObject>();

                        foreach(var line in data.Split('\n'))
                        {
                            items.Add(JsonConvert.DeserializeObject<ExpandoObject>(line, converter));
                        }
                        return items;

                    default:
                        throw new ArgumentException("Unrecognised serialization format for data", "format");
                }

            }
            else
            {
                // Standard API responses

                if (data.StartsWith("["))
                {
                    return JsonConvert.DeserializeObject<List<ExpandoObject>>(data, converter);
                }
                else
                {
                    return JsonConvert.DeserializeObject<ExpandoObject>(data, converter);
                }
            }
            
        }

        public static bool HasAttr(dynamic expando, string key)
        {
            return ((IDictionary<string, object>)expando).ContainsKey(key);
        }

        public static RateLimitInfo ParseRateLimitHeaders(IList<Parameter> headers)
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

        public static PullInfo ParsePullDetailHeaders(IList<Parameter> headers)
        {
            var result = new PullInfo();

            foreach (var header in headers)
            {
                switch (header.Name)
                {
                    case "X-DataSift-Format":
                        result.Format = (string)header.Value;
                        break;
                    case "X-DataSift-Cursor-Current":
                        result.CursorCurrent = (string)header.Value;
                        break;
                    case "X-DataSift-Cursor-Next":
                        result.CursorNext = (string)header.Value;
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
                    else if (val.GetType().IsArray)
                        val = String.Join(",", val);
                    else if (prop.PropertyType == typeof(DateTimeOffset))
                        val = ToUnixTime(val);
                    else if (val.GetType().IsGenericType)
                        val = JsonConvert.SerializeObject(val);

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
