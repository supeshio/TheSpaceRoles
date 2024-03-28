using HarmonyLib;
using System.Linq;

namespace TheSpaceRoles.Plugin
{
    [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame))]
    public static class GameEnd
    {
        public static void Prefix()
        {
            DataBase.buttons.Clear();
            HudManagerGame.
            IsGameStarting = false;
            Logger.Info($"EndGame!!\nDeathReasons:\n{string.Join("\n", DataBase.AllPlayerDeathReasons.ToArray().Select(x => $"{DataBase.AllPlayerControls().First(y => y.PlayerId == x.Key).Data.PlayerName}  ({x.Key}):{x.Value}"))}");

        }
    }
}
