namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;

    using Microsoft.Practices.Unity;

    public class UnityIIOCContainerAdapter : Interfaces.IIOCContainer
    {
        public UnityContainer Container { get; set; }

        public UnityIIOCContainerAdapter()
        {
            this.Container = new UnityContainer();
        }

        public void RegisterType<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            this.Container.RegisterType(interfaceType, implementationType);
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            this.Container.RegisterType(interfaceType, implementationType, new ContainerControlledLifetimeManager());
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type interfaceType)
        {
            return this.Container.Resolve(interfaceType);
        }

        public void FinishRegistering()
        {

        }
    }
}