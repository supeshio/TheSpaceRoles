//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace TheSpaceRoles
//{
//    public static class RoleOptionTeamsHolder
//    {
//        public static List<RoleOptionTeams> TeamsHolder = new();
//        public static void Create()
//        {
//            int i = 0;
//            foreach (Teams team in Enum.GetValues(typeof(Teams)))
//            {
//                i++;
//                if (GetLink.CustomTeamLink.Any(x => x.Team == team))
//                {
//                    _ = new RoleOptionTeams(team, i);

//                }
//            }
//            _ = new RoleOptionTeams((Teams)(-1), i);
//        }
//    }
//}
