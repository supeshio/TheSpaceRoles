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
            Color c =GetLink.ColorFromTeams(teams);
            return "#" + ColorUtility.ToHtmlStringRGB(Helper.ColorEditHSV(c,s:-0.2f,v:0.2f));
        }
        public static void CreateRoleOption()
        {
            HeaderCreate(OptionType.Roles, $"rolemax"); 
            Create(OptionType.Roles, $"team_{Teams.Crewmate.ToString().ToLower()}", GetCountList(), 0,colorcode: "#cccccc");
            Create(OptionType.Roles, $"team_{Teams.Impostor.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc");
            Create(OptionType.Roles, $"team_{Teams.Madmate.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc");
            Create(OptionType.Roles, $"team_{Teams.Jackal.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc");
            Create(OptionType.Roles, $"team_{Teams.Jester.ToString().ToLower()}", GetCountList(), 0, colorcode: "#cccccc");
            Logger.Info(ColorUtility.ToHtmlStringRGB(GetLink.ColorFromTeams(Teams.Jester)));
            foreach (Teams team in GetLink.GetAllTeams())
            {
                if (Teams.None == team) continue;
                if ((int)team >= 6) return;
                HeaderCreate(OptionType.Roles, $"team_{team}", colorcode:"#cccccc");
                foreach (CustomRole role in GetLink.CustomRoleLink)
                {
                    if (role.teamsSupported.Contains(team))
                    {

                        Create(OptionType.Roles, $"role_{role.Role}_count", GetCountList(), 0);
                    }
                }
                //GetLink.CustomTeamLink.First(x => x.Team.ToString().ToLower() == ids[1].ToLower()).ColoredTeamName;



            }
        }
    }
}
