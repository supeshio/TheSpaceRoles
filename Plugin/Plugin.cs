using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace TheSpaceRoles;

[BepInPlugin(Id, name, version)]
[HarmonyPatch]
[BepInProcess("Among Us.exe")]
public class TSR : BasePlugin
{
    public const string Id = "supeshio.com.github";
    public const string name = "TheSpaceRoles";
    public const string s_name = "TSR";
    public const string c_name = $"<color=#87cefa>{name}";
    public const string cs_name_v = $"<color=#87cefa>{s_name} <color=#5ccbff><size=100%>v{version}";
    public const string c_name_v = $"<color=#87cefa>{name} <color=#5ccbff><size=100%>v{version}";
    public const string version = "0.4.1-beta";
    internal static BepInEx.Logging.ManualLogSource Logger;
    public Harmony Harmony = new(Id);
    public static TSR Instance;
    public static ConfigEntry<bool> LobbyTimer { get; set; }
    public static ConfigEntry<bool> DebugMode { get; set; }

    public override void Load()
    {
        Logger = Log;
        Instance = this;
        Translation.Load();
        CustomColor.Load();
        Harmony.PatchAll();
        // Plugin startup logic
        TheSpaceRoles.Logger.Info($"Plugin {Id} is loaded!");
        LobbyTimer = Config.Bind("Lobby", "LobbyTimer", true, "ロビータイマーを使うか");
        DebugMode = Config.Bind("Debug", "DebugMode", false, "デバッグモードを使うか");
        CustomHatManager.LoadHats();
        Instance = new TSR();
    }

    /*
    - 後でやること
    役職設定時バグ治す
     
     
     
     
     
     
     
     
     
     
     
     
     */
}
