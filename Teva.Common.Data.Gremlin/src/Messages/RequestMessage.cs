using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class RequestMessage
    {
        public RequestMessage()
        {
            RequestID = Guid.NewGuid();
            Operation = "eval";
            Processor = "";
        }

        [JsonProperty("requestId", Order = 0)]
        public Guid RequestID { get; set; }

        [JsonProperty("op", Order = 1)]
        public string Operation { get; set; }

        [JsonProperty("processor", Order = 2)]
        public string Processor { get; set; }
    }
}
