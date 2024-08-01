using System;
using System.Collections.Generic;

namespace TheSpaceRoles
{
    public static class RoleOptionTeamsHolder
    {
        public static List<RoleOptionTeams> TeamsHolder = new();
        public static void Create()
        {
            int i = 0;
            foreach (Teams team in Enum.GetValues(typeof(Teams)))
            {
                TeamsHolder.Add(new RoleOptionTeams(team, i));
                i++;
            }
        }
    }
}
