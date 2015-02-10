using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class ScriptResponseResult<DataType>
    {
        [JsonProperty("data")]
        public List<DataType> Data { get; set; }

        [JsonProperty("meta")]
        public object Meta { get; set; }
    }
}
