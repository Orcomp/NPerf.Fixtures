namespace NPerf.Fixture.ISerializer.Adapters
{
    using ServiceStack.Text;

    public class ServiceStackJsonISerializerAdapter<T> : ISerializer<T>
    {
        public string Serialize(T theObject)
        {
            JsConfig.ConvertObjectTypesIntoStringDictionary = false;
            JsConfig.ExcludeTypeInfo = true;
            JsConfig.IncludeNullValues = true;
            return JsonSerializer.SerializeToString(theObject, typeof(T));
        }

        public T Deserialize(string serialized)
        {
            return JsonSerializer.DeserializeFromString<T>(serialized);
        }
    }
}
