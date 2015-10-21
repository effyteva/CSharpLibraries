using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class AuthenticationRequestArguments
    {
        public AuthenticationRequestArguments()
        {
        }

        public AuthenticationRequestArguments(string Username, string Password)
            : this()
        {
            this.SASL = "\0" + Username + "\0" + Password;
        }

        [JsonProperty("sasl")]
        public string SASL { get; set; }
    }
}
