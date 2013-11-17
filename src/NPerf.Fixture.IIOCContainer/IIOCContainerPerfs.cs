namespace NPerf.Fixture.IIOCContainer
{
    using System;
    using System.Collections.Generic;

    using NPerf.Fixture.IIOCContainer.Helper;
    using NPerf.Fixture.IIOCContainer.Interfaces;
    using NPerf.Framework;

    [PerfTester(
        typeof(IIOCContainer),
        20,
        Description = "IOC containers operations benchmark",
        FeatureDescription = "Number of resolves")]
    public class IIOCContainerPerfs
    {
        private readonly Random random = new Random();

        /// <summary>
        /// The number of interface resolutions in the current test execution.
        /// </summary>
        private int count;
        private List<Type> interfaceTypes;
        private int numberOfTypes;


        /// <summary>
        /// Calculates the number of resolves to do from the test index number.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <returns>
        /// The number of resolves to do.
        /// </returns>
        public int CollectionCount(int testIndex)
        {
            return testIndex * 10;
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
        /// Set up the IOC container for a test execution.
        /// </summary>
        /// <param name="testIndex">
        /// The test index number.
        /// </param>
        /// <param name="container">
        /// The container to be tested.
        /// </param>
        [PerfSetUp]
        public void SetUp(int testIndex, IIOCContainer container)
        {
            this.count = this.CollectionCount(testIndex);

            this.interfaceTypes = new List<Type>();
            var i = 0;
            var bt = TypeRandomizerHelper.BaseTypesInMSCorLib();

            foreach (var iimp in bt)
            {
                var iint = TypeRandomizerHelper.InterfaceForTypeInMSCorLib(iimp);
                if ((iint == null) || this.interfaceTypes.Contains(iint))
                {
                    continue;
                }

                this.interfaceTypes.Add(iint);
                container.RegisterType(iint, iimp);
                
                i++;
            }
            this.numberOfTypes = i;

            container.FinishRegistering();
        }

        /// <summary>
        /// Performance test for resolving interfaces given an IOC container with 10 registered types.
        /// </summary>
        /// <param name="container">
        /// The IOC container to be tested.
        /// </param>
        [PerfTest]
        public void Resolve(IIOCContainer container)
        {
            for (var i = 0; i < this.count; i++)
            {
                container.Resolve(this.interfaceTypes[this.random.Next(this.numberOfTypes)]);
            }
        }

    }
}