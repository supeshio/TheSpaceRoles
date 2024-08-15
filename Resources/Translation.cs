using AmongUs.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            StreamReader sr = new(fileName);
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
                    return data[1];
                }
                else
                {
                    return data[lang];
                }
            }
            else
            {
                return data[1];
            }
        }

        public static string GetString(string str, string[] strs = null)
        {

            string getstr = Get(str);
            if (strs != null)
            {

                for (int i = 0; i < strs.Length; i++)
                {
                    getstr = getstr.Replace("{" + i + "}", strs[i]);

                }
            }

            var ext = ExtractTextBetweenBrackets(getstr);
            foreach (var item in ext)
            {
                var items = item.Split('.');
                try
                {

                    if (items[0] == "role")
                    {
                        if (items[2] == "coloredname")
                        {
                            getstr = getstr.Replace("{" + item + "}", RoleData.GetCustomRoleFromRole((Roles)Enum.Parse(typeof(Roles), items[1], true)).ColoredRoleName);
                        }
                    }
                    if (items[0] == "team")
                    {
                        if (items[2] == "coloredname")
                        {
                            if (items[1] == "other")
                            {
                                getstr = getstr.Replace("{" + item + "}", Helper.ColoredText(RoleData.GetOtherRolesColor, GetString("team.other.name")));
                            }
                            else
                            {
                                getstr = getstr.Replace("{" + item + "}", RoleData.GetCustomTeamFromTeam((Teams)Enum.Parse(typeof(Teams), items[1], true)).ColoredTeamName);

                            }

                        }
                    }
                }
                catch
                {

                }

            }
            return getstr;
        }
        static List<string> ExtractTextBetweenBrackets(string input)
        {
            List<string> result = new();

            // 正規表現を使用して、"{" と "}" の間のテキストを抽出
            Regex regex = new("{(.*?)}", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            MatchCollection matches = regex.Matches(input);
            result.AddRange(
            // 抽出したテキストをリストに追加
            from Match match in matches
            select match.Groups[1].Value);
            return result;
        }
    }
}
