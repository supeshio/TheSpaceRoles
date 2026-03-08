using BepInEx;
using BepInEx.Unity.IL2CPP;
using BepInEx.Configuration;
using HarmonyLib;
using System.Text;
using TSR.Game;
using TSR.Game.Options;
using TSR.Module.MonoRegister;
using TSR.Module.Translation;
using UnityEngine;

namespace TSR
{
    [BepInPlugin(Id, Name, Version)]
    [BepInProcess("Among Us.exe")]
    public class TSR : BasePlugin
    {
        public const string Id = "supeshio.com.github";
        public const string Name = "TheSpaceRoles";
        public const string Version = "1.0.0";
        public const string ShortName = "TSR";
        public static Color32 Color = new(0x87, 0xCE, 0xFA, 0xff);
        public const string ColoredName = $"<color=#87cefa>{Name}</color>";
        public const string ColoredShortNameAndVersion = $"<color=#87cefa>{ShortName}</color> <color=#5ccbff><size=100%>v{Version}</color>";
        public const string ColoredNameAndVersion = $"<color=#87cefa>{Name}</color> <color=#5ccbff><size=100%>v{Version}</color>";
        internal static BepInEx.Logging.ManualLogSource? Logger;
        public static readonly Harmony Harmony = new Harmony(Id);
        public override void Load()
        {
            System.Console.OutputEncoding = Encoding.UTF8;
            // Plugin startup logic
            Logger = Log;
            global::TSR.Logger.Info($"TSR is loaded!");
            Harmony.PatchAll();
            ConfigInit();
            SmartPatchLoader.PatchAll();
            Assets.AssetLoader.LoadAssetsFromEmbeddedBundle();
            MonoRegister.Register();
            // 翻訳ファイルを読み込む
            Translation.ReLoad();
            CustomOption.Load();
            Register.Init();
        }
        public static ConfigEntry<bool> DebugMode;
        public static ConfigEntry<string> TranslationId;

        private void ConfigInit()
        {
            DebugMode = Config.Bind("General", "DebugMode", false, "DebugMode");
            TranslationId = Config.Bind("General", "TranslationId", "ja_jp", "TranslationId");
        }
    }
}

//TODO: help