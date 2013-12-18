namespace NPerf.Fixture.ISorter
{
    using System;
    using System.Collections.Generic;

    using Orc.Algorithms.Sort.Interfaces;
    using NPerf.Framework;

    [PerfTester(typeof(ISorter<int>), 10, Description = "Sorting Algorithm benchmark", FeatureDescription = "Collection size")]
    public class ISorterPerfs
    {

        private List<int> list;

        public int CollectionCount(int testIndex)
        {
            var n = 0;

            if (testIndex < 0)
            {
                n = 10;
            }
            else
            {
                n = (testIndex + 1) * 100;
            }

            return n;
        }

        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return (double)this.CollectionCount(testIndex);
        }

        [PerfSetUp]
        public void SetUp(int testIndex, ISorter<int> sorter)
        {
            var rnd = new Random();
            this.list = new List<int>();

            for (var i = 0; i < this.CollectionCount(testIndex); ++i)
            {
                this.list.Add(rnd.Next());
            }
        }

        [PerfTest]
        public void Sort(ISorter<int> sorter)
        {
            sorter.Sort(this.list);
        }

        [PerfTearDown]
        public void TearDown(ISorter<int> sorter)
        {
            // checking up
            for (var i = 0; i < this.list.Count - 1; ++i)
            {
                if (this.list[i] > this.list[i + 1])
                {
                    throw new Exception("The list is not sorted");
                }
            }
        }
    }
}