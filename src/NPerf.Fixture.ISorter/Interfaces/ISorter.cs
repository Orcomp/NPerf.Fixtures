namespace NPerf.Fixture.ISorter.Interfaces
{
    using System.Collections.Generic;

    public interface ISorter<T>
    {
        void Sort(IList<T> list); 
    }
}