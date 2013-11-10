namespace NPerf.Fixture.IIOCContainer.Interfaces
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IIOCContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInt">interface type</typeparam>
        /// <typeparam name="TImp">implementation type</typeparam>
        void RegisterType<TInt, TImp>();
        void RegisterType(Type interfaceType, Type implementationType);

        void RegisterSingleton<TInt, TImp>();
        void RegisterSingleton(Type interfaceType, Type implementationType);

        T Resolve<T>();
        object Resolve(Type interfaceType);

        void FinishRegistering();
    }
}