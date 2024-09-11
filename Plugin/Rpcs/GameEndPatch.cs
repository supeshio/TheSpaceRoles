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
                DataBase.buttons.Clear();
                HudManagerGame.
                IsGameStarting = false;
                Logger.Info($"EndGame!!\nDeathReasons:\n{string.Join("\n", DataBase.AllPlayerDeathReasons.ToArray().Select(x => $"{DataBase.AllPlayerControls().First(y => y.PlayerId == x.Key).Data.PlayerName}  ({x.Key}):{x.Value}"))}");

                if (DataBase.buttons.Count != 0)
                {
                    foreach (var item in DataBase.buttons)
                    {
                        try
                        {

                            GameObject.Destroy(item.actionButton.gameObject);
                        }
                        catch
                        {

                        }


                    }
                    DataBase.buttons.Clear();
                }
            }
        }
    }
}


