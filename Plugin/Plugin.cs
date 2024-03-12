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
    public const string c_name = $"<color=#5ccbff> {s_name} v{version}";
    public const string cs_name = $"<color=#5ccbff> {s_name} <size=80%>v{version}";
    public const string version = "0.1.2-beta";
    internal static BepInEx.Logging.ManualLogSource Logger;
    public Harmony Harmony = new(Id);

    public static ConfigEntry<bool> LobbyTimer { get; set; }

    public override void Load()
    {
        Logger = Log;
        Translation.Load();
        Harmony.PatchAll();
        // Plugin startup logic
        TheSpaceRoles.Logger.Info($"Plugin {Id} is loaded!");
        LobbyTimer = Config.Bind("Lobby", "LobbyTimer", true, "ロビータイマーを使うか");
    }

    /*
     後でやること
    playerJoin時に部屋爆破までの時間を伝える。
     
     
     
     
     
     
     
     
     
     
     
     
     */
}
