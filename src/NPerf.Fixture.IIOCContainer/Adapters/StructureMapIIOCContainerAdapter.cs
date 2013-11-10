namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;

    using Fixture.IIOCContainer.Interfaces;

    public class StructureMapIIOCContainerAdapter : IIOCContainer
    {
        public void RegisterType<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type interfaceType)
        {
            throw new NotImplementedException();
        }

        public void FinishRegistering()
        {
            throw new NotImplementedException();
        }
    }
}