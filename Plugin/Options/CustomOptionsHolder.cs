using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public static class CustomOptionsHolder
    {

        public static void CreateCustomOptions()
        {
            





            if (CustomOption.options.Count != 0) return;
            HeaderCreate(OptionType.General, "admin");
            //Create(OptionType.Default, "use_records_admin", true);
            Create(OptionType.General, "limit_admin", new CustomFloatRange(0, 180, 2.5f), 0);

            HeaderCreate(OptionType.General, "vital");
            Create(OptionType.General, "limit_vital", new CustomFloatRange(0, 180, 2.5f), 0);

            HeaderCreate(OptionType.General, "camera");
            Create(OptionType.General, "limit_camera", new CustomFloatRange(0, 180, 2.5f), 0);

            HeaderCreate(OptionType.General, "doorlog");
            Create(OptionType.General, "limit_doorlog", new CustomFloatRange(0, 180, 2.5f), 0);

            HeaderCreate(OptionType.General, "binoculars");
            Create(OptionType.General, "limit_binoculars", new CustomFloatRange(0, 180, 2.5f), 0);

            HeaderCreate(OptionType.General, "map");

            //Create(OptionType.General, "random_map", [()=>"",], 0);
            CreateRoleOption();

        }
        public static string GetTeamColor(Teams teams)
        {
            Color c = RoleData.GetColorFromTeams(teams);
            return "#" + ColorUtility.ToHtmlStringRGB(Helper.ColorEditHSV(c, s: -0.2f, v: 0.2f));
        }
        public static Dictionary<Teams, CustomOption> TeamOptions_Count = new();
        public static Dictionary<Roles, CustomOption> RoleOptions_Count = new();
        public static Dictionary<Roles, List<CustomOption>> roleFamilarOptions = new();
        public static void CreateRoleOption()
        {
            HeaderCreate(OptionType.Roles, $"rolemax");
            //teamoptions.Add( Teams.Crewmate,Create(OptionType.Roles, $"team_{Teams.Crewmate.ToString().ToLower()}", GetCountList(), 0,colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Impostor, Create(OptionType.Roles, $"team_{Teams.Impostor.ToString().ToLower()}", new CustomIntRange(0, 15, 1), 0, colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Madmate, Create(OptionType.Roles, $"team_{Teams.Madmate.ToString().ToLower()}", new CustomIntRange(0, 15, 1), 0, colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Jackal, Create(OptionType.Roles, $"team_{Teams.Jackal.ToString().ToLower()}", new CustomIntRange(0, 15, 1), 0, colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Jester, Create(OptionType.Roles, $"team_{Teams.Jester.ToString().ToLower()}", new CustomIntRange(0, 15, 1), 0, colorcode: "#cccccc"));
            foreach (Teams team in RoleData.GetAllTeams())
            {
                if (Teams.None == team) continue;
                if ((int)team >= 6) break;
                Logger.Info($"{team}", $"teamLogger_{team}");

                HeaderCreate(OptionType.Roles, $"team_{team}", colorcode: "#cccccc");
                if (RoleData.GetCustomRoles.Count > 0)
                {
                    foreach (CustomRole role in RoleData.GetCustomRoles.Where(x => x.Team == team))
                    {
                        Logger.Info($"{role}", $"roleLogger_{role.Role}");
                        if (RoleData.GetCustomRole_NormalFromTeam(role.Team).Role != role.Role)
                        {

                            RoleOptions_Count.Add(role.Role, Create(OptionType.Roles, $"role_{role.Role}_count", new CustomIntRange(0, 15, 1), 0));
                        }
                    }
                    //GetLink.CustomTeamLink.First(x => x.Team.ToString().ToLower() == ids[1].ToLower()).ColoredTeamName;

                }



            }
            Logger.Info("OptionCreating...");
            //HeaderCreate(OptionType.Impostor, $"team_impostor");
            //HeaderCreate(OptionType.Crewmate, $"team_crewmate");
            //HeaderCreate(OptionType.Neutral, $"team_jackal");

            foreach (var customrole in RoleData.GetCustomRoles)
            {
                if (customrole == null)
                {
                    //Logger.Info("what?");
                    continue;
                }
                Logger.Info(customrole.Role.ToString());
                roleFamilarOptions.Add(customrole.Role, []);
                CustomOption h;
                switch (customrole.Team)
                {
                    case Teams.Crewmate:
                        h = HeaderCreate(OptionType.Crewmate, $"role_{customrole.Role}_header");
                        break;
                    case Teams.Impostor:
                    case Teams.Madmate:
                        h = HeaderCreate(OptionType.Impostor, $"role_{customrole.Role}_header");
                        break;
                    default:
                        h = HeaderCreate(OptionType.Neutral, $"role_{customrole.Role}_header");
                        break;
                }
                roleFamilarOptions[customrole.Role].Add(h);
                customrole.OptionCreate();
                foreach (var option in customrole.Options)
                {
                    roleFamilarOptions[customrole.Role].Add(option);
                }
                Logger.Info($"{customrole.RoleName} OptionCreating...", "");

            }
        }
    }
}
