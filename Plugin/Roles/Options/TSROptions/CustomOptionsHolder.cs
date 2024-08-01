using System;
using System.Collections.Generic;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Translation;


namespace TheSpaceRoles
{
    public static class CustomOptionsHolder
    {
        public static List<List<CustomOption>> Options => [
            TSROptions,
            RoleOptions

            ];
        public static List<CustomOption> RoleOptions = [];
        public static List<CustomOption> TSROptions = [];
        public static void AllCheck()
        {
            try
            {

                foreach (CustomOptionSelectorSetting value in Enum.GetValues(typeof(CustomOptionSelectorSetting)))
                {

                    foreach (var option in Options)
                    {
                        if (option == null || option.Count == 0) continue;
                        int b = 0;
                        Logger.Info(option.Count.ToString());


                        for (int i = 0; i < option.Count; i++)
                        {
                            if (option[i].obj_parent == value)
                            {

                                var op = option[i];
                                if (op == null) continue;
                                op.Check(ref b);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.Source + "\n" + e.Message + "\n" + e.StackTrace);
            }
        }
        public static Func<string>[] GetSeconds(float sec = 60f, float delta_sec = 2.5f)
        {

            List<Func<string>> second = [];
            for (float i = 0; i <= sec; i += delta_sec)
            {
                second.Add(Sec(i));
            }
            return [.. second];
        }
        public static Func<string>[] GetSecondsIncludeUnlimited(float sec = 60f, float delta_sec = 2.5f, bool include_off = true)
        {

            List<Func<string>> second = [];

            second.Add(() => GetString("tsroption.selection.unlimited"));
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

        public static void CreateCustomOptions()
        {
            if (TSROptions.Count != 0) return;

            TSROptions = [
            Create(CustomOptionSelectorSetting.InformationEquipment, "use_records_admin", true),
            Create(CustomOptionSelectorSetting.InformationEquipment, "limit_admin",GetSecondsIncludeUnlimited(180),Unlimited()),

            Create(CustomOptionSelectorSetting.InformationEquipment, "limit_vital",GetSecondsIncludeUnlimited(180),Unlimited()),

            Create(CustomOptionSelectorSetting.InformationEquipment, "limit_camera",GetSecondsIncludeUnlimited(180),Unlimited()),

            Create(CustomOptionSelectorSetting.InformationEquipment, "limit_doorlog",GetSecondsIncludeUnlimited(180),Unlimited()),

            Create(CustomOptionSelectorSetting.InformationEquipment, "limit_binoculars",GetSecondsIncludeUnlimited(180),Unlimited()),

            /*
            Create(CustomOptionSelectorSetting.General, "seee", GetSeconds(), ()=>"0"),
            Create(CustomOptionSelectorSetting.General, "seer", GetSeconds(180), ()=>"0"),

            Create(CustomOptionSelectorSetting.Starter, "use", GetSeconds(180,1), ()=>"0"),
            Create(CustomOptionSelectorSetting.Starter, "user", GetSeconds(120,10), ()=>"0"),*/

            ];
        }

    }
}
