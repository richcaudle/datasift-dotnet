using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSift.Rest;
using RestSharp;
using DataSift.Enum;

namespace DataSift
{
    public class DataSift
    {
        private string _username;
        private string _apikey;
        private GetRequestDelegate _getRequest;
        private Historics _historics;
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

        #endregion

        #region Core API Endpoints

        public dynamic Validate(string csdl)
        {
            return GetRequest().Request("validate", new { csdl = csdl }, Method.POST);
        }

        public dynamic Compile(string csdl)
        {
            return GetRequest().Request("compile", new { csdl = csdl }, Method.POST);
        }

        public dynamic Usage(UsagePeriod? period = null)
        {
            return GetRequest().Request("usage", new { period = period.ToString().ToLower() });
        }

        public dynamic DPU(string hash)
        {
            return GetRequest().Request("dpu", new { hash = hash });
        }

        public dynamic Balance()
        {
            return GetRequest().Request("balance");
        }

        #endregion

    }
}
