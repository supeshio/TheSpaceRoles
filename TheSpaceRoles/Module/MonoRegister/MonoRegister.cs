using System;
using System.Linq;
using System.Reflection;

namespace TSR.Module.MonoRegister
{
    public static class MonoRegister
    {
        public static void Register()
        {
            var types = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass &&
                            t.GetCustomAttributes(typeof(MonoRegisterAttribute), inherit: true).Any())
                .ToArray();
            foreach (var type in types)
            {
                Il2CppInterop.Runtime.Injection.ClassInjector.RegisterTypeInIl2Cpp(type);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MonoRegisterAttribute : Attribute;
}