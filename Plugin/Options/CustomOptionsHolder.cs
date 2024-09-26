using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Translation;

namespace TheSpaceRoles
{
    public static class CustomOptionsHolder
    {
        public static Func<string>[] GetCounts(int sec = 15, float delta_sec = 1, bool include_0 = true)
        {

            List<Func<string>> count = [];
            ;
            if (include_0) count.Add(Count(0));

            for (float i = delta_sec; i <= sec; i += delta_sec)
            {
                count.Add(Count(i));
            }
            return [.. count];
        }
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
        public static Func<string>[] GetAreasize(float size = 5f, float delta_size = 1f, bool include_0 = true)
        {

            List<Func<string>> areas = [];
            if (include_0) areas.Add(() => Translation.GetString("option.selection.areasize", ["0"]));

            for (float i = delta_size; i <= size; i += delta_size)
            {
                Logger.Info(i.ToString());
                string area = Translation.GetString("option.selection.areasize", [i.ToString()]);
                areas.Add(() => area);
            }
            return [.. areas];
        }
        public static List<float> KillDistances = new() { 0.5f, 1f, 1.8f, 2.5f };
        public static Func<string>[] GetKillDistances(bool defaultkilldistance = true)
        {

            List<string> strings = ["veryshort", "short", "medium", "long"];
            List<Func<string>> killarea = [];//short
            foreach (string i in strings)
            {
                killarea.Add(() => Translation.GetString("option.selection.killdistance." + i));
            }
            if (defaultkilldistance)
            {

                killarea.Add(() => Translation.GetString("option.selection.killdistance.default"));
            }
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
                if ((int)team >= 6) break;
                Logger.Info($"{team}", $"teamLogger_{team}");

                HeaderCreate(OptionType.Roles, $"team_{team}", colorcode: "#cccccc");
                if (RoleData.GetCustomRoles.Count > 0)
                {
                    foreach (CustomRole role in RoleData.GetCustomRoles.Where(x => x.team == team))
                    {
                        Logger.Info($"{role}", $"roleLogger_{role.Role}");
                        if (RoleData.GetCustomRole_NormalFromTeam(role.team).Role != role.Role)
                        {

                            RoleOptions_Count.Add(role.Role, Create(OptionType.Roles, $"role_{role.Role}_count", GetCountList(), 0));
                        }
                    }
                    //GetLink.CustomTeamLink.First(x => x.Team.ToString().ToLower() == ids[1].ToLower()).ColoredTeamName;

                }



            }
            Logger.Fatel("what?", "teamimpostors");
            //HeaderCreate(OptionType.Impostor, $"team_impostor");
            //HeaderCreate(OptionType.Crewmate, $"team_crewmate");
            //HeaderCreate(OptionType.Neutral, $"team_jackal");

            foreach (var customrole in RoleData.GetCustomRoles)
            {
                if (customrole == null)
                {
                    Logger.Info("what?");
                    continue;
                }
                Logger.Info(customrole.Role.ToString());
                roleFamilarOptions.Add(customrole.Role, []);
                CustomOption h;
                switch (customrole.team)
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
            }
        }
    }
}
