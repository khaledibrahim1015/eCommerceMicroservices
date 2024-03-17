using System;
using System.Reflection;

namespace Discount.Api.Helpers
{
    public static class ReflectionHelper
    {
        public static string GetClassName<TClass>() 
            =>  typeof(TClass).Name;
        public static string GetClassFullName<TClass>() 
            => typeof(TClass).FullName;

        public static object CreateInstance<TClass>()
        {

            Assembly assembly = typeof(TClass).Assembly;
            Type type = assembly.GetType(GetClassName<TClass>());
            if(type == null)
                 throw new ArgumentException($"Type '{type}' not found in the current assembly.");
            return Activator.CreateInstance(type);

        }

    }
}
