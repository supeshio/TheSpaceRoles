using HarmonyLib;
using System.Linq;


namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class GameEnd
    {

        public static void CustomRpcEndGame(WinCheck.WinData data)
        {
            GameManager.Instance.RpcEndGame((GameOverReason)data.wincondition + 20, true);
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
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        public class OnGameEndPatch
        {
            private static void Prefix(AmongUsClient __instance, [HarmonyArgument(0)] ref EndGameResult endGameResult)
            {
                WinCheck.Check();
                WinCheck.WinCondition c = (WinCheck.WinCondition)((int)endGameResult.GameOverReason - 20);
                Logger.Info(c.ToString());
            }
        }
        [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
        public static class CheckEndCriteriaPatch
        {
            public static bool Prefix(LogicGameFlowNormal __instance)
            {
                var data = WinCheck.Check();
                if (data.Winners.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;    
                }
            }

        }
    }
}


