﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSift.Rest;
using RestSharp;
using DataSift.Enum;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace DataSift
{
    public class DataSift
    {
        private string _username;
        private string _apikey;
        private GetRequestDelegate _getRequest;
        private Historics _historics;
        private Push _push;
        public delegate IRestAPIRequest GetRequestDelegate(string username, string apikey);

        public DataSift(string username, string apikey, GetRequestDelegate requestCreator = null)
        {
            _username = username;
            _apikey = apikey;

            if (requestCreator == null)
                _getRequest = GetRequestDefault;
            else
                _getRequest = requestCreator;
        }

        private IRestAPIRequest GetRequestDefault(string username, string apikey)
        {
            return new RestAPIRequest(username, apikey);
        }

        internal IRestAPIRequest GetRequest()
        {
            return _getRequest(_username, _apikey);
        }

        #region Properties

        public Historics Historics
        {
            get
            {
                if (_historics == null) _historics = new Historics(this);
                return _historics;
            }
        }

        public Push Push
        {
            get
            {
                if (_push == null) _push = new Push(this);
                return _push;
            }
        }

        #endregion

        #region Core API Endpoints

        public RestAPIResponse Validate(string csdl)
        {
            Contract.Requires<ArgumentNullException>(csdl != null);
            Contract.Requires<ArgumentException>(csdl.Trim().Length > 0);

            return GetRequest().Request("validate", new { csdl = csdl }, Method.POST);
        }

        public RestAPIResponse Compile(string csdl)
        {
            Contract.Requires<ArgumentNullException>(csdl != null);
            Contract.Requires<ArgumentException>(csdl.Trim().Length > 0);

            return GetRequest().Request("compile", new { csdl = csdl }, Method.POST);
        }

        public RestAPIResponse Usage(UsagePeriod? period = null)
        {
            return GetRequest().Request("usage", new { period = period.ToString().ToLower() });
        }

        public RestAPIResponse DPU(string hash)
        {
            Contract.Requires<ArgumentNullException>(hash != null);
            Contract.Requires<ArgumentException>(hash.Trim().Length > 0);
            Contract.Requires<ArgumentException>(new Regex(@"[a-z0-9]{32}").IsMatch(hash), "Hash should be a 32 character string of lower-case letters and numbers");

            return GetRequest().Request("dpu", new { hash = hash });
        }

        public RestAPIResponse Balance()
        {
            return GetRequest().Request("balance");
        }

        #endregion

    }
}
