using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using System.Text;
using TSR.Game;
using TSR.Game.Options;
using UnityEngine;

namespace TSR
{
    [BepInPlugin(Id, name, version)]
    [BepInProcess("Among Us.exe")]
    public class TSR : BasePlugin
    {
        public const string Id = "supeshio.com.github";
        public const string name = "TheSpaceRoles";
        public const string version = "1.0.0";
        public const string s_name = "TSR";
        public static Color32 color = new(0x87, 0xCE, 0xFA, 0xff);
        public const string c_name = $"<color=#87cefa>{name}</color>";
        public const string cs_name_v = $"<color=#87cefa>{s_name}</color> <color=#5ccbff><size=100%>v{version}</color>";
        public const string c_name_v = $"<color=#87cefa>{name}</color> <color=#5ccbff><size=100%>v{version}</color>";
        internal static BepInEx.Logging.ManualLogSource? _Logger;
        public static Harmony Harmony = new(Id);

        public override void Load()
        {
            System.Console.OutputEncoding = Encoding.UTF8;
            // Plugin startup logic
            _Logger = Log;
            Logger.Info($"TSR is loaded!");
            Harmony.PatchAll();
            ConfigInit();
            SmartPatchLoader.PatchAll();
            Assets.AssetLoader.LoadAssetsFromEmbeddedBundle();
            // 翻訳ファイルを読み込む
            Translation.ReLoad();
            CustomOption.Load();
            Register.Init();
        }
        public static ConfigEntry<bool> DebugMode;
        public static ConfigEntry<string> TranslationId;
        public void ConfigInit()
        {
            DebugMode = Config.Bind("General", "DebugMode", false, "DebugMode");
            TranslationId = Config.Bind("General", "TranslationId", "ja_jp", "TranslationId");
        }
    }
}

//TODO: help