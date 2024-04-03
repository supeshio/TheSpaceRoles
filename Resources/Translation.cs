using AmongUs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheSpaceRoles
{
    public static class Translation
    {
        public static Dictionary<string, string[]> tranlatedata = new();
        public static List<string>Errors = new();
        public static void Load()
        {
            var fileName = Assembly.GetExecutingAssembly().GetManifestResourceStream("TheSpaceRoles.Resources.Translation.csv");
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] strs = line.Split(",");

                if (strs[0] == null|| strs.Length < 2|| strs[0] == ""||line == "" || line[0] == '#') continue;
                try
                {
                    List<string> list = new();
                    foreach (string str in strs) 
                    {

                        list.Add(str.Replace("\\n","\n").Replace(", ",","));
                    }
                    tranlatedata.Add(strs[0], list.ToArray());

                }
                catch (Exception)
                {
                    Logger.Info(line);
                }
            }
            Logger.Info(string.Join(",",tranlatedata.Keys));
        }
        public static string Get(string str)
        {

            str = str.ToLower();

            SupportedLangs langId = TranslationController.Instance?.currentLanguage?.languageID ?? DataManager.settings.language.CurrentLanguage ;
            
            int lang = (int)langId +1;
            if (!tranlatedata.ContainsKey(str))
            {
                if (!Errors.Contains(str))
                {

                    Errors.Add(str); Logger.Info(str); return str;
                }
                return str;

            }
            
            var data = tranlatedata[str];
            if(data.Length > lang)
            {
                if (data[lang]=="")
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
        public static string GetString(string str,string[] strs = null)
        {
            string getstr = Get(str);
            if (strs==null) return getstr;


            for (int i = 0; i < strs.Length; i++){
                getstr = getstr.Replace("{"+ i  +"}", strs[i]);
                
            }
            return getstr;
        }
    }
}
