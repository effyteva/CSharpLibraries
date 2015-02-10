using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class ScriptResponseStatus
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("attributes")]
        public object Attributes { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
