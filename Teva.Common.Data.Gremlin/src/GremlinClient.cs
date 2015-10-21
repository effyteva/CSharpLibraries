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
        public GremlinClient(string Host, int Port = 8182, string Username = null, string Password = null)
        {
            this.Host = Host;
            this.Port = Port;
            this.Client = new GremlinServerClient(Host, Port, Username: Username, Password: Password);
        }

        #region Vertices
        public GraphItems.Vertex GetVertex(GremlinScript Script)
        {
            return Client.ExecuteScalar<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }
        public Task<GraphItems.Vertex> GetVertexAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }

        public List<GraphItems.Vertex> GetVertices(GremlinScript Script)
        {
            return Client.Execute<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<GraphItems.Vertex>> GetVerticesAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }

        public GraphItems.Vertex CreateVertex(Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            return GetVertex(new GremlinScript().Append_CreateVertex(Properties));
        }
        public Task<GraphItems.Vertex> CreateVertexAsync(Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            return GetVertexAsync(new GremlinScript().Append_CreateVertex(Properties));
        }

        public void UpdateVertex(string ID, Dictionary<string, List<GraphItems.VertexValue>> Properties, bool RemoveOtherProperties)
        {
            Execute(new GremlinScript().Append_UpdateVertex(ID, Properties, RemoveOtherProperties).Append("null;"));
        }
        public Task UpdateVertexAsync(string ID, Dictionary<string, List<GraphItems.VertexValue>> Properties, bool RemoveOtherProperties)
        {
            return ExecuteAsync(new GremlinScript().Append_UpdateVertex(ID, Properties, RemoveOtherProperties).Append("null;"));
        }
        #endregion

        #region Edges
        public GraphItems.Edge CreateEdge(string StartVertexID, string EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            return GetEdge(new GremlinScript().Append_CreateEdge(StartVertexID, EndVertexID, Name, Properties));
        }
        public Task<GraphItems.Edge> CreateEdgeAsync(string StartVertexID, string EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            return GetEdgeAsync(new GremlinScript().Append_CreateEdge(StartVertexID, EndVertexID, Name, Properties));
        }

        public void DeleteEdge(string EdgeID)
        {
            Execute(new GremlinScript().Append_DeleteEdge(EdgeID));
        }
        public Task DeleteEdgeAsync(string EdgeID)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteEdge(EdgeID));
        }

        public GraphItems.Edge GetEdge(string ID)
        {
            return GetEdge(new GremlinScript().Append_GetEdge(ID));
        }
        public Task<GraphItems.Edge> GetEdgeAsync(string ID)
        {
            return GetEdgeAsync(new GremlinScript().Append_GetEdge(ID));
        }

        public GraphItems.Edge GetEdge(GremlinScript Script)
        {
            return Client.ExecuteScalar<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        public Task<GraphItems.Edge> GetEdgeAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }

        public List<GraphItems.Edge> GetEdges(GremlinScript Script)
        {
            return Client.Execute<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<GraphItems.Edge>> GetEdgesAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        #region GetArray
        public List<object> GetArray(GremlinScript Script)
        {
            return Client.Execute<object>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<object>> GetArrayAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<object>(Script.GetScript(), Script.GetBindings());
        }

        public List<ValueType> GetArray<ValueType>(GremlinScript Script)
        {
            return Client.Execute<ValueType>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<ValueType>> GetArrayAsync<ValueType>(GremlinScript Script)
        {
            return Client.ExecuteAsync<ValueType>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        #region Scalar
        public object GetScalar(GremlinScript Script)
        {
            return Client.ExecuteScalar<object>(Script.GetScript(), Script.GetBindings());
        }
        public Task<object> GetScalarAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<object>(Script.GetScript(), Script.GetBindings());
        }

        public ValueType GetScalar<ValueType>(GremlinScript Script)
        {
            return Client.ExecuteScalar<ValueType>(Script.GetScript(), Script.GetBindings());
        }
        public Task<ValueType> GetScalarAsync<ValueType>(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<ValueType>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        #region GetDictionaryArray
        public List<Dictionary<string, object>> GetDictionaryArray(GremlinScript Script)
        {
            return Client.Execute<Dictionary<string, object>>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<Dictionary<string, object>>> GetDictionaryArrayAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<Dictionary<string, object>>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        #region Execute
        public void Execute(GremlinScript Script)
        {
            Client.Execute<object>(Script.GetScript(), Script.GetBindings());
        }
        public Task ExecuteAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<object>(Script.GetScript(), Script.GetBindings());
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