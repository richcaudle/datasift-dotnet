using DataSift.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace DataSift.Rest
{
    public class Historics
    {
        DataSift _client = null;

        internal Historics(DataSift client)
        {
            _client = client;
        }

        public dynamic Get(string id = null, int? max = null, int? page = null, bool? withEstimate = null)
        {
            return _client.GetRequest().Request("historics/get", new { id = id, max = max, page = page, with_estimate = withEstimate });
        }

        public dynamic Prepare(string hash, DateTimeOffset start, DateTimeOffset end, string name, string[] sources, Sample? sample = null)
        {
            return _client.GetRequest().Request("historics/prepare", new { hash = hash, start = start, end = end, name = name, sources = sources, sample = sample }, Method.POST);
        }

        public dynamic Delete() { return null; }

        public dynamic Start() { return null;  }

        public dynamic Stop() { return null;  }

        public dynamic Update() { return null;  }

        public dynamic Status() {  return null; }

        public dynamic Pause() { return null;  }

        public dynamic Resume() { return null;  }

    }
}
