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
            Contract.Requires<ArgumentException>((page.HasValue) ? page.Value > 0 : true);
            Contract.Requires<ArgumentException>((perPage.HasValue) ? perPage.Value > 0 : true);
            
            return _client.GetRequest().Request("push/get", new { id = id, hash = hash, historics_id = historicsId, page = page, per_page = perPage, order_by = orderBy, order_dir = orderDirection, include_finished = includeFinished });
        }

        public RestAPIResponse Validate(string outputType)
        {
            return Validate(outputType, null);
        }

        public RestAPIResponse Validate(string outputType, dynamic outputParameters)
        {
            Contract.Requires<ArgumentNullException>(outputType != null);
            Contract.Requires<ArgumentException>(outputType.Trim().Length > 0);

            return _client.GetRequest().Request("push/validate", new { output_type = outputType, output_params = outputParameters }, Method.POST);
        }

        public RestAPIResponse Create(string name, string outputType, string hash = null, string historicsId = null, PushStatus? initialStatus = null, DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            return Create(name, outputType, null, hash, historicsId, initialStatus, start, end);
        }
        public RestAPIResponse Create(string name, string outputType, dynamic outputParameters, string hash = null, string historicsId = null, PushStatus? initialStatus = null, DateTimeOffset? start = null, DateTimeOffset? end = null)
        {
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Trim().Length > 0);
            Contract.Requires<ArgumentNullException>(outputType != null);
            Contract.Requires<ArgumentException>(outputType.Trim().Length > 0);
            Contract.Requires<ArgumentException>(hash != null || historicsId != null, "You must provide either a hash or historicsId");
            Contract.Requires<ArgumentException>(hash == null || historicsId == null, "You cannot specify both a hash AND historicsId"); 
            Contract.Requires<ArgumentException>((hash != null) ? hash.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((hash != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(hash) : true, "Hash should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((historicsId != null) ? historicsId.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((historicsId != null) ? new Regex(@"[a-z0-9]{20}").IsMatch(historicsId) : true, "Hash should be a 20 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((end != null && start != null) ? end > start : true, "If start and end are specified, end must be after start");

            return _client.GetRequest().Request("push/create", new { name = name, output_type = outputType, output_params = outputParameters, hash = hash, historics_id = historicsId, 
                initial_status = initialStatus, start = start, end = end }, Method.POST);
        }

        public RestAPIResponse Delete(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");
            
            return _client.GetRequest().Request("push/delete", new { id = id }, Method.DELETE);
        }

        public RestAPIResponse Stop(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("push/stop", new {id = id}, Method.PUT);
        }

        public RestAPIResponse Pause(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("push/pause", new { id = id }, Method.PUT);
        }

        public RestAPIResponse Resume(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("push/resume", new { id = id }, Method.PUT);
        }

        public RestAPIResponse Log(string id = null, int? page = null, int? perPage = null, OrderDirection? orderDirection = null) 
        {
            Contract.Requires<ArgumentException>((id != null) ? id.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((page.HasValue) ? page.Value > 0 : true);
            Contract.Requires<ArgumentException>((perPage.HasValue) ? perPage.Value > 0 : true);

            return _client.GetRequest().Request("push/log", new { id = id, page = page, perPage = perPage, order_dir = orderDirection });
        }

        public RestAPIResponse Update(string id, string name = null) 
        {
            return Update(id, null, name);
        }

        public RestAPIResponse Update(string id, dynamic outputParameters, string name = null) 
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("push/update", new { id = id, name = name, output_params = outputParameters }, Method.PUT);
        }
    }
}
