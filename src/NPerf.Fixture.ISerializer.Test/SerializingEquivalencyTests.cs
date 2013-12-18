using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NPerf.Fixture.ISerializer.Test
{
    using System.Collections.Generic;

    using NPerf.Fixture.ISerializer.Adapters;
    using NPerf.Fixture.ISerializer.Helpers;

    [TestClass]
    public class SerializingEquivalencyTests
    {
        private List<RandomObjectClass> objects;
        private List<string> serialized;

        private const int NUMBER_OF_OBJECTS = 20;
            
        [TestInitialize]
        public void SetUp()
        {
            var jsSerializer = new JavascriptISerializerAdapter<RandomObjectClass>();

            objects = new List<RandomObjectClass>();
            serialized = new List<string>();

            for (var i = 0; i < NUMBER_OF_OBJECTS; i++)
            {
                var obj = (RandomObjectClass)ObjectRandomizerHelper.RandomSimpleObject();
                objects.Add(obj);
                serialized.Add(jsSerializer.Serialize(obj));
            }
        }

        [TestMethod]
        public void FastJsonToJavascriptSerializerEquivalence()
        {
            var serializer = new FastJsonISerializerAdapter<RandomObjectClass>();

            this.ISerializerCompare(serializer);
        }

        [TestMethod]
        public void NewtonSoftJsonToJavascriptSerializerEquivalence()
        {
            var serializer = new NewtonSoftJsonISerializerAdapter<RandomObjectClass>();

            this.ISerializerCompare(serializer);
        }

        [TestMethod]
        public void ServiceStackToJavascriptSerializerEquivalence()
        {
            var serializer = new ServiceStackJsonISerializerAdapter<RandomObjectClass>();

            this.ISerializerCompare(serializer);
        }

        [TestMethod]
        public void SimpleJsonToJavascriptSerializerEquivalence()
        {
            var serializer = new SimpleJsonISerializerAdapter<RandomObjectClass>();

            this.ISerializerCompare(serializer);
        }

        private void ISerializerCompare(ISerializer<RandomObjectClass> serializer)
        {
            for (var i = 0; i < objects.Count; i++)
            {
                Assert.AreEqual(serialized[i], serializer.Serialize(objects[i]), true);
            }
        }

        private void IDeserializerCompare(ISerializer<RandomObjectClass> serializer)
        {
            for (var i = 0; i < objects.Count; i++)
            {
                Assert.AreEqual(serializer.Deserialize(serialized[i]), objects[i]);
            }
        }

        [TestMethod]
        public void FastJsonToJavascriptDeserializerEquivalence()
        {
            var serializer = new FastJsonISerializerAdapter<RandomObjectClass>();

            this.IDeserializerCompare(serializer);
        }

        [TestMethod]
        public void NewtonSoftJsonToJavascriptDeserializerEquivalence()
        {
            var serializer = new NewtonSoftJsonISerializerAdapter<RandomObjectClass>();

            this.IDeserializerCompare(serializer);
        }

        [TestMethod]
        public void ServiceStackToJavascriptDeserializerEquivalence()
        {
            var serializer = new ServiceStackJsonISerializerAdapter<RandomObjectClass>();

            this.IDeserializerCompare(serializer);
        }

        [TestMethod]
        public void SimpleJsonToJavascriptDeserializerEquivalence()
        {
            var serializer = new SimpleJsonISerializerAdapter<RandomObjectClass>();

            this.IDeserializerCompare(serializer);
        }
    }
}
