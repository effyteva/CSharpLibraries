using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class ScriptRequestArguments
    {
        public ScriptRequestArguments()
        {
        }

        public ScriptRequestArguments(string Gremlin, Dictionary<string, object> Bindings, Guid? Session)
        {
            this.Gremlin = Gremlin;
            this.Bindings = Bindings;
            this.Session = Session;
        }

        [JsonProperty("gremlin")]
        public string Gremlin { get; set; }

        [JsonProperty("session", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Session { get; set; }

        [JsonProperty("bindings")]
        public Dictionary<string, object> Bindings { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; } = "gremlin-groovy";
    }
}
