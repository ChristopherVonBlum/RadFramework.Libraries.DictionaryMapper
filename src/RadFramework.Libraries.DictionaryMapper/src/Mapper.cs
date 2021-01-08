using System;
using System.Collections.Generic;
using RadFramework.Libraries.Reflection.Caching;
using RadFramework.Libraries.Reflection.Caching.Queries;
using Activator = RadFramework.Libraries.Reflection.Activation.Activator;

namespace RadFramework.Libraries.DictionaryMapper
{
    public class Mapper : IMapper
    {
        public TypedMap ToDictionary(Type type, object @object)
        {
            Dictionary<string, object> values = new Dictionary<string, object>();

            CachedType t = type;

            foreach (var property in t.Query(TypeQueries.GetProperties))
            {
                var propertyValue = property.GetValue(@object);
                
                if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                {
                    values[property.Name] = propertyValue;
                    continue;
                }

                if (propertyValue is ICloneable c)
                {
                    values[property.Name] = c.Clone();
                    continue;
                }
                
                values[property.Name] = ToDictionary(property.PropertyType, propertyValue);
            }

            return new TypedMap
            {
                Type = type,
                Values = values
            };
        }

        public object ToObject(TypedMap map)
        {
            CachedType t = map.Type;

            var obj = Activator.Activate(map.Type);

            foreach (var property in t.Query(TypeQueries.GetProperties))
            {
                var propertyValue = map.Values[property.Name];

                if (propertyValue is ICloneable c)
                {
                    propertyValue = c.Clone();
                }
                else if (propertyValue is TypedMap childMap)
                {
                    propertyValue = ToObject(childMap);
                }
                
                property.SetValue(obj, propertyValue);
            }

            return obj;
        }

        public object DeepClone(Type type, object @object)
        {
            return ToObject(ToDictionary(type, @object));
        }
    }
}