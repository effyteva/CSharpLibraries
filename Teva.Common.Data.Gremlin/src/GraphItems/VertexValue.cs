using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.GraphItems
{
    public class VertexValue : GraphItem
    {
        public VertexValue()
        {
        }

        public VertexValue(object Contents)
        {
            this.Contents = Contents;
        }

        [JsonProperty("value")]
        public object Contents { get; set; }
    }
}
