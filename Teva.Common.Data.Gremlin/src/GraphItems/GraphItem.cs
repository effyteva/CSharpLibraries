using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.GraphItems
{
    public abstract class GraphItem
    {
#warning Should be string
        [JsonProperty("id")]
        public long ID { get; set; }
    }
}
