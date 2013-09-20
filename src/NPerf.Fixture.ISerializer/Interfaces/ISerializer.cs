namespace NPerf.Fixture.ISerializer
{
    public interface ISerializer<T>
    {
        string Serialize(T theObject);

        T Deserialize(string serialized);
    }
}
