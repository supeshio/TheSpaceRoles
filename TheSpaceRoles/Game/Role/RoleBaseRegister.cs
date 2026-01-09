using System;
using System.Collections.Generic;

namespace TSR.Game.Role
{
    public static class RoleBaseRegister
    {
        public static List<Type> AllRoleTypes = [];
        public static Dictionary<string, Type> RoleBaseTypeMap = new();
        /// <summary>
        /// RoleBase型を登録し、RoleIdと型情報のマッピングも保存します。
        /// </summary>
        public static void RegisterRole<T>() where T : RoleBase, new()
        {
            var roleId = new T().Role();
            if (!AllRoleTypes.Contains(typeof(T)))
                AllRoleTypes.Add(typeof(T));
            if (!RoleBaseTypeMap.ContainsKey(roleId))
                RoleBaseTypeMap[roleId] = typeof(T);
            Logger.Info($"Register : RegistRole({roleId})");
        }

        /// <summary>
        /// RoleIdから新しいRoleBaseインスタンスを生成して返します。
        /// </summary>
        public static RoleBase GetRoleBase(string roleId)
        {
            if (RoleBaseTypeMap.TryGetValue(roleId, out var type))
            {
                return Activator.CreateInstance(type) as RoleBase;
            }
            Logger.Error(roleId + "is not included","RegisterError");
            return null;
        }
    }
}
