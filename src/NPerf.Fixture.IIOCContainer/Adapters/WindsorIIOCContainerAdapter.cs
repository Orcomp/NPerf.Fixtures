namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    public class WindsorIIOCContainerAdapter : Interfaces.IIOCContainer
    {
        private WindsorContainer Container { get; set; }

        public WindsorIIOCContainerAdapter()
        {
            this.Container = new WindsorContainer();
        }

        public void RegisterType<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            this.Container.Register(Component.For(interfaceType)
                .Named(Guid.NewGuid().ToString()).ImplementedBy(implementationType));
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            this.Container.Register(Component.For(interfaceType).ImplementedBy(implementationType).LifestyleSingleton());
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