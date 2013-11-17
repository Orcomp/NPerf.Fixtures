namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;
    using Dynamo.Ioc;

    public class DynamoIIOCContainerAdapter : Interfaces.IIOCContainer
    {
        private IocContainer Container { get; set; }

        public DynamoIIOCContainerAdapter()
        {
            this.Container = new IocContainer();
        }

        public void RegisterType<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            this.Container.Register(interfaceType, implementationType, Guid.NewGuid());
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            this.Container.Register(interfaceType, implementationType, null, new ContainerLifetime());
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