using System;
using System.Linq;
using System.Reflection;

namespace TSR.Game.Role
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RoleBaseAttribute : Attribute;

    public static class RoleBaseLoad
    {
        public static void Load()
        {
            var typesWithAttribute =
            Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass &&
                            t.GetCustomAttribute<RoleBaseAttribute>() != null)
                .ToList();
        }
    }
}