using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataSift.Rest;
using RestSharp;
using DataSift.Enum;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
using DataSift.Streaming;
using WebSocket4Net;

namespace DataSift
{
    public class DataSift
    {
        private string _username;
        private string _apikey;
        private GetAPIRequestDelegate _getRequest;
        private GetStreamConnectionDelegate _getConnection;
        private Historics _historics;
        private HistoricsPreview _historicsPreview;
        private Source _source;
        private Push _push;
        private List _list;
        public delegate IRestAPIRequest GetAPIRequestDelegate(string username, string apikey);
        public delegate IStreamConnection GetStreamConnectionDelegate(string url);

        public DataSift(string username, string apikey, GetAPIRequestDelegate requestCreator = null, GetStreamConnectionDelegate connectionCreator = null)
        {
            Contract.Requires<ArgumentNullException>(username != null);
            Contract.Requires<ArgumentException>(username.Trim().Length > 0);
            Contract.Requires<ArgumentNullException>(apikey != null);
            Contract.Requires<ArgumentException>(apikey.Trim().Length > 0);
            Contract.Requires<ArgumentException>(new Regex(@"[a-z0-9]{32}").IsMatch(apikey), "API key should be a 32 character string of lower-case letters and numbers");

            _username = username;
            _apikey = apikey;

            if (requestCreator == null)
                _getRequest = GetRequestDefault;
            else
                _getRequest = requestCreator;

            _getConnection = connectionCreator;
        }

        #region Mocking / Faking

        private IRestAPIRequest GetRequestDefault(string username, string apikey)
        {
            return new RestAPIRequest(username, apikey);
        }

        internal IRestAPIRequest GetRequest()
        {
            return _getRequest(_username, _apikey);
        }

        #endregion

        #region Properties

        public Source Source
        {
            get
            {
                if (_source == null) _source = new Source(this);
                return _source;
            }
        }
        public List List
        {
            get
            {
                if (_list == null) _list = new List(this);
                return _list;
            }
        }

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

        public HistoricsPreview HistoricsPreview
        {
            get
            {
                if (_historicsPreview == null) _historicsPreview = new HistoricsPreview(this);
                return _historicsPreview;
            }
        }

        #endregion

        #region Streaming

        public DataSiftStream Connect(bool secure = true)
        {
            var stream = new DataSiftStream(_getConnection);
            stream.Connect(_username, _apikey, secure);
            return stream;
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

        public PullAPIResponse Pull(string id, int? size = null, string cursor = null)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(id.Trim().Length > 0);
            Contract.Requires<ArgumentException>((id != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(id) : true, "ID should be a 32 character string of lower-case letters and numbers");
            Contract.Requires<ArgumentException>((size.HasValue) ? size > 0: true);
            Contract.Requires<ArgumentException>((cursor != null) ? cursor.Trim().Length > 0 : true);
            Contract.Requires<ArgumentException>((cursor != null) ? new Regex(@"[a-z0-9]{32}").IsMatch(cursor) : true, "Cursor should be a 32 character string of lower-case letters and numbers");

            return (PullAPIResponse)GetRequest().Request("pull", new { id = id, size = size, cursor = cursor });
        }

        #endregion

    }
}
