﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataSift.Rest
{
    public class Source
    {
        DataSift _client = null;

        internal Source(DataSift client)
        {
            _client = client;
        }

        public RestAPIResponse Start(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("source/start", new { id = id }, Method.PUT);
        }

        public RestAPIResponse Create(string sourceType, string name, dynamic parameters, dynamic resources, dynamic auth)
        {
            Contract.Requires<ArgumentNullException>(sourceType != null);
            Contract.Requires<ArgumentException>(sourceType.Trim().Length > 0);
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Trim().Length > 0);

            return _client.GetRequest().Request("source/create", new { source_type = sourceType, name = name, parameters = parameters, resources = resources, auth = auth } );
        }
        public RestAPIResponse Stop(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("source/stop", new { id = id }, Method.PUT);
        }

        public RestAPIResponse Log(string id, int? page = null, int? perPage = null)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((page.HasValue) ? page.Value > 0 : true);
            Contract.Requires<ArgumentException>((perPage.HasValue) ? perPage.Value > 0 : true);

            return _client.GetRequest().Request("source/log", new { id = id, page = page, per_page = perPage });
        }

        public RestAPIResponse Update(string sourceType, string name, string id, dynamic parameters, dynamic resources, dynamic auth)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            Contract.Requires<ArgumentNullException>(sourceType != null);
            Contract.Requires<ArgumentException>(sourceType.Trim().Length > 0);
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Trim().Length > 0);

            return _client.GetRequest().Request("source/update", new { source_type = sourceType, name = name, id = id, parameters = parameters, resources = resources, auth = auth });
        }

        public RestAPIResponse Delete(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");

            return _client.GetRequest().Request("source/delete", new { id = id }, Method.DELETE);
        }

        public RestAPIResponse Get(string sourceType = null, int? page = null, int? perPage = null, string id = null)
        {
            Contract.Requires<ArgumentException>((id != null) ? id.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((sourceType != null) ? sourceType.Trim().Length > 0 : true); 
            Contract.Requires<ArgumentException>((page.HasValue) ? page.Value > 0 : true);
            Contract.Requires<ArgumentException>((perPage.HasValue) ? perPage.Value > 0 : true);

            return _client.GetRequest().Request("source/get", new { source_type = sourceType, page = page, per_page = perPage, id = id });
        }

    }
}
