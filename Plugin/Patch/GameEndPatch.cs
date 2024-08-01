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


                HudManagerGame.
                IsGameStarting = false;
                //Logger.Info($"EndGame!!\nDeathReasons:\n{string.Join("\n", DataBase.AllPlayerDeathReasons.ToArray().Select(x => $"{DataBase.AllPlayerControls().First(y => y.PlayerId == x.Key).Data.PlayerName}  ({x.Key}):{x.Value}"))}");

            }
        }
        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnGameEnd))]
        private static class OnGameEndPatch
        {
            private static GameOverReason reason = GameOverReason.HumansDisconnect;
            private static void Prefix([HarmonyArgument(0)] ref EndGameResult endGameResult)
            {
                reason = endGameResult.GameOverReason;
                if ((int)endGameResult.GameOverReason >= 10) endGameResult.GameOverReason = GameOverReason.ImpostorByKill;
            }
            private static void Postfix()
            {
                if ((int)reason >= 10)
                {
                    var winteam = (Teams)(reason - 10);
                    EndGameResult.CachedWinners = new();
                    var v = EndGameResult.CachedWinners;

                    Logger.Info("WinTeam:" + winteam.ToString());
                    foreach (var item in DataBase.AllPlayerRoles)
                    {
                        if (item.Value.Any(c => c.Team.Team == winteam))
                        {

                            v.Add(new CachedPlayerData(DataBase.AllPlayerControls().First(y => y.PlayerId == item.Key).Data));
                        }
                        else if (item.Value.Any(c => c.Team.AdditionalWinCheck(winteam)))
                        {
                            v.Add(new CachedPlayerData(DataBase.AllPlayerControls().First(y => y.PlayerId == item.Key).Data));
                            item.Value.DoIf(x => x.Team.AdditionalWinCheck(winteam) && !AdditionalWinnerTeams.Contains(x.Team.Team), x => AdditionalWinnerTeams.Add(x.Team.Team));
                        }
                        else
                        {
                            if (v.ToArray().Any(x => x.PlayerName == DataBase.AllPlayerControls().First(y => y.PlayerId == item.Key).Data.PlayerName))
                            {
                                v.ToArray().ToList().RemoveAll(x => x.PlayerName == DataBase.AllPlayerControls().First(y => y.PlayerId == item.Key).Data.PlayerName);

                            }
                        }
                    }
                    EndGameResult.CachedWinners = v;
                }
                else
                {
                    var gameOverReason = (CustomGameOverReason)reason;

                }
            }
        }
        [HarmonyPatch(typeof(LogicGameFlowNormal), nameof(LogicGameFlowNormal.CheckEndCriteria))]
        private static class GameManagerFixedUpdate
        {
            private static bool Prefix()
            {
                if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started || AmongUsClient.Instance?.GameState == null) return false;
                /*string str = "";
                foreach (var item in DataBase.GetPlayerCountInTeam())
                {
                    str +="\n"+ item.Key.ToString() + ","+item.Value.ToString();
                }
                Logger.Info(str);*/
                System.Collections.Generic.List<Teams> WinTeams = [];
                foreach (var roles in DataBase.AllPlayerRoles)
                {
                    foreach (var item in roles.Value)
                    {
                        if (item.Team.WinCheck())
                        {
                            if (!WinTeams.Contains(item.Team.Team))
                            {
                                WinTeams.Add(item.Team.Team);
                            }
                        }
                    }
                }
                if (WinTeams.Count > 0)
                {
                    Logger.Info("WinnerTeam:" + WinTeams[0].ToString());
                    System.Collections.Generic.List<Teams> AdditionalWinTeams = [];
                    if (WinTeams.Count == 1)
                    {
                        CustomRpcEndGame(WinTeams[0], [.. AdditionalWinnerTeams]);
                        return false;
                    }
                    else if (WinTeams.Count > 1)
                    {
                        Logger.Info("bugbug");
                    }
                }

                return false;
            }
        }
    }
}
