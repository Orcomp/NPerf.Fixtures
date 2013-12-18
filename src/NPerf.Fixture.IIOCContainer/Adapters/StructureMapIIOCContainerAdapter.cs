namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;

    using Fixture.IIOCContainer.Interfaces;

    using StructureMap;

    public class StructureMapIIOCContainerAdapter : IIOCContainer
    {
        private Container Container { get; set; }

        public StructureMapIIOCContainerAdapter()
        {
            Container = new Container();
        }

        public void RegisterType<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            Container.Configure(x => x.For(interfaceType).Use(implementationType));
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            Container.Configure(x => x.For(interfaceType).Singleton().Use(implementationType));
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type interfaceType)
        {
            return Container.GetInstance(interfaceType);
        }

        public void FinishRegistering()
        {

        }
    }
}