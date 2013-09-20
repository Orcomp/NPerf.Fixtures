namespace NPerf.Fixture.ISerializer.Adapters
{
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using System.Xml;

    public class DataContractJsonISerializerAdaper<T> : ISerializer<T>
    {
        private DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

        public string Serialize(T theObject)
        {
            var stream = XmlDictionaryWriter.Create(new StringBuilder());
            this.serializer.WriteObject(stream, theObject);
            return stream.ToString();
        }

        public T Deserialize(string serialized)
        {
            var reader = XmlDictionaryReader.Create(new StringReader(serialized));
            return (T)this.serializer.ReadObject(reader);
        }
    }
}
