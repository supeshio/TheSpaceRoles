using AmongUs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace TheSpaceRoles
{
    public static class Translation
    {
        public static Dictionary<string, string[]> tranlatedata = new();
        public static List<string> Errors = new();
        public static void Load()
        {
            var fileName = Assembly.GetExecutingAssembly().GetManifestResourceStream("TheSpaceRoles.Resources.Translation.csv");
            StreamReader sr = new StreamReader(fileName);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();

                string[] strs = line.Split(",");

                if (strs[0] == null || strs.Length < 2 || strs[0] == "" || line == "" || line[0] == '#') continue;
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
            Logger.Info(string.Join(",", tranlatedata.Keys));
        }
        public static string Get(string str)
        {

            str = str.ToLower();

            SupportedLangs langId = TranslationController.Instance?.currentLanguage?.languageID ?? DataManager.settings.language.CurrentLanguage;

            int lang = (int)langId + 1;
            if (!tranlatedata.ContainsKey(str))
            {
                if (!Errors.Contains(str))
                {

                    Errors.Add(str); Logger.Info(str); return str;
                }
                return str;

            }

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
        public static string GetString(string str, string[] strs = null)
        {
            string getstr = Get(str);
            if (strs == null) return getstr;


            for (int i = 0; i < strs.Length; i++)
            {
                getstr = getstr.Replace("{" + i + "}", strs[i]);

            }
            var ext = ExtractTextBetweenBrackets(getstr);
            foreach (var item in ext)
            {
                var items = item.Split('.');
                if (items[0]=="role")
                {
                    if (items[2] == "coloredname")
                    {
                        getstr = getstr.Replace("{" + item + "}",GetLink.GetCustomRole((Roles)Enum.Parse(typeof(Roles), items[1])).ColoredRoleName);
                    }
                }
                if (items[0] == "team")
                {
                    if (items[2] == "coloredname")
                    {
                        if (items[3] == "other")
                        {
                            getstr = getstr.Replace("{" + item + "}", Helper.ColoredText(GetLink.GetOtherRolesColor, GetString("team.other.name")));
                        }
                        else
                        {
                            getstr = getstr.Replace("{" + item + "}", Helper.ColoredText(GetLink.ColorFromTeams[(Teams)Enum.Parse(typeof(Teams), items[1], true)], GetString("team" + ((Teams)Enum.Parse(typeof(Teams), items[1])).ToString() + "name")));

                        }

                    }
                }

            }

            return getstr;
        }
       private static bool SWord(string sentence,string word)
        {
            return sentence.StartsWith(word);
        }
        private static string GetText(List<string> str, Dictionary<string,object> dic)
        {
            dic.TryGetValue(str[0], out var text);
            if (text.GetType() != typeof(string))
            {
                var strs = str.DeepCopy();
                strs.RemoveAt(0); 
                text = GetText(str, (Dictionary<string, object>)text);
            }
            return text.ToString();
        }
        static List<string> ExtractTextBetweenBrackets(string input)
        {
            List<string> result = new List<string>();

            // 正規表現を使用して、"{" と "}" の間のテキストを抽出
            Regex regex = new(@"\{(.*?)\}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(input);

            // 抽出したテキストをリストに追加
            foreach (Match match in matches)
            {
                result.Add(match.Groups[1].Value);
            }

            return result;
        }
    }
}
