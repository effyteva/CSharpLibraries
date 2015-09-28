using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin
{
    public class GremlinClient
    {
        public GremlinClient(string Host, int Port)
        {
            this.Host = Host;
            this.Port = Port;
            this.Client = new GremlinServerClient(Host, Port);
        }

        #region Vertices
        public GraphItems.Vertex CreateVertex(Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            var Script = new GremlinScript();
            Script.Append_CreateVertex(Properties);
            return GetVertex(Script);
        }
        public GraphItems.Vertex GetVertex(string ID)
        {
            var Script = new GremlinScript();
            Script.Append_GetVertex(ID);
            return GetVertex(Script);
        }
        public GraphItems.Vertex GetVertex(GremlinScript Script)
        {
            var Vertices = Client.Send<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
            if (Vertices == null || Vertices.Count == 0)
                return null;
            return Vertices.First();
        }
        public GraphItems.Vertex GetVertexByIndex(string IndexName, object ID)
        {
            var Script = new GremlinScript();
            Script.Append_GetVertexByIndex(IndexName, ID);
            return GetVertex(Script);
        }
        public List<GraphItems.Vertex> GetVerticesByIndex(string IndexName, IEnumerable<object> ID)
        {
            var Script = new GremlinScript();
            Script.Append_GetVerticesByIndex(IndexName, ID);
            return GetVertices(Script);
        }
        public void UpdateVertex(string ID, Dictionary<string, List<GraphItems.VertexValue>> Properties, bool RemoveOtherProperties)
        {
            var Script = new GremlinScript();
            Script.Append_UpdateVertex(ID, Properties, RemoveOtherProperties);
            Script.Append("null;");
            Execute(Script);
        }

        public List<GraphItems.Vertex> GetVertices(GremlinScript Script)
        {
            return Client.Send<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }
        public IEnumerable<T> ReadVertices<T>(Func<GraphItems.Vertex, T> ReadMethod, GremlinScript Script)
        {
            foreach (var Vertex in GetVertices(Script))
                yield return ReadMethod(Vertex);
        }
        public IEnumerable<T> ReadVertices<T>(GremlinScript Script, Func<GraphItems.Vertex, T> ReadMethod)
        {
            foreach (var Vertex in GetVertices(Script))
                yield return ReadMethod(Vertex);
        }
        #endregion

        #region Edges
        public GraphItems.Edge CreateEdge(string StartVertexID, string EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            var Script = new GremlinScript();
            Script.Append_CreateEdge(StartVertexID, EndVertexID, Name, Properties);
            return GetEdge(Script);
        }
        public void DeleteEdge(string EdgeID)
        {
            var Script = new GremlinScript();
            Script.Append_DeleteEdge(EdgeID);
            Execute(Script);
        }
        public GraphItems.Edge GetEdge(string ID)
        {
            var Script = new GremlinScript();
            Script.Append_GetEdge(ID);
            return GetEdge(Script);
        }
        public GraphItems.Edge GetEdge(GremlinScript Script)
        {
            var Edges = Client.Send<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
            if (Edges == null || Edges.Count == 0)
                return null;
            return Edges.First();
        }
        public IEnumerable<GraphItems.Edge> GetEdges(GremlinScript Script)
        {
            return Client.Send<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        public IEnumerable<T> ReadEdges<T>(GremlinScript Script, Func<GraphItems.Edge, T> ReadMethod)
        {
            foreach (var Edge in GetEdges(Script))
                yield return ReadMethod(Edge);
        }
        #endregion

        #region GetArray
        public List<object> GetArray(GremlinScript Script)
        {
            try
            {
                return Client.Send<object>(Script.GetScript(), Script.GetBindings());
            }
            catch (Exception E)
            {
                throw new Exception(E.Message + "\r\n" + Script.GetReadableScript(), E);
            }
        }
        #endregion

        #region GetDictionaryArray
        public List<Dictionary<string, object>> GetDictionaryArray(GremlinScript Script)
        {
            try
            {
                return Client.Send<Dictionary<string, object>>(Script.GetScript(), Script.GetBindings());
            }
            catch (Exception E)
            {
                throw new Exception(E.Message + "\r\n" + Script.GetReadableScript(), E);
            }
        }
        #endregion

        #region GetScalar
        public object GetScalar(GremlinScript Script)
        {
            var Results = Client.Send<object>(Script.GetScript(), Script.GetBindings());
            if (Results == null || Results.Count == 0)
                return null;
            return Results.First();
        }
        #endregion

        #region Execute
        public void Execute(GremlinScript Script)
        {
            Client.Send<object>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        private GremlinServerClient Client { get; set; }
        public string Host { get; private set; }
        public int Port { get; private set; }

        public List<GraphItems.Vertex> GetVerticesFromJArray(object Object)
        {
            if (Object == null)
                return null;
            var Array = (JArray)Object;
            return Array.ToObject<List<GraphItems.Vertex>>();
        }
        public Dictionary<string, object> GetDictionaryFromJArray(object Object)
        {
            if (Object == null)
                return null;
            var Array = (JArray)Object;
            if (Array.Count == 0)
                return null;
            if (Array.Count != 1)
                throw new Exception("Invalid JArray length");
            return Array[0].ToObject<Dictionary<string, object>>();
        }
        public List<Dictionary<string, object>> GetDictionariesFromJArray(object Object)
        {
            if (Object == null)
                return null;
            var Array = (JArray)Object;
            if (Array.Count == 0)
                return null;
            return Array.Select(T =>
            {
                if (T.Type == JTokenType.Array && T.Count() == 1)
                    return T.First.ToObject<Dictionary<string, object>>();
                else
                    return T.ToObject<Dictionary<string, object>>();
            }).ToList();
        }
        public GraphItems.Vertex GetVertexFromJObject(object Object)
        {
            return GetValueFromJObject<GraphItems.Vertex>(Object);
        }
        public Dictionary<string, object> GetDictionaryFromJObject(object Object)
        {
            return GetValueFromJObject<Dictionary<string, object>>(Object);
        }
        public T GetValueFromJObject<T>(object Object)
        {
            if (Object == null)
                return default(T);
            return ((JObject)Object).ToObject<T>();
        }
    }
}