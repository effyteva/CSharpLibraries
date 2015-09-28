#define UseIndexes

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.GraphItems
{
    public class Vertex : GraphItem
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public VertexProperties Properties { get; set; }

        public long VertexID
        {
            get
            {
#if UseIndexes
                return ExplicitID.Value;
#else
                return ID;
#endif
            }
        }
        public long? ExplicitID
        {
            get
            {
                return Properties.GetProperty<long?>("explicitid");
            }
        }
    }
}
