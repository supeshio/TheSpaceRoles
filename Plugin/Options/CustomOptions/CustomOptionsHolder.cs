////using HarmonyLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using static TheSpaceRoles.CustomOption;
//using static TheSpaceRoles.Translation;


//namespace TheSpaceRoles
//{

//    public static class CustomOptionsHolder
//    {
//        public static List<List<CustomOption>> Options => [
//            TSROptions,
//            RoleOptions

//            ];
//        public static List<CustomOption> RoleOptions = [];
//        public static List<CustomOption> TSROptions = [];
//        public static void AllCheck()
//        {
//            foreach (CustomOptionSelectorSetting value in Enum.GetValues(typeof(CustomOptionSelectorSetting)))
//            {

//                if (TSROptions == null || TSROptions.Count == 0) continue;
//                int b = 0;
//                Logger.Info(TSROptions.Count.ToString());


//                for (int i = 0; i < TSROptions.Count; i++)
//                {
//                    if (TSROptions[i].obj_parent == value)
//                    {
//                        var op = TSROptions[b];
//                        if (op == null) continue;
//                        op.Check(b);
//                        b++;

//                    }
//                }
//            }
//        }
//        public static Func<string>[] GetSeconds(float sec = 60f, float delta_sec = 2.5f)
//        {

//            List<Func<string>> second = [];
//            for (float i = 0; i <= sec; i += delta_sec)
//            {
//                second.Add(Sec(i));
//            }
//            return [.. second];
//        }
//        public static Func<string>[] GetSecondsIncludeUnlimited(float sec = 60f, float delta_sec = 2.5f, bool include_off = true)
//        {

//            List<Func<string>> second = [];

//            second.Add(() => GetString("option.selection.unlimited"));
//            if (include_off) second.Add(Sec(0));

//            for (float i = 0; i <= sec; i += delta_sec)
//            {
//                second.Add(Sec(i));
//            }
//            return [.. second];
//        }
//        public static Func<string> GetSecond(float sec)
//        {
//            return sec.ToString;
//        }
//        public static Func<string>[] GetRateList()
//        {
//            return [() => "0%", () => "10%", () => "20%", () => "30%", () => "40%", () => "50%", () => "60%", () => "70%", () => "80%", () => "90%", () => "100%"];

//        }

//        public static Func<string>[] GetCountList()
//        {
//            return [() => "0", () => "1", () => "2", () => "3", () => "4", () => "5", () => "6", () => "7", () => "8", () => "9", () => "10", () => "11", () => "12", () => "13", () => "14", () => "15"];

//        }
//        public static void CreateRoleOptions(Teams team, Roles role)
//        {

//            RoleCreate(team, role, "spawncount", GetCountList(), () => "0", onChange: () => RoleOptionTeamRoles.RoleOptionsInTeam.ToArray().Do(x => x.CheckCount()));
//            RoleCreate(team, role, "spawnrate", GetRateList(), () => "0%"); ;
//        }
//        public static void CreateCustomOptions()
//        {
//            if (TSROptions.Count != 0) return;

//            TSRCreate(CustomOptionSelectorSetting.InformationEquipment, "use_records_admin", true);
//            TSRCreate(CustomOptionSelectorSetting.InformationEquipment, "limit_admin", GetSecondsIncludeUnlimited(180), Unlimited());

//            TSRCreate(CustomOptionSelectorSetting.InformationEquipment, "limit_vital", GetSecondsIncludeUnlimited(180), Unlimited());

//            TSRCreate(CustomOptionSelectorSetting.InformationEquipment, "limit_camera", GetSecondsIncludeUnlimited(180), Unlimited());

//            TSRCreate(CustomOptionSelectorSetting.InformationEquipment, "limit_doorlog", GetSecondsIncludeUnlimited(180), Unlimited());

//            TSRCreate(CustomOptionSelectorSetting.InformationEquipment, "limit_binoculars", GetSecondsIncludeUnlimited(180), Unlimited());

//            /*
//            Create(CustomOptionSelectorSetting.General, "seee", GetSeconds(), ()=>"0"),
//            Create(CustomOptionSelectorSetting.General, "seer", GetSeconds(180), ()=>"0"),

//            Create(CustomOptionSelectorSetting.Starter, "use", GetSeconds(180,1), ()=>"0"),
//            Create(CustomOptionSelectorSetting.Starter, "user", GetSeconds(120,10), ()=>"0"),*/


//            foreach (Roles role in Enum.GetValues(typeof(Roles)))
//            {
//                if (GetLink.CustomRoleLink.Any(x => x.Role == role))
//                {
//                    foreach (var team in GetLink.GetCustomRole(role).teamsSupported)
//                    {

//                        string s = team + "_" + role + "_" + "spawncount";
//                        var value = TSR.Instance.Config.Bind($"Preset{preset}", s, 0).Value;
//                        if (value > 0)
//                        {

//                            RoleOptionTeamRoles.RoleOptionsInTeam.Add(new RoleOptionTeamRoles(team, role));
//                        }
//                    }

//                    string s_ = "-1" + "_" + role + "_" + "spawncount";
//                    var value_ = TSR.Instance.Config.Bind($"Preset{preset}", s_, 0).Value;
//                    if (value_ > 0)
//                    {

//                        RoleOptionTeamRoles.RoleOptionsInTeam.Add(new RoleOptionTeamRoles((Teams)(-1), role));
//                    }
//                }
//            }
//        }

//    }
//}
