using AmongUs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TheSpaceRoles
{
    public static class Translation
    {
        public static Dictionary<string, string[]> tranlatedata = new();
        public static void Load()
        {
            var fileName = Assembly.GetExecutingAssembly().GetManifestResourceStream("TheSpaceRoles.Resources.Translation.csv");
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] strs = line.Split(",");

                if (strs[0] == "" || line == "" || line[0] == '#') continue;
                try
                {
                    List<string> list = new();
                    foreach (string str in strs)
                    {

                        list.Add(str.Replace("\\n", "\n").Replace(", ", ","));
                    }
                    tranlatedata.Add(strs[0], list.ToArray());

                }
                catch (Exception)
                {
                    Logger.Info(line);
                }
            }
        }
        public static string GetString(string str)
        {

            str = str.ToLower();

            SupportedLangs langId = TranslationController.Instance?.currentLanguage?.languageID ?? DataManager.settings.language.CurrentLanguage;

            int lang = (int)langId + 1;
            if (!tranlatedata.ContainsKey(str)) { Logger.Info(str); return str; }

            var data = tranlatedata[str];
            if (data.Length > lang)
            {
                if (data[lang] == "")
                {
                    return data[0];
                }
                else
                {
                    return data[lang];
                }
            }
            else
            {
                return data[0];
            }
        }
    }
}
