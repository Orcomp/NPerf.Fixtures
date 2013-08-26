namespace NPerf.Fixture.IDictionary
{
    using System;
    using System.Collections.Generic;

    using NPerf.Framework;

    /// <summary>
    /// Performance test for implementations of the .NET Base Class Library IDictionary&lt;int&gt; interface.
    /// <see cref="http://msdn.microsoft.com/library/s4ys34ea.aspx"/>
    /// </summary>
    [PerfTester(
        typeof(IDictionary<int, int>),
        20,
        Description = "IDictionary operations benchmark",
        FeatureDescription = "Dictionary size")]
    public class IDictionaryPerfs
    {
        private readonly Random random = new Random();

        /// <summary>
        /// The number of key-value pairs of the tested IDictionary for the current test execution.
        /// </summary>
        private int count;

        /// <summary>
        /// Calculates the number of elements of the tested IDictionary from the test index number.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <returns>
        /// The number of elements of the collection to be tested..
        /// </returns>
        public int CollectionCount(int testIndex)
        {
            return testIndex * 10000;
        }

        /// <summary>
        /// The value that describes each execution of a test.
        /// In this case, the size of the IDictionary.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <returns>
        /// A double value that describes an execution of the test.
        /// </returns>
        [PerfRunDescriptor]
        public double RunDescription(int testIndex)
        {
            return this.CollectionCount(testIndex);
        }

        /// <summary>
        /// Set up the IDictionary with the appropriate number of elements for a test execution.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfSetUp]
        public void SetUp(int testIndex, IDictionary<int, int> dictionary)
        {
            this.count = this.CollectionCount(testIndex);

            for (var i = 0; i < this.count; i++)
            {
                dictionary.Add(i, i);
            }
        }

        /// <summary>
        /// Performance test for adding key-value pairs to an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void Add(IDictionary<int, int> dictionary)
        {
            dictionary.Add(this.count, this.count);
        }

        /// <summary>
        /// Performance test for checking if an IDictionary contains a key.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void ContainsKey(IDictionary<int, int> dictionary)
        {
            dictionary.ContainsKey(this.random.Next());
        }

        /// <summary>
        /// Performance test for getting the value associated with the first inserted key from an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void GetValueFirstAdded(IDictionary<int, int> dictionary)
        {
            int val;
            if (!dictionary.TryGetValue(0, out val))
            {
                throw new Exception("[PerfTest] GetValueFirstAdded: The element with the given key should be in the IDictionary");
            }
        }

        /// <summary>
        /// Performance test for getting the value associated with the last inserted key from an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void GetValueLastAdded(IDictionary<int, int> dictionary)
        {
            int val;
            if (!dictionary.TryGetValue(this.count - 1, out val))
            {
                throw new Exception("[PerfTest] GetValueLastAdded: The element with the given key should be in the IDictionary");
            }
        }

        /// <summary>
        /// Performance test for getting the value associated with a random inserted key from an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void GetValueRandomAdded(IDictionary<int, int> dictionary)
        {
            int val;
            if (!dictionary.TryGetValue(this.random.Next(0, this.count), out val))
            {
                throw new Exception("[PerfTest] GetValueRandomAdded: The element with the given key should be in the IDictionary");
            }
        }

        /// <summary>
        /// Performance test for trying to get the value associated with a key which was not added to an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void GetValueKeyNotExists(IDictionary<int, int> dictionary)
        {
            int val;
            if (dictionary.TryGetValue(this.count, out val))
            {
                throw new Exception("[PerfTest] GetValueKeyNotExists: The element with the given key should NOT be in the IDictionary");
            }
        }

        /// <summary>
        /// Performance test for setting the value associated with the first inserted key from an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void SetValueFirstAdded(IDictionary<int, int> dictionary)
        {
            try
            {
                dictionary[0] = this.random.Next();
            }
            catch (Exception e)
            {
                throw new Exception("[PerfTest] SetValueFirstAdded: The element with the given key should be in the IDictionary", e);
            }
        }

        /// <summary>
        /// Performance test for setting the value associated with the last inserted key from an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void SetValueLastAdded(IDictionary<int, int> dictionary)
        {
            try
            {
                dictionary[this.count - 1] = this.random.Next();
            }
            catch (Exception e)
            {
                throw new Exception("[PerfTest] SetValueLastAdded: The element with the given key should be in the IDictionary", e);
            }
        }

        /// <summary>
        /// Performance test for setting the value associated with a random inserted key from an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void SetValueRandomAdded(IDictionary<int, int> dictionary)
        {
            try
            {
                dictionary[this.random.Next(0, this.count)] = this.random.Next();
            }
            catch (Exception e)
            {
                throw new Exception("[PerfTest] SetValueRandomAdded: The element with the given key should be in the IDictionary", e);
            }
        }

        /// <summary>
        /// Performance test for emptying an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void Clear(IDictionary<int, int> dictionary)
        {
            dictionary.Clear();
        }

        /// <summary>
        /// Performance test for counting the elements of an IDictionary.
        /// </summary>
        /// <param name="dictionary">
        /// The dictionary to be tested.
        /// </param>
        [PerfTest]
        public void Count(IDictionary<int, int> dictionary)
        {
            var foo = dictionary.Count;
        }

        /// <summary>
        /// Performance test for removing the first added element to an IDictionary given its key.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void RemoveTheFirstAddedElement(IDictionary<int, int> dictionary)
        {
            dictionary.Remove(0);
        }

        /// <summary>
        /// Performance test for removing the last added element to an IDictionary given its key.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void RemoveTheLastAddedElement(IDictionary<int, int> dictionary)
        {
            dictionary.Remove(this.count - 1);
        }

        /// <summary>
        /// Performance test for removing a random element from an IDictionary given its key.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTest]
        public void RemoveRandomElement(IDictionary<int, int> dictionary)
        {
            dictionary.Remove(this.random.Next(0, this.count));
        }

        /// <summary>
        /// Clears the tested IDictionary after the execution of a test.
        /// </summary>
        /// <param name="dictionary">
        /// The IDictionary to be tested.
        /// </param>
        [PerfTearDown]
        public void TearDown(IDictionary<int, int> dictionary)
        {
            dictionary.Clear();
        }
    } // End of IDictionaryPerfs class
}
