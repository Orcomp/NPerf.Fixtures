using Autofac;
namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;

    public class AutofacIIOCContainerAdapter : IIOCContainer.Interfaces.IIOCContainer
    {
        private ContainerBuilder builder { get; set; }
        private IContainer container { get; set; }

        public AutofacIIOCContainerAdapter()
        {
            this.builder = new ContainerBuilder();
        }

        public void RegisterType<TInt, TImp>()
        {
            this.builder.RegisterType<TInt>().As<TImp>();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            this.builder.RegisterType(implementationType).As(interfaceType);
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            this.builder.RegisterType<TInt>().As<TImp>().SingleInstance();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            this.builder.RegisterType(implementationType).As(interfaceType).SingleInstance();
        }

        public T Resolve<T>()
        {
            return this.container.Resolve<T>();
        }

        public object Resolve(Type interfaceType)
        {
            return this.container.Resolve(interfaceType);
        }

        public void FinishRegistering()
        {
            this.container = this.builder.Build();
            this.builder = new ContainerBuilder();
        }
    }
}