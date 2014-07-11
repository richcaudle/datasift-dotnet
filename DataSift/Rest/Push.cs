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
    public class Push
    {
        DataSift _client = null;

        internal Push(DataSift client)
        {
            _client = client;
        }

        public RestAPIResponse Get(string id = null, string hash = null, string historicsId = null, int? page = null, int? perPage = null, OrderBy? orderBy = null, OrderDirection? orderDirection = null, bool? includeFinished = null)
        {
            Contract.Requires<ArgumentException>((id != null) ? id.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((hash != null) ? hash.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((hash != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(hash) : true, "Hash should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((historicsId != null) ? historicsId.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((historicsId != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(historicsId) : true, "Hash should be a 20 character string of lower-case letters and numbers");
           
            return _client.GetRequest().Request("push/get", new { id = id, hash = hash, historics_id = historicsId, page = page, per_page = perPage, order_by = orderBy, order_dir = orderDirection, include_finished = includeFinished });
        }

    }
}
