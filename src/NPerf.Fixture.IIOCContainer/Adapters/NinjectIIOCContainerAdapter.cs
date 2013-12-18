namespace NPerf.Fixture.IIOCContainer.Adapters
{
    using System;

    using Ninject;

    public class NinjectIIOCContainerAdapter : Interfaces.IIOCContainer
    {
        private StandardKernel Kernel { get; set; }

        public NinjectIIOCContainerAdapter()
        {
            this.Kernel = new StandardKernel();
        }

        public void RegisterType<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type interfaceType, Type implementationType)
        {
            this.Kernel.Bind(interfaceType).To(implementationType).Named(Guid.NewGuid().ToString());
        }

        public void RegisterSingleton<TInt, TImp>()
        {
            throw new NotImplementedException();
        }

        public void RegisterSingleton(Type interfaceType, Type implementationType)
        {
            this.Kernel.Bind(interfaceType).To(implementationType).InSingletonScope();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type interfaceType)
        {
            return this.Kernel.Get(interfaceType);
        }

        public void FinishRegistering()
        {

        }
    }
}