using System;

namespace RadFramework.Libraries.DictionaryMapper
{
    public interface IMapper
    {
        TypedMap ToDictionary(Type type, object @object);
        object ToObject(TypedMap values);

        object DeepClone(Type type, object @object);
    }
}