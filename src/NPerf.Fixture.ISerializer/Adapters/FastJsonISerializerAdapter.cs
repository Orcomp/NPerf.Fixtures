namespace NPerf.Fixture.ISerializer.Adapters
{
    public class FastJsonISerializerAdapter<T> : ISerializer<T>
    {
        public string Serialize(T theObject)
        {
            fastJSON.JSON.Instance.Parameters.UsingGlobalTypes = false;
            fastJSON.JSON.Instance.Parameters.UseExtensions = false;
            return fastJSON.JSON.Instance.ToJSON(theObject);
        }

        public T Deserialize(string serialized)
        {
            return fastJSON.JSON.Instance.ToObject<T>(serialized);
        }
    }
}