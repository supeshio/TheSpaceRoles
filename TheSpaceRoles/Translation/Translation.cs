using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TSR
{
    public static class Translation
    {
        private static Dictionary<string, string> MainTranslation=[];
        private static Dictionary<string,Dictionary<string, string> > AllTranslations = [];
        private static Dictionary<string,string> TranslateId;//id,その言語名
        public static void ReLoad()
        {
            AllTranslations = Assets.ResourcesLoader.LoadTranslations();
            TranslateId = [];
            foreach (var file in AllTranslations) { 
            
                TranslateId.Add(file.Key,file.Value["id"]);
                Logger.Info(file.Key, file.Value["id"], "Translation");
            }
            if(TranslateId.ContainsKey(TSR.TranslationId.Value))
            {
                MainTranslation = AllTranslations[TSR.TranslationId.Value];
                Logger.Info($"Loaded Translation : {TranslateId[TSR.TranslationId.Value]}");
            }
            else
            {
                if (AllTranslations.Count > 0)
                {
                    var first = AllTranslations.GetEnumerator();
                    first.MoveNext();
                    MainTranslation = first.Current.Value;
                    Logger.Info($"Not Found {TSR.TranslationId.Value} Translation, Loaded First Translation : {TranslateId[first.Current.Key]}");
                }
                else
                {
                    MainTranslation = [];
                    Logger.Warning($"Not Found Any Translation");
                }
            }
        }
        public static string Get(string key)
        {
            if (MainTranslation.TryGetValue(key.ToLower(),out var value))
            {
                return value;
            }
            else
            {
                return key.ToLower();
            }
        }
    }
}