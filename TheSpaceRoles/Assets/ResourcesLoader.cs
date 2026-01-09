using MS.Internal.Xml.XPath;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static FilterPopUp.FilterInfoUI;
using static Rewired.Platforms.Custom.CustomPlatformUnifiedKeyboardSource.KeyPropertyMap;

namespace TSR.Assets;

public static class ResourcesLoader
{
    private const string ResourcesDirectory = "TSR";
    private static string ResourcesPath => Path.Combine(Path.GetDirectoryName(Application.dataPath)!, ResourcesDirectory);
    private static string LangPath => Path.Combine(ResourcesPath, "lang");

    public static Dictionary<string, Dictionary<string, string>> LoadTranslations()
    {
        if (!Directory.Exists(ResourcesPath))
            Directory.CreateDirectory(ResourcesPath);
        if (!Directory.Exists(LangPath))
            Directory.CreateDirectory(LangPath);

        var translations = new Dictionary<string, Dictionary<string, string>>();

        // --- 外部 YAML ファイル読み込み ---
        foreach (var file in Directory.GetFiles(LangPath, "*.yaml"))
        {
            YamlParse.Parse(File.ReadAllLines(file), Path.GetFileNameWithoutExtension(file), translations);
        }
        foreach (var file in Directory.GetFiles(LangPath, "*.yml"))
        {
            YamlParse.Parse(File.ReadAllLines(file), Path.GetFileNameWithoutExtension(file), translations);
        }

        // --- 埋め込みリソースから読み込む ---
        var asm = Assembly.GetExecutingAssembly();

        foreach (var resourceName in asm.GetManifestResourceNames())
        {
            if (resourceName.StartsWith("TSR.Resources.lang.") &&
                (resourceName.EndsWith(".yaml") || resourceName.EndsWith(".yml")))
            {
                using var stream = asm.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    string fileName = resourceName
                        .Replace("TSR.Resources.lang.", "")
                        .Replace(".yaml", "")
                        .Replace(".yml", "");
                    using var reader = new StreamReader(stream);
                    List<string> lines = [];
                    //LoadYaml(reader.ReadToEnd(), fileName, translations);
                    while (!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine() ?? string.Empty);
                    }

                    YamlParse.Parse([.. lines], fileName, translations);
                }
            }
            Logger.Info(resourceName);
        }

        foreach (var tr in translations)
        {
            Logger.Info(tr.Key, tr.Value["id"], "Translation");
        }

        return translations;
    }

    
    static void TranslationSet(Dictionary<string, Dictionary<string, string>> translations,string filename,string key,string value)
    {
        Logger.Info($"{key} : {value}",filename);
        translations[filename][key] = value;
    }

    private static class YamlParse
    {
        public static void Parse(string[] lines,string fileName, Dictionary<string, Dictionary<string, string>> translations)
        {
            //var result = new Dictionary<string, string>();
            var keyStack = new List<string>();
            var indentStack = new List<int>();

            foreach (var rawLine in lines)
            {
                if (string.IsNullOrWhiteSpace(rawLine))
                    continue;

                string line = RemoveComment(rawLine);
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                int indent = CountIndent(line);
                string trimmed = line.Trim();

                if (!trimmed.Contains(":"))
                    continue;

                var parts = trimmed.Split(new[] { ':' }, 2);
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                //インデント比較
                while (indentStack.Count > 0 && indent < indentStack[^1])
                {
                    indentStack.RemoveAt(indentStack.Count - 1);
                    keyStack.RemoveAt(keyStack.Count - 1);
                }

                if (indentStack.Count == 0 || indent > indentStack[^1])
                {
                    indentStack.Add(indent);
                    keyStack.Add(key);
                }
                else
                {
                    keyStack[^1] = key;
                }
                if (value.StartsWith('\"') && value.EndsWith('\"'))
                {
                    value = value[1 .. (value.Length - 1)];
                }
                if (!string.IsNullOrEmpty(value))
                {
                    string connectedKey = string.Join(".", keyStack);
                    if(!translations.ContainsKey(fileName)) translations.TryAdd(fileName, new Dictionary<string, string>());
                    translations[fileName][connectedKey] = value;
                }
            }

        }

        static int CountIndent(string s)
        {
            int count = 0;
            foreach (char c in s)
            {
                if (c == ' ')
                    count++;
                else
                    break;
            }
            return count;
        }

        static string RemoveComment(string line)
        {
            int idx = line.IndexOf('#');
            return idx >= 0 ? line.Substring(0, idx) : line;
        }
    }
}