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

        #region VertexExists
        public bool VertexExistsByIndex(string IndexName, object ID)
        {
            return GetBoolean(new GremlinScript().Append_VertexExistsByIndex(IndexName, ID));
        }
        public Task<bool> VertexExistsByIndexAsync(string IndexName, object ID)
        {
            return GetBooleanAsync(new GremlinScript().Append_VertexExistsByIndex(IndexName, ID));
        }

        public bool VertexExistsByIndexAndLabel(string Label, string IndexName, object ID)
        {
            return GetBoolean(new GremlinScript().Append_VertexExistsByIndexAndLabel(Label, IndexName, ID));
        }
        public Task<bool> VertexExistsByIndexAndLabelAsync(string Label, string IndexName, object ID)
        {
            return GetBooleanAsync(new GremlinScript().Append_VertexExistsByIndexAndLabel(Label, IndexName, ID));
        }
        #endregion

        #region GetVertex
        public GraphItems.Vertex GetVertex(GremlinScript Script)
        {
            return Client.ExecuteScalar<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }
        public Task<GraphItems.Vertex> GetVertexAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }

        public GraphItems.Vertex GetVertex(string ID)
        {
            return GetVertex(new GremlinScript().Append_GetVertex(ID));
        }
        public Task<GraphItems.Vertex> GetVertexAsync(string ID)
        {
            return GetVertexAsync(new GremlinScript().Append_GetVertex(ID));
        }

        public GraphItems.Vertex GetVertexByIndex(string IndexName, object ID)
        {
            return GetVertex(new GremlinScript().Append_GetVerticesByIndex(IndexName, ID));
        }
        public Task<GraphItems.Vertex> GetVertexByIndexAsync(string IndexName, object ID)
        {
            return GetVertexAsync(new GremlinScript().Append_GetVerticesByIndex(IndexName, ID));
        }

        public GraphItems.Vertex GetVertexByIndexAndLabel(string Label, string IndexName, object ID)
        {
            return GetVertex(new GremlinScript().Append_GetVerticesByIndexAndLabel(Label, IndexName, ID));
        }
        public Task<GraphItems.Vertex> GetVertexByIndexAndLabelAsync(string Label, string IndexName, object ID)
        {
            return GetVertexAsync(new GremlinScript().Append_GetVerticesByIndexAndLabel(Label, IndexName, ID));
        }
        #endregion

        #region GetVertices
        public List<GraphItems.Vertex> GetVertices(GremlinScript Script)
        {
            return Client.Execute<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<GraphItems.Vertex>> GetVerticesAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<GraphItems.Vertex>(Script.GetScript(), Script.GetBindings());
        }

        public List<GraphItems.Vertex> GetVerticesByIndex(string IndexName, object ID)
        {
            return GetVertices(new GremlinScript().Append_GetVerticesByIndex(IndexName, ID));
        }
        public Task<List<GraphItems.Vertex>> GetVerticesByIndexAsync(string IndexName, object ID)
        {
            return GetVerticesAsync(new GremlinScript().Append_GetVerticesByIndex(IndexName, ID));
        }

        public List<GraphItems.Vertex> GetVerticesByIndex(string IndexName, IEnumerable<object> IDs)
        {
            return GetVertices(new GremlinScript().Append_GetVerticesByIndex(IndexName, IDs));
        }
        public Task<List<GraphItems.Vertex>> GetVerticeByIndexAsync(string IndexName, IEnumerable<object> IDs)
        {
            return GetVerticesAsync(new GremlinScript().Append_GetVerticesByIndex(IndexName, IDs));
        }

        public List<GraphItems.Vertex> GetVerticesByIndexAndLabel(string Label, string IndexName, object ID)
        {
            return GetVertices(new GremlinScript().Append_GetVerticesByIndexAndLabel(Label, IndexName, ID));
        }
        public Task<List<GraphItems.Vertex>> GetVerticesByIndexAndLabelAsync(string Label, string IndexName, object ID)
        {
            return GetVerticesAsync(new GremlinScript().Append_GetVerticesByIndexAndLabel(Label, IndexName, ID));
        }

        public List<GraphItems.Vertex> GetVerticesByIndexAndLabel(string Label, string IndexName, IEnumerable<object> IDs)
        {
            return GetVertices(new GremlinScript().Append_GetVerticesByIndexAndLabel(Label, IndexName, IDs));
        }
        public Task<List<GraphItems.Vertex>> GetVerticesByIndexAndLabelAsync(string Label, string IndexName, IEnumerable<object> IDs)
        {
            return GetVerticesAsync(new GremlinScript().Append_GetVerticesByIndexAndLabel(Label, IndexName, IDs));
        }
        #endregion

        #region GetVertexID
        public string GetVertexIDByIndex(string IndexName, object ID)
        {
            return GetString(new GremlinScript().Append_GetVertexIDByIndex(IndexName, ID));
        }
        public Task<string> GetVertexIDByIndexAsync(string IndexName, object ID)
        {
            return GetStringAsync(new GremlinScript().Append_GetVertexIDByIndex(IndexName, ID));
        }

        public string GetVertexIDByIndexAndLabel(string Label, string IndexName, object ID)
        {
            return GetString(new GremlinScript().Append_GetVertexIDByIndexAndLabel(Label, IndexName, ID));
        }
        public Task<string> GetVertexIDByIndexAndLabelAsync(string Label, string IndexName, object ID)
        {
            return GetStringAsync(new GremlinScript().Append_GetVertexIDByIndexAndLabel(Label, IndexName, ID));
        }
        #endregion

        #region CreateVertex
        public GraphItems.Vertex CreateVertex(Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            return GetVertex(new GremlinScript().Append_CreateVertex(Properties));
        }
        public Task<GraphItems.Vertex> CreateVertexAsync(Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            return GetVertexAsync(new GremlinScript().Append_CreateVertex(Properties));
        }

        public GraphItems.Vertex CreateVertexAndLabel(string Label, Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            return GetVertex(new GremlinScript().Append_CreateVertexAndLabel(Label, Properties));
        }
        public Task<GraphItems.Vertex> CreateVertexAndLabelAsync(string Label, Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            return GetVertexAsync(new GremlinScript().Append_CreateVertexAndLabel(Label, Properties));
        }
        #endregion

        #region DeleteVertex
        public void DeleteVertex(string ID)
        {
            Execute(new GremlinScript().Append_DeleteVertex(ID));
        }
        public Task DeleteVertexAsync(string ID)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteVertex(ID));
        }

        public void DeleteVertexByIndex(string IndexName, object ID)
        {
            Execute(new GremlinScript().Append_DeleteVertexByIndex(IndexName, ID));
        }
        public Task DeleteVertexByIndexAsync(string IndexName, object ID)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteVertexByIndex(IndexName, ID));
        }

        public void DeleteVertexByIndexAndLabel(string Label, string IndexName, object ID)
        {
            Execute(new GremlinScript().Append_DeleteVertexByIndexAndLabel(Label, IndexName, ID));
        }
        public Task DeleteVertexByIndexAndLabelAsync(string Label, string IndexName, object ID)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteVertexByIndexAndLabel(Label, IndexName, ID));
        }
        #endregion

        #region UpdateVertex
        public void UpdateVertex(string ID, Dictionary<string, List<GraphItems.VertexValue>> Properties, bool RemoveOtherProperties)
        {
            Execute(new GremlinScript().Append_UpdateVertex(ID, Properties, RemoveOtherProperties).Append("null;"));
        }
        public Task UpdateVertexAsync(string ID, Dictionary<string, List<GraphItems.VertexValue>> Properties, bool RemoveOtherProperties)
        {
            return ExecuteAsync(new GremlinScript().Append_UpdateVertex(ID, Properties, RemoveOtherProperties).Append("null;"));
        }
        #endregion

        #region EdgeExists
        public bool EdgeExistsBoth(string StartVertexID, string Name)
        {
            return GetBoolean(new GremlinScript().Append_EdgeExists_Both(StartVertexID, Name));
        }
        public Task<bool> EdgeExistsBothAsync(string StartVertexID, string Name)
        {
            return GetBooleanAsync(new GremlinScript().Append_EdgeExists_Both(StartVertexID, Name));
        }

        public bool EdgeExistsOut(string StartVertexID, string Name)
        {
            return GetBoolean(new GremlinScript().Append_EdgeExists_Out(StartVertexID, Name));
        }
        public Task<bool> EdgeExistsOutAsync(string StartVertexID, string Name)
        {
            return GetBooleanAsync(new GremlinScript().Append_EdgeExists_Out(StartVertexID, Name));
        }

        public bool EdgeExistsIn(string StartVertexID, string Name)
        {
            return GetBoolean(new GremlinScript().Append_EdgeExists_In(StartVertexID, Name));
        }
        public Task<bool> EdgeExistsInAsync(string StartVertexID, string Name)
        {
            return GetBooleanAsync(new GremlinScript().Append_EdgeExists_In(StartVertexID, Name));
        }
        #endregion

        #region CreateEdge
        public GraphItems.Edge CreateEdge(string StartVertexID, string EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            return GetEdge(new GremlinScript().Append_CreateEdge(StartVertexID, EndVertexID, Name, Properties));
        }
        public Task<GraphItems.Edge> CreateEdgeAsync(string StartVertexID, string EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            return GetEdgeAsync(new GremlinScript().Append_CreateEdge(StartVertexID, EndVertexID, Name, Properties));
        }
        #endregion

        #region DeleteEdge
        public void DeleteEdge(string ID)
        {
            Execute(new GremlinScript().Append_DeleteEdge(ID));
        }
        public Task DeleteEdgeAsync(string ID)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteEdge(ID));
        }

        public void DeleteEdgeBoth(string StartVertexID, string Name)
        {
            Execute(new GremlinScript().Append_DeleteEdge_Both(StartVertexID, Name));
        }
        public Task DeleteEdgeBothAsync(string StartVertexID, string Name)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteEdge_Both(StartVertexID, Name));
        }

        public void DeleteEdgeOut(string StartVertexID, string Name)
        {
            Execute(new GremlinScript().Append_DeleteEdge_Out(StartVertexID, Name));
        }
        public Task DeleteEdgeOutAsync(string StartVertexID, string Name)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteEdge_Out(StartVertexID, Name));
        }

        public void DeleteEdgeIn(string StartVertexID, string Name)
        {
            Execute(new GremlinScript().Append_DeleteEdge_In(StartVertexID, Name));
        }
        public Task DeleteEdgeInAsync(string StartVertexID, string Name)
        {
            return ExecuteAsync(new GremlinScript().Append_DeleteEdge_In(StartVertexID, Name));
        }
        #endregion

        #region GetEdge
        public GraphItems.Edge GetEdge(GremlinScript Script)
        {
            return Client.ExecuteScalar<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        public Task<GraphItems.Edge> GetEdgeAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }

        public GraphItems.Edge GetEdge(string ID)
        {
            return GetEdge(new GremlinScript().Append_GetEdge(ID));
        }
        public Task<GraphItems.Edge> GetEdgeAsync(string ID)
        {
            return GetEdgeAsync(new GremlinScript().Append_GetEdge(ID));
        }
        #endregion

        #region GetEdges
        public List<GraphItems.Edge> GetEdges(GremlinScript Script)
        {
            return Client.Execute<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        public Task<List<GraphItems.Edge>> GetEdgesAsync(GremlinScript Script)
        {
            return Client.ExecuteAsync<GraphItems.Edge>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        #region GetBoolean
        public bool GetBoolean(GremlinScript Script)
        {
            return Client.ExecuteScalar<bool>(Script.GetScript(), Script.GetBindings());
        }
        public Task<bool> GetBooleanAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<bool>(Script.GetScript(), Script.GetBindings());
        }
        #endregion

        #region GetString
        public string GetString(GremlinScript Script)
        {
            return Client.ExecuteScalar<string>(Script.GetScript(), Script.GetBindings());
        }
        public Task<string> GetStringAsync(GremlinScript Script)
        {
            return Client.ExecuteScalarAsync<string>(Script.GetScript(), Script.GetBindings());
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
            var ToReturn = new List<Dictionary<string, object>>();
            foreach (var Item in Array)
            {
                if (Item.Type == JTokenType.Array && Item.Count() <= 1)
                {
                    if (Item.First == null)
                        continue;
                    if (Item.First.Type == JTokenType.Array && Item.First.Count() <= 1)
                    {
                        if (Item.First.First == null)
                            continue;
                        ToReturn.Add(Item.First.First.ToObject<Dictionary<string, object>>());
                    }
                    else
                        ToReturn.Add(Item.First.ToObject<Dictionary<string, object>>());
                }
                else
                    ToReturn.Add(Item.ToObject<Dictionary<string, object>>());
            }
            return ToReturn;
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