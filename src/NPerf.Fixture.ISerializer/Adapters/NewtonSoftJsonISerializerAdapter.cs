namespace NPerf.Fixture.ISerializer.Adapters
{
    using System.IO;

    using Newtonsoft.Json;

    public class NewtonSoftJsonISerializerAdapter<T> : ISerializer<T>
    {
        public NewtonSoftJsonISerializerAdapter()
        {
            
        }

        public string Serialize(T theObject)
        {
            return JsonConvert.SerializeObject(theObject);
        }

        public T Deserialize(string serialized)
        {
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
