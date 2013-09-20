namespace NPerf.Fixture.ISerializer.Adapters
{
    using Json;

    public class JsonISerializerAdapter<T> : ISerializer<T>
    {
        public string Serialize(T theObject)
        {
            return JsonParser.Serialize(theObject);
        }

        public T Deserialize(string serialized)
        {
            return JsonParser.Deserialize<T>(serialized);
        }
    }
}