using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teva.Common.Data.Gremlin.GraphItems
{
    public class VertexProperties : Dictionary<string, List<VertexValue>>
    {
        public void SetProperty<T>(string Key, T Value, bool IgnoreDefaultValue = true)
        {
            if (Value is Enum)
                throw new Exception("Please cast Enum to base type before SetProperty");

            if (Value == null || (IgnoreDefaultValue && EqualityComparer<T>.Default.Equals(Value, default(T))))
                base.Remove(Key);
            else
                base[Key] = new List<VertexValue> { new VertexValue(Value) };
        }
        public void SetProperty<T>(string Key, string Value, bool IgnoreDefaultValue = true)
        {
            if (Value == null || (IgnoreDefaultValue && Value.Length == 0))
                base.Remove(Key);
            else
                base[Key] = new List<VertexValue> { new VertexValue(Value) };
        }
        public void RemoveProperty(string Key)
        {
            base.Remove(Key);
        }
        public object GetProperty(string Key)
        {
            if (!base.ContainsKey(Key))
                return null;

            return base[Key][0].Contents;
        }
        public T GetProperty<T>(string Key)
        {
            if (!base.ContainsKey(Key))
                return default(T);

            return (T)base[Key][0].Contents;
        }
        public T GetProperty<T>(string Key, T DefaultValue)
        {
            if (!base.ContainsKey(Key))
                return DefaultValue;

            return (T)base[Key][0].Contents;
        }
        public bool HasProperty(string Key)
        {
            return base.ContainsKey(Key);
        }
    }
}
