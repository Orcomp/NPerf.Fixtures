namespace NPerf.Fixture.ISerializer.Adapters
{
    public class SimpleJsonISerializerAdapter<T> : ISerializer<T>
    {
        public string Serialize(T theObject)
        {
            return SimpleJson.SerializeObject(theObject);
        }

        public T Deserialize(string serialized)
        {
            return SimpleJson.DeserializeObject<T>(serialized);
        }
    }
}