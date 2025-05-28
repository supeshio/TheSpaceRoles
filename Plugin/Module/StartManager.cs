using HarmonyLib;

namespace TheSpaceRoles
{

    public static class GameStartManagerPatch
    {
        [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Start)), HarmonyPostfix]
        public static void Start(GameStartManager __instance)
        {
            DataBase.ResetButtons();
            __instance.HostInfoPanel.playerName.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Normal;
            GameStartManager.Instance.HostInfoPanel.playerName.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Normal;
        }
    }
}
