using System;
using System.Collections.Generic;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Translation;

namespace TheSpaceRoles
{
    public static class CustomOptionsHolder
    {
        public static Func<string>[] GetSecondsIncludeUnlimited(float sec = 60f, float delta_sec = 2.5f, bool include_off = true)
        {

            List<Func<string>> second = [];

            second.Add(() => GetString("option.selection.unlimited"));
            if (include_off) second.Add(Sec(0));

            for (float i = 0; i <= sec; i += delta_sec)
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
        public static void CreateRoleOptions(Teams team, Roles role)
        {

            //RoleCreate(team, role, "spawncount", GetCountList(), () => "0", onChange: () => RoleOptionTeamRoles.RoleOptionsInTeam.ToArray().Do(x => x.CheckCount()));
            //RoleCreate(team, role, "spawnrate", GetRateList(), () => "0%"); ;
        }
        public static void CreateCustomOptions()
        {
            if (CustomOption.options.Count != 0) return;
            HeaderMasked.Create(OptionType.General, "admin");
            //Create(OptionType.Default, "use_records_admin", true);
            Create(OptionType.General, "use_records_admin", true);
            Create(OptionType.General, "limit_admin", GetSecondsIncludeUnlimited(180), 0);

            Create(OptionType.General, "limit_vital", GetSecondsIncludeUnlimited(180), 0);

            Create(OptionType.General, "limit_camera", GetSecondsIncludeUnlimited(180), 0);

            Create(OptionType.General, "limit_doorlog", GetSecondsIncludeUnlimited(180), 0);

            Create(OptionType.General, "limit_binoculars", GetSecondsIncludeUnlimited(180), 0);

            /*
            Create(CustomOptionSelectorSetting.General, "seee", GetSeconds(), ()=>"0"),
            Create(CustomOptionSelectorSetting.General, "seer", GetSeconds(180), ()=>"0"),

            Create(CustomOptionSelectorSetting.Starter, "use", GetSeconds(180,1), ()=>"0"),
            Create(CustomOptionSelectorSetting.Starter, "user", GetSeconds(120,10), ()=>"0"),*/


        }
    }
}
