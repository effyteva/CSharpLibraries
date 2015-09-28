using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.GraphItems
{
    public class Edge : GraphItem
    {
        [JsonProperty("inV")]
        public string InVertex { get; set; }

        [JsonProperty("inVLabel")]
        public string InVertexLabel { get; set; }

        [JsonProperty("outV")]
        public string OutVertex { get; set; }

        [JsonProperty("outVLabel")]
        public string OutVertexLabel { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public EdgeProperties Properties { get; set; }
    }
}