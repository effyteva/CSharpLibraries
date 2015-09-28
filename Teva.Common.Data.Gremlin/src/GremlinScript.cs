using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin
{
    public class GremlinScript
    {
        public GremlinScript()
        {
        }
        public GremlinScript(string Script)
        {
            Append(Script);
        }
        public GremlinScript(params object[] Parts)
        {
            Append(Parts);
        }

        public object GetMetadata(string Key)
        {
            if (Metadata == null || !Metadata.ContainsKey(Key))
                return null;
            return Metadata[Key];
        }
        public void SetMetadata(string Key, object Value)
        {
            if (Metadata == null)
                Metadata = new Dictionary<string, object>();
            Metadata[Key] = Value;
        }
        private Dictionary<string, object> Metadata { get; set; }

        public GremlinScript Append(string Script)
        {
            //if (SB.Length == 0 && !string.IsNullOrWhiteSpace(Script) && Script.StartsWith("."))
            //    SB.Append("it");

            SB.Append(Script);
            return this;
        }
        public GremlinScript Append(params object[] Parts)
        {
            return Insert(SB.Length, Parts);
        }
        public GremlinScript Insert(int Index, params object[] Parts)
        {
            var ToAppend = new StringBuilder();
            foreach (var Part in Parts)
                ToAppend.Append((string)Part);
            SB.Insert(Index, ToAppend.ToString());
            return this;
        }

        public void CreateInternalSB()
        {
            if (StoredSBs == null)
                StoredSBs = new List<StringBuilder>();
            StoredSBs.Add(SB);
            SB = new StringBuilder();
        }
        public string CloseInternalSB()
        {
            if (StoredSBs == null || StoredSBs.Count == 0)
                throw new Exception("There's no stored SBs");
            var ToReturn = SB.ToString();
            SB = StoredSBs[StoredSBs.Count - 1];
            StoredSBs.RemoveAt(StoredSBs.Count - 1);
            return ToReturn;
        }
        private List<StringBuilder> StoredSBs;
        private StringBuilder SB = new StringBuilder();

        public string GetScript()
        {
            return SB.ToString();
        }
        public string GetReadableScript()
        {
            string ToReturn = SB.ToString();
            for (int i = Parameters.Count - 1; i > -1; i--)
            //for (int i = 0; i < Parameters.Count; i++)
            {
                string NewValue;
                if (Parameters[i] is string)
                {
                    if (Parameters[i] == null)
                        NewValue = "''";
                    else
                        NewValue = "'" + ((string)Parameters[i]).Replace("'", "\\'") + "'";
                }
                else if (Parameters[i] is long)
                    NewValue = Parameters[i].ToString() + "L";
                else if (Parameters[i] is decimal || Parameters[i] is int || Parameters[i] is short)
                    NewValue = Parameters[i].ToString();
                else if (Parameters[i] is bool)
                    NewValue = Parameters[i].ToString().ToLower();
                else if (Parameters[i] is DateTime)
                    NewValue = (((DateTime)Parameters[i]).Ticks).ToString();
                else if (Parameters[i] == null)
                    NewValue = "null";
                else
                    NewValue = "?" + Parameters[i].ToString();
                ToReturn = ToReturn.Replace("p" + i, NewValue);
            }
            return ToReturn;
        }
        public Dictionary<string, object> GetBindings()
        {
            var ToReturn = new Dictionary<string, object>();
            int Count = 0;
            foreach (var Parameter in Parameters)
                ToReturn.Add(string.Format("p{0}", Count++), Newtonsoft.Json.Linq.JToken.FromObject(Parameter));
            return ToReturn;
        }
        public bool IsEmpty()
        {
            return SB.Length == 0;
        }
        public override string ToString()
        {
            return SB.ToString();
        }

        public string GetParameterName(object Value)
        {
            int CurrentIndex = Parameters.FindIndex(T => T == Value);
            if (CurrentIndex != -1)
                return "p" + CurrentIndex;
            else
            {
                string ParameterName = "p" + Parameters.Count;
                Parameters.Add(Value);
                return ParameterName;
            }
        }
        private List<object> Parameters = new List<object>();

        public string GetNextVariableName()
        {
            VariableNameIndex++;
            return "x_" + VariableNameIndex;
        }
        private int VariableNameIndex = -1;

        public string Script_GetID()
        {
            if (IdentifierIndexName != null)
                return ".values('" + IdentifierIndexName + "')[0]";
            else
                return ".id()";
        }

        public GremlinScript Append_Values<T>(IEnumerable<T> Values, string Seperator = ",")
        {
            bool First = true;
            foreach (var Value in Values)
            {
                if (First)
                    First = false;
                else
                    Append(Seperator);
                Append_Parameter(Value);
            }
            return this;
        }
        public GremlinScript Append_Values<T>(IEnumerable<long> Values, string Seperator = ",")
        {
            bool First = true;
            foreach (var Value in Values)
            {
                if (First)
                    First = false;
                else
                    Append(Seperator);
                Append("(long)");
                Append_Parameter(Value);
            }
            return this;
        }
        public GremlinScript Append_PropertiesArrayString(Dictionary<string, object> Properties, bool AddCommaOnFirstItem = false)
        {
            if (Properties == null)
                return this;

            bool First = true;
            foreach (var Property in Properties)
            {
                if (Property.Value == null || Property.Value == null)
                    continue;
                if (First)
                {
                    First = false;
                    if (AddCommaOnFirstItem)
                        Append(",");
                }
                else
                    Append(",");
                Append_Parameter(Property.Key).Append(",").Append_Parameter(Property.Value);
            }
            return this;
        }
        public GremlinScript Append_PropertiesArrayString(Dictionary<string, List<GraphItems.VertexValue>> Properties, bool AddCommaOnFirstItem = false)
        {
            if (Properties == null)
                return this;

            bool First = true;
            foreach (var Property in Properties)
            {
                if (Property.Value == null || Property.Value.Count == 0 || Property.Value[0].Contents == null)
                    continue;
                if (Property.Value.Count != 1)
                    throw new Exception("Invalid Property.Value count: " + Property.Key);
                if (First)
                {
                    First = false;
                    if (AddCommaOnFirstItem)
                        Append(",");
                }
                else
                    Append(",");
                Append_Parameter(Property.Key).Append(",").Append_Parameter(Property.Value[0].Contents);
            }
            return this;
        }

        public GremlinScript Append_GetVertex(long ID)
        {
            return Append_GetVertices(ID);
        }
        public GremlinScript Append_GetVertices(long ID)
        {
            if (IdentifierIndexName != null)
                return Append_GetVerticesByIndex(IdentifierIndexName, ID);
            else
                return Append_GetVerticesByID(ID);
        }
        public GremlinScript Append_GetVertices(params long[] IDs)
        {
            if (IdentifierIndexName != null)
                return Append_GetVerticesByIndex(IdentifierIndexName, IDs);
            else
                return Append_GetVerticesByID(IDs);
        }
        public GremlinScript Append_GetVertexByID(long ID)
        {
            return Append_GetVerticesByID(ID);
        }
        public GremlinScript Append_GetVerticesByID(params long[] IDs)
        {
            if (IDs.Length == 0)
                throw new Exception("Missing IDs");
            else if (IDs.Length == 1)
                return Append("g.V((long)" + GetParameterName(IDs[0]) + ")");
            else
                return Append("g.V(").Append_Values(IDs).Append(")");
        }
        public GremlinScript Append_GetVertexByIndex(string IndexName, long ID)
        {
            return Append_GetVertexByIndex(IndexName, ID);
        }
        public GremlinScript Append_GetVerticesByIndex(string IndexName, params long[] IDs)
        {
            if (IDs.Length == 0)
                throw new Exception("Missing IDs");
            else if (IDs.Length == 1)
                return Append("g.V().has(").Append_Parameter(IndexName).Append(",(long)").Append_Parameter(IDs[0]).Append(")");
            else
                return Append("g.V().has(").Append_Parameter(IndexName).Append(",within(").Append_Values(IDs).Append("))");
        }
        public GremlinScript Append_GetEdge(long ID)
        {
            return Append("g.E((long)" + GetParameterName(ID) + ")");
        }
        public GremlinScript Append_CreateEdge(long StartVertexID, long EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            return Append_GetVertex(StartVertexID).Append_Next().Append(".addEdge(").Append_Parameter(Name).Append(",").Append_GetVertex(EndVertexID).Append_Next().Append_PropertiesArrayString(Properties, true).Append(");");
        }
        public GremlinScript Append_CreateEdge(string StartVertexPropertyName, long EndVertexID, string Name, Dictionary<string, object> Properties = null)
        {
            return Append(StartVertexPropertyName + ".addEdge(").Append_Parameter(Name).Append(",").Append_GetVertex(EndVertexID).Append_Next().Append_PropertiesArrayString(Properties, true).Append(");");
        }
        public GremlinScript Append_CreateEdge(long StartVertexID, string EndVertexPropertyName, string Name, Dictionary<string, object> Properties = null)
        {
            return Append_GetVertex(StartVertexID).Append_Next().Append(".addEdge(").Append_Parameter(Name).Append("," + EndVertexPropertyName).Append_PropertiesArrayString(Properties, true).Append(");");
        }
        public GremlinScript Append_DeleteEdge(long ID)
        {
            //return Append_GetEdge(ID).Append_Next().Append(".remove();");
            return Append_GetEdge(ID).Append(".sideEffect{it.get().remove()}.iterate();");
        }
        public GremlinScript Append_CreateVertex(Dictionary<string, List<GraphItems.VertexValue>> Properties)
        {
            //return Append("graph.addVertex(").Append_PropertiesArrayString(Properties).Append(")");
            return Append("g.addV(").Append_PropertiesArrayString(Properties).Append(").next()");
        }
        public GremlinScript Append_UpdateVertex(long ID, Dictionary<string, List<GraphItems.VertexValue>> Properties, bool RemoveOtherProperties)
        {
            string VariableName = GetNextVariableName();
            Append("def " + VariableName + "=").Append_GetVertex(ID).Append_Next().Append(";");
            if (RemoveOtherProperties)
            {
                if (IdentifierIndexName != null)
                    Append(VariableName + ".properties().each{if(it.key!='" + IdentifierIndexName + "')it.remove()};");
                else
                    Append(VariableName + ".properties().each{it.remove()};");
                //Append_CommitGraph();
                //Append(VariableName);
                foreach (var Property in Properties)
                {
                    if ((IdentifierIndexName != null && Property.Key == IdentifierIndexName) || Property.Value == null || Property.Value.Count == 0 || Property.Value[0].Contents == null)
                        continue;
                    Append(VariableName).Append_SetProperty(Property.Key, Property.Value[0].Contents);
                }
            }
            else
            {
                foreach (var Property in Properties)
                    Append(VariableName).Append_SetProperty(Property.Key, Property.Value == null || Property.Value.Count == 0 ? null : Property.Value[0].Contents);
            }
            return this;
        }
        public GremlinScript Append_ID()
        {
            if (IdentifierIndexName != null)
                return Append(".values('" + IdentifierIndexName + "')[0]");
            else
                return Append(".id()");
        }
        public GremlinScript Append_IDs()
        {
            if (IdentifierIndexName != null)
                return Append(".values('" + IdentifierIndexName + "')");
            else
                return Append(".id()");
        }
        public GremlinScript Append_In()
        {
            return Append(".in()");
        }
        public GremlinScript Append_In(string Name)
        {
            return Append(".in('" + Name + "')");
        }
        public GremlinScript Append_InE(string Name)
        {
            return Append(".inE('" + Name + "')");
        }
        public GremlinScript Append_Out()
        {
            return Append(".out()");
        }
        public GremlinScript Append_Out(string Name)
        {
            return Append(".out('" + Name + "')");
        }
        public GremlinScript Append_OutE(string Name)
        {
            return Append(".outE('" + Name + "')");
        }
        public GremlinScript Append_InV()
        {
            return Append(".inV()");
        }
        public GremlinScript Append_OutV()
        {
            return Append(".outV()");
        }
        public GremlinScript Append_ValueMap()
        {
            return Append(".valueMap()");
        }
        public GremlinScript Append_StartTransform()
        {
            return Append(".map");
        }
        public GremlinScript Append_StartDictionaryTransform()
        {
            return Append(".map{[");
        }
        public GremlinScript Append_EndDictionaryTransform()
        {
            return Append("]}");
        }
        public GremlinScript Append_Next()
        {
            return Append(".next()");
        }
        // Untested
        public GremlinScript Append_HasNext()
        {
            return Append(".hasNext()");
        }
        // Untested
        public GremlinScript Append_As(string Name)
        {
            return Append(".as('" + Name + "')");
        }
        // Untested
        public GremlinScript Append_Optional(int Count)
        {
            return Append(".optional(" + Count + ")");
        }
        // Untested
        public GremlinScript Append_Optional(string Name)
        {
            return Append(".optional('" + Name + "')");
        }
        // Untested
        public GremlinScript Append_Back(int Count)
        {
            return Append(".back(" + Count + ")");
        }
        public GremlinScript Append_Back(string Name)
        {
            return Append(".select('" + Name + "')");
        }
        public GremlinScript Append_GetProperty(string Name)
        {
            return Append(".values('" + Name + "')[0]");
        }
        public GremlinScript Append_GetProperties(string Name)
        {
            return Append(".values('" + Name + "')");
        }
        public GremlinScript Append_SetProperty(string Name, object Value)
        {
            if (Value == null)
                return Append(".property(" + GetParameterName(Name) + ").remove();");
            else
                return Append(".property(" + GetParameterName(Name) + "," + GetParameterName(Value) + ");");
        }
        public GremlinScript Append_RemoveProperty(string Name)
        {
            return Append(".property(" + GetParameterName(Name) + ").remove();");
        }
        public GremlinScript Append_FilterEquals(string Name, params object[] Values)
        {
            if (Values.Length == 0)
                throw new Exception("Missing Values");
            else if (Values.Length == 1)
                return Append(".has(").Append_Parameter(Name).Append(",").Append_Parameter(Values[0]).Append(")");
            else
                return Append(".has(").Append_Parameter(Name).Append(",within(").Append_Values(Values).Append("))");
        }
        public GremlinScript Append_FilterGreaterThanEquals(string Name, object Value)
        {
            return Append(".has(").Append_Parameter(Name).Append(",gte(").Append_Parameter(Value).Append("))");
        }
        // Untested
        public GremlinScript Append_FilterIDEquals(long Value)
        {
            if (IdentifierIndexName != null)
                return Append_FilterEquals(IdentifierIndexName, Value);
            else
                return Append(".filter{it.id()==(long)" + GetParameterName(Value) + "}");
        }
        // Untested
        public GremlinScript Append_FilterIDEquals(List<long> Values)
        {
            if (IdentifierIndexName != null)
                return Append_FilterEquals(IdentifierIndexName, Values.Cast<object>().ToArray());
            else
            {
                Append(".filter{it.id()==(long)" + GetParameterName(Values[0]));
                for (int i = 1; i < Values.Count; i++)
                    Append(" || it.id()==(long)" + GetParameterName(Values[i]));
                Append("}");
                return this;
            }
        }
        public GremlinScript Append_Count()
        {
            return Append(".count()");
        }
        public GremlinScript Append_Max()
        {
            return Append(".max()");
        }
        public GremlinScript Append_Min()
        {
            return Append(".min()");
        }
        public GremlinScript Append_Sum()
        {
            return Append(".sum()");
        }
        public GremlinScript Append_Scatter()
        {
            return Append(".unfold()");
        }
        public GremlinScript Append_Gather()
        {
            return Append(".fold()");
        }
        // Untested
        public GremlinScript Append_SortProperty(string Name, object DefaultValue = null, bool Descending = false)
        {
#warning Make sure DefaultValue is Long and not Int32
            if (DefaultValue == null)
                Append(".order().by(").Append_Parameter(Name).Append(", " + (Descending ? "decr" : "incr") + ")");
            else
                Append(".order().by(coalesce(values(").Append_Parameter(Name).Append("),constant(").Append_Parameter(DefaultValue).Append(")), " + (Descending ? "decr" : "incr") + ")");
            return this;
        }
        public GremlinScript Append_Reverse()
        {
            return Append(".reverse()");
        }
        // Untested
        public GremlinScript Append_ToPipe()
        {
            return Append("._()");
        }
        public GremlinScript Append_ItToPipe()
        {
            return Append("g.V(it.get())");
            //return Append("it._()");
        }
        public GremlinScript Append_ItEdgeToPipe()
        {
            return Append("g.E(it.get())");
            //return Append("it._()");
        }
        public GremlinScript Append_ItGet()
        {
            return Append("it.get()");
        }
        // Untested
        public GremlinScript Append_CommitGraph()
        {
            return Append("g.commit();");
        }
        public GremlinScript Append_Range(long From, long To)
        {
            if (From == 0)
                return Append(".limit(" + To + ")");
            else
                return Append(".range(" + From + "," + To + ")");
        }
        public GremlinScript Append_RemovePipeVertices()
        {
            return Append(".sideEffect{it.get().remove()}.iterate();");
        }
        public GremlinScript Append_FirstResult()
        {
            return Append("[0]");
        }

        public GremlinScript Append_Parameter(object Value)
        {
            return Append(GetParameterName(Value));
        }

        private string IdentifierIndexName = "explicitid";
    }
}