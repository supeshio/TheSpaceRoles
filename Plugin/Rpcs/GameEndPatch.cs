using HarmonyLib;
using System.Linq;
using UnityEngine;


namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class GameEnd
    {
        public static void CustomRpcEndGame(Teams winteams, Teams[] additionalwinteams)
        {
            GameManager.Instance.RpcEndGame((GameOverReason)winteams + 10, true);
        }
        public static Teams WinnerTeam = Teams.None;
        public static System.Collections.Generic.List<Teams> AdditionalWinnerTeams = [];
        //public static void CustomEndGame(Teams winteam, Teams[] additionalwinteams)
        //{
        //    WinnerTeam = winteam;
        //    AddtionalWinnerTeams = [.. additionalwinteams];
        //    GameManager.Instance.EndGame();
        //}


        [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame))]
        private static class EndGame
        {
            private static void Prefix()
            {
                DataBase.ResetAndPrepare();
                DataBase.EndGame();
                Logger.Info($"EndGame!!,DeathReasons:\n{string.Join(",\n", DataBase.AllPlayerData.ToArray().Select(x => $"{DataBase.AllPlayerControls().First(y => y.PlayerId == x.Key).Data.PlayerName}  ({x.Key}):{x.Value}"))}");

                DataBase.ResetButtons();
            }
        }
    }
}


