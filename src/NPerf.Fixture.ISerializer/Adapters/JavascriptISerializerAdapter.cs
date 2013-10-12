namespace NPerf.Fixture.ISerializer.Adapters
{
    using System.Web.Script.Serialization;

    public class JavascriptISerializerAdapter<T> : ISerializer<T>
    {
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        public string Serialize(T theObject)
        {
            return serializer.Serialize(theObject);
        }

        public T Deserialize(string serialized)
        {
            return serializer.Deserialize<T>(serialized);
        }
    }
}