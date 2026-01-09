using System;
using System.Collections.Generic;

namespace TSR.Game.Role
{
    public static class TeamBaseRegister
    {
        public static List<Type> AllTeamTypes = [];
        public static Dictionary<string, Type> TeamBaseTypeMap = new();

        /// <summary>
        /// TeamBase型を登録し、(string)TeamIdと型情報のマッピングも保存します。
        /// </summary>
        public static void RegisterTeam<T>() where T : TeamBase, new()
        {
            var team = new T();
            Logger.Info($"TryRegister: {typeof(T).Name}, TeamId={team.Team}");

            if (!AllTeamTypes.Contains(typeof(T)))
                AllTeamTypes.Add(typeof(T));
            if (!TeamBaseTypeMap.ContainsKey(team.Team))
                TeamBaseTypeMap[team.Team] = typeof(T);
            else
                Logger.Warning($"TeamId {team.Team} is already registered!");
        }

        /// <summary>
        /// TeamIdから新しいTeamBaseインスタンスを生成して返します。
        /// </summary>
        public static TeamBase GetTeamBase(string teamId)
        {
            if (TeamBaseTypeMap.TryGetValue(teamId, out var type))
            {
                return Activator.CreateInstance(type) as TeamBase;
            }
            Logger.Error(teamId + " is not included", "RegisterError");
            return null;
        }
    }
}