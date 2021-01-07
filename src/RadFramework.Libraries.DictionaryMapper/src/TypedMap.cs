using System;
using System.Collections.Generic;

namespace RadFramework.Libraries.DictionaryMapper
{
    public class TypedMap
    {
        public Type Type { get; set; }
        public IDictionary<string, object> Values { get; set; }
    }
}