using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSpaceRoles{

    public static class GameStartManagerPatch
    {
        [HarmonyPatch(typeof(GameStartManager),nameof(GameStartManager.Start)),HarmonyPostfix]
        public static void Start(GameStartManager __instance)
        {
            __instance.HostInfoPanel.playerName.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Normal;
            GameStartManager.Instance.HostInfoPanel.playerName.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Normal;
        }
    }
}
