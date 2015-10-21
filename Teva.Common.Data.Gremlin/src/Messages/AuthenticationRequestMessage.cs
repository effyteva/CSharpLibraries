using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class AuthenticationRequestMessage : RequestMessage
    {
        public AuthenticationRequestMessage()
        {
            Operation = "authentication";
        }

        [JsonProperty("args", Order = 3)]
        public AuthenticationRequestArguments Arguments { get; set; }
    }
}
