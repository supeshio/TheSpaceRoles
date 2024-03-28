using System;
using System.Collections.Generic;
using System.Linq;
using static TheSpaceRoles.CustomOption;


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

                foreach (var option in Options)
                {
                    if (option == null||option.Count == 0) continue;
                    int b = 0;
                    Logger.Info(option.Count.ToString());
                    for (int i = 0; i < option.Count; i++)
                    {

                        var op = option[i];
                        if (op == null) continue;
                        op.Check(b);
                        if (op.func == null || op.parentId == null)
                        {
                            b++;
                        }
                        else
                        {
                            if (op.func(op.selection))
                            {
                                b++;
                            }
                            else
                            {

                            }

                        }
                    }
                }
            }
            catch(Exception e) 
            {
                Logger.Error(e.Source + "\n" + e.Message +"\n"+e.StackTrace);
            }
        }
        
        public static void CreateCustomOptions()
        {
            if (TSROptions.Count != 0) return;
            List<string> second = [];
            for (float i = 0; i < 100; i+=2.5f)
            {
                second.Add(i.ToString());
            }
            string[] sec = [.. second];

            TSROptions = [
            Create(setting.TSRSettings, "use_admin", true),
            Create(setting.TSRSettings, "use_recodes_admin", true, "use_admin", OnOff),
            Create(setting.TSRSettings, "use", sec, "0"),
            Create(setting.TSRSettings, "user", sec, "0")
            
            ];
        }

    }
}
