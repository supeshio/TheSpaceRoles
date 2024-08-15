using System;
using System.Collections.Generic;
using UnityEngine;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Translation;

namespace TheSpaceRoles
{
    public static class CustomOptionsHolder
    {
        public static Func<string>[] GetSecondsIncludeUnlimited(float sec = 60f, float delta_sec = 2.5f, bool include_0 = true)
        {

            List<Func<string>> second = [];

            second.Add(() => GetString("option.selection.unlimited"));
            if (include_0) second.Add(Sec(0));

            for (float i = delta_sec; i <= sec; i += delta_sec)
            {
                second.Add(Sec(i));
            }
            return [.. second];
        }
        public static Func<string>[] GetSeconds(float sec = 60f, float delta_sec = 2.5f, bool include_0 = true)
        {

            List<Func<string>> second = [];
;
            if (include_0) second.Add(Sec(0));

            for (float i = delta_sec; i <= sec; i += delta_sec)
            {
                second.Add(Sec(i));
            }
            return [.. second];
        }
        public static Func<string>[] GetKillArea()
        {
            AmongUs.GameOptions.FloatArrayOptionNames.KillDistances
            List<Func<string>> killarea = [];//short
            killarea.Add(()=>"killarea");
            return [.. killarea];
        }
        public static Func<string> GetSecond(float sec)
        {
            return sec.ToString;
        }
        public static Func<string>[] GetRateList()
        {
            return [() => "0%", () => "10%", () => "20%", () => "30%", () => "40%", () => "50%", () => "60%", () => "70%", () => "80%", () => "90%", () => "100%"];

        }

        public static Func<string>[] GetCountList()
        {
            return [() => "0", () => "1", () => "2", () => "3", () => "4", () => "5", () => "6", () => "7", () => "8", () => "9", () => "10", () => "11", () => "12", () => "13", () => "14", () => "15"];

        }
        public static void CreateCustomOptions()
        {
            if (CustomOption.options.Count != 0) return;
            HeaderCreate(OptionType.General, "admin");
            //Create(OptionType.Default, "use_records_admin", true);
            Create(OptionType.General, "limit_admin", GetSecondsIncludeUnlimited(180), 0);

            HeaderCreate(OptionType.General, "vital");
            Create(OptionType.General, "limit_vital", GetSecondsIncludeUnlimited(180), 0);

            HeaderCreate(OptionType.General, "camera");
            Create(OptionType.General, "limit_camera", GetSecondsIncludeUnlimited(180), 0);

            HeaderCreate(OptionType.General, "doorlog");
            Create(OptionType.General, "limit_doorlog", GetSecondsIncludeUnlimited(180), 0);

            HeaderCreate(OptionType.General, "binoculars");
            Create(OptionType.General, "limit_binoculars", GetSecondsIncludeUnlimited(180), 0);

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
            TeamOptions_Count.Add(Teams.Impostor, Create(OptionType.Roles, $"team_{Teams.Impostor.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Madmate, Create(OptionType.Roles, $"team_{Teams.Madmate.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Jackal, Create(OptionType.Roles, $"team_{Teams.Jackal.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc"));
            TeamOptions_Count.Add(Teams.Jester, Create(OptionType.Roles, $"team_{Teams.Jester.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc"));
            foreach (Teams team in RoleData.GetAllTeams())
            {
                if (Teams.None == team) continue;
                if ((int)team >= 6) return;
                HeaderCreate(OptionType.Roles, $"team_{team}", colorcode: "#cccccc");
                foreach (CustomRole role in RoleData.GetCustomRoles)
                {
                    if (role.team == team)
                    {
                        if (RoleData.GetCustomRole_NormalFromTeam(role.team).Role != role.Role)
                        {

                            RoleOptions_Count.Add(role.Role, Create(OptionType.Roles, $"role_{role.Role}_count", GetCountList(), 0));
                        }
                    }
                }
                //GetLink.CustomTeamLink.First(x => x.Team.ToString().ToLower() == ids[1].ToLower()).ColoredTeamName;



            }

            foreach(var customrole in RoleData.GetCustomRoles)
            {
                foreach (var option in customrole.Options)
                {
                    roleFamilarOptions.Add();
                }
            }
        }
    }
}
