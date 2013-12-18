namespace NPerf.Fixture.IIOCContainer.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TypeRandomizerHelper
    {
        private static Random random;

        static TypeRandomizerHelper()
        {
            random = new Random((int)DateTime.Now.Ticks);
        }

        private static List<Type> TypesInMSCorLib()
        {
            var mscorlib = typeof(string).Assembly;
            return new List<Type>(mscorlib.GetTypes());
        }

        public static List<Type> InterfaceTypesInMSCorLib()
        {
            var l = TypesInMSCorLib();
            return l.Where(t => t.IsInterface).ToList();
        }

        public static Type TypeImplementingInMSCorLib(Type type)
        {
            var l = TypesInMSCorLib();
            return l.First(t => !t.IsInterface && !t.IsAbstract && t.IsClass
                                && type.IsAssignableFrom(t));
        }

        public static List<Type> BaseTypesInMSCorLib()
        {
            var l = TypesInMSCorLib();
            return l.Where(t => !t.IsInterface && !t.IsAbstract && t.IsClass &&
                (t.GetConstructor(Type.EmptyTypes) != null) &&
                !t.IsGenericType && !t.IsGenericTypeDefinition &&
                (t.GetConstructors().Count() == 1)).ToList();
        }

        public static Type InterfaceForTypeInMSCorLib(Type type)
        {
            var l = TypesInMSCorLib();
            if (!l.Any(t => t.IsInterface && !t.IsGenericType && !t.IsGenericTypeDefinition
                             && t.IsAssignableFrom(type)))
            {
                return null;
            }

            return l.First(t => t.IsInterface
                                && t.IsAssignableFrom(type));
        }
    }
}