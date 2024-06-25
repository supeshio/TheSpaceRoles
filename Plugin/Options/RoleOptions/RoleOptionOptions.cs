//using HarmonyLib;
//using System.Linq;

//namespace TheSpaceRoles
//{
//    public static class RoleOptionOptions
//    {
//        public static Teams nowTeam = Teams.None;

//        public static Roles nowRole = Roles.None;
//        public static void Check(Teams teams, Roles roles)
//        {
//            nowRole = roles;
//            nowTeam = teams;

//            CustomOptionsHolder.RoleOptions.Where(x => x.role != roles || x.team != teams).Do(x => x.@object.active = false);
//            int i = 0;
//            foreach (var ro in CustomOptionsHolder.RoleOptions.Where(x => x.role == roles && x.team == teams))
//            {
//                ro.@object.SetActive(true);
//                ro.Check(i + 4);
//                i++;
//            }
//        }
//    }
//}
