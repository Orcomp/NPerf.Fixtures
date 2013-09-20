namespace NPerf.Fixture.ISerializer
{
    using System;
    using System.Collections.Generic;

    using NPerf.Framework;

    /// <summary>
    /// Performance test for Serializers.
    /// </summary>
    [PerfTester(
        typeof(ISerializer<object>),
        20,
        Description = "Serializers operations benchmark",
        FeatureDescription = "Object count")]
    public class ISerializerPerfs
    {
        private readonly Random random = new Random();

        /// <summary>
        /// The number of objects to serialize in the current test execution.
        /// </summary>
        private int count;

        private List<Object> objects;

        private List<String> serialized;

        /// <summary>
        /// Calculates the number of objects to serialize from the test index number.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <returns>
        /// The number of objects to serialize.
        /// </returns>
        public int CollectionCount(int testIndex)
        {
            return testIndex * 500;
        }

        /// <summary>
        /// The value that describes each execution of a test.
        /// In this case, the number of objects to serialize.
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
        /// Set up the objects list with the appropriate number of elements for a test execution.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <param name="serializer">
        /// The serializer to be tested.
        /// </param>
        [PerfSetUp]
        public void SetUp(int testIndex, ISerializer<Object> serializer)
        {
            this.count = this.CollectionCount(testIndex);
            objects = new List<Object>();
            serialized = new List<String>();

            for (var i = 0; i < this.count; i++)
            {
                objects.Add(i);
                serialized.Add(serializer.Serialize(i));
            }
        }

        /// <summary>
        /// Performance test for serializing a list of objects.
        /// </summary>
        /// <param name="serializer">
        /// The serializer to be tested.
        /// </param>
        [PerfTest]
        public void Serialize(ISerializer<Object> serializer)
        {
            for (var i = 0; i < this.count; i++)
            {
                serializer.Serialize(objects[i]);
            }
        }

        /// <summary>
        /// Performance test for deserializing a list of objects.
        /// </summary>
        /// <param name="serializer">
        /// The serializer to be tested.
        /// </param>
        [PerfTest]
        public void Deserialize(ISerializer<Object> serializer)
        {
            for (var i = 0; i < this.count; i++)
            {
                serializer.Deserialize(serialized[i]);
            }
        }

        /// <summary>
        /// Clears the tested serializer after the execution of a test.
        /// </summary>
        /// <param name="serializer">
        /// The serializer to be tested.
        /// </param>
        [PerfTearDown]
        public void TearDown(ISerializer<Object> serializer)
        {
            objects.Clear();
            serialized.Clear();
        }
    } // End of ISerializerPerfs class
}
