namespace NPerf.Fixture.ISerializer.Adapters
{
    using ServiceStack.Text;

    public class ServiceStackJsonISerializerAdapter<T> : ISerializer<T>
    {
        public string Serialize(T theObject)
        {
            return JsonSerializer.SerializeToString(theObject, typeof(T));
        }

        public T Deserialize(string serialized)
        {
            return JsonSerializer.DeserializeFromString<T>(serialized);
        }
    }
}
