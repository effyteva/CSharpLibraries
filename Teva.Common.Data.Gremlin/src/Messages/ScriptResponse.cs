using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.Messages
{
    public class ScriptResponse<DataType>
    {
        [JsonProperty("result")]
        public ScriptResponseResult<DataType> Result { get; set; }

        [JsonProperty("requestId")]
        public Guid? RequestID { get; set; }

        [JsonProperty("status")]
        public ScriptResponseStatus Status { get; set; }
    }
}
