using DataSift.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace DataSift.Rest
{
    public class Historics
    {
        DataSift _client = null;

        internal Historics(DataSift client)
        {
            _client = client;
        }

        public RestAPIResponse Get(string id = null, int? max = null, int? page = null, bool? withEstimate = null)
        {
            Contract.Requires<ArgumentException>((id != null) ? id.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((max.HasValue) ? max.Value > 0 : true);
            Contract.Requires<ArgumentException>((page.HasValue) ? page.Value > 0 : true); 
            
            return _client.GetRequest().Request("historics/get", new { id = id, max = max, page = page, with_estimate = withEstimate });
        }

        public RestAPIResponse Prepare(string hash, DateTimeOffset start, DateTimeOffset end, string name, string[] sources, Sample? sample = null)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Trim().Length > 0);

            Contract.Requires<ArgumentNullException>(hash != null);
            Contract.Requires<ArgumentException>(hash.Trim().Length > 0);
            Contract.Requires<ArgumentException>(new Regex(@"[a-z0-9]{32}").IsMatch(hash), "Hash should be a 32 character string of lower-case letters and numbers");

            Contract.Requires<ArgumentException>(end < DateTimeOffset.Now.AddHours(-1), "End must be at least one hour ago");
            Contract.Requires<ArgumentException>(end > start, "Start date must be before end date");

            Contract.Requires<ArgumentNullException>(sources != null);
            Contract.Requires<ArgumentException>(sources.Length > 0);
        
            return _client.GetRequest().Request("historics/prepare", new { hash = hash, start = start, end = end, name = name, sources = sources, sample = sample }, Method.POST);
        }

        public RestAPIResponse Delete(string id) 
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("historics/delete", new { id = id }, Method.DELETE); 
        }

        public RestAPIResponse Start(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("historics/start", new { id = id }, Method.PUT); 
        }

        public RestAPIResponse Stop(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("historics/stop", new { id = id }, Method.PUT); 
        }

        public RestAPIResponse Update(string id, string name) 
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");

            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Trim().Length > 0);

            return _client.GetRequest().Request("historics/update", new { id = id, name = name });
        }

        public RestAPIResponse Status(DateTimeOffset start, DateTimeOffset end, string[] sources) 
        {
            Contract.Requires<ArgumentException>(end < DateTimeOffset.Now, "End cannot be in the future");
            Contract.Requires<ArgumentException>(end > start, "Start date must be before end date");

            Contract.Requires<ArgumentNullException>(sources != null);
            Contract.Requires<ArgumentException>(sources.Length > 0);

            return _client.GetRequest().Request("historics/status", new { start = start, end = end, sources = sources });
        }

        public RestAPIResponse Pause(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("historics/pause", new { id = id }, Method.PUT); 
        }

        public RestAPIResponse Resume(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(id) : true, "ID should be a 20 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("historics/resume", new { id = id }, Method.PUT); 
        }

    }
}
