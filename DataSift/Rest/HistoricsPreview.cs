using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataSift.Rest
{
    public class HistoricsPreview
    {
        DataSift _client = null;

        internal HistoricsPreview(DataSift client)
        {
            _client = client;
        }
        public RestAPIResponse Create(string hash, string[] sources, List<HistoricsPreviewParameter> parameters, DateTimeOffset start, DateTimeOffset? end = null)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            Contract.Requires<ArgumentException>(hash.Trim().Length > 0);
            Contract.Requires<ArgumentException>(new Regex(@"[a-z0-9]{32}").IsMatch(hash), "Hash should be a 32 character string of lower-case letters and numbers");

            Contract.Requires<ArgumentNullException>(sources != null);
            Contract.Requires<ArgumentException>(sources.Length > 0);

            Contract.Requires<ArgumentException>(start >= new DateTimeOffset(2010, 1, 1, 0, 0, 0, TimeSpan.Zero), "Start must be at least one hour ago");
            Contract.Requires<ArgumentException>(start <= DateTimeOffset.Now.AddHours(-2), "Start must be at least two hours ago");
            Contract.Requires<ArgumentException>((end.HasValue) ? end <= DateTimeOffset.Now.AddHours(-1) : true, "End must be at least one hour ago");
            Contract.Requires<ArgumentException>((end.HasValue) ? end > start : true, "Start date must be before end date");

            Contract.Requires<ArgumentNullException>(parameters != null);
            Contract.Requires<ArgumentException>(parameters.Count > 0);
            Contract.Requires<ArgumentException>(parameters.Count <= 20);

            return _client.GetRequest().Request("preview/create", new { hash = hash, sources = sources, parameters = parameters, start = start, end = end }, Method.POST);
        }

        public RestAPIResponse Get(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("preview/get", new { id= id });
        }

    }
}
