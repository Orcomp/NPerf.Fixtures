namespace NPerf.Fixture.ISerializer.Adapters
{
    public class FastJsonISerializerAdapter<T> : ISerializer<T>
    {
        public string Serialize(T theObject)
        {
            return fastJSON.JSON.Instance.ToJSON(theObject);
        }

        public T Deserialize(string serialized)
        {
            return fastJSON.JSON.Instance.ToObject<T>(serialized);
        }
    }
}