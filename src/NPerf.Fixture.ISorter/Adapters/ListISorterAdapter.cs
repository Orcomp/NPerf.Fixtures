namespace NPerf.Fixture.ISorter.Adapters
{
    using System.Collections.Generic;

    using Orc.Algorithms.Sort.Interfaces;

    public class ListISorterAdapter<T> : ISorter<T>
    {
        public void Sort(IList<T> list)
        {
            if (list is List<T>)
            {
                (list as List<T>).Sort();
            }
        }
    }
}