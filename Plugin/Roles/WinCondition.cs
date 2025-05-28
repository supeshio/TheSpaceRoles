using Epic.OnlineServices.Sessions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSpaceRoles
{
    public static class WinCheck
    {
        /// <summary>
        /// WinCondition 無記名Teamsに関しては
        /// キルor切断or投票とします｡
        /// </summary>
        public enum WinCondition
        {
            None = 0,

            Crewmate,
            CrewmateByTask,
            Impostor,
            ImpostorBySabotage,

            Jackal,
            Jester


        }
        public class WinData
        {
            public WinCondition wincondition = WinCondition.None;
            public List<Teams> WinnerTeams = [];
            public List<PlayerControl> Winners = [];
            public WinData(WinCondition wincondition = WinCondition.None, List<Teams> WinnerTeams = null, List<PlayerControl> Winners = null) {
                this.wincondition = wincondition;
                this.WinnerTeams = WinnerTeams;
                this.Winners = Winners;
            }
        }
        public static WinData Check()
        {
            WinData data = Checker();
            if (data.wincondition!=WinCondition.None)
            {
                windata = data;
                //何かしらが勝利した
                GameEnd.CustomRpcEndGame(data);
            }
            return data;
        }
        public static WinData windata = new();
        private static WinData Checker()
        {
            WinCondition wincondition = WinCondition.None;
            List<Teams> WinnerTeams = [];
            List<PlayerControl> Winners = [];
            /*
             * 勝利条件をまとめてみる
             * 
             * インポスター = インポスターの総数
             * クルーメイト = クルー陣営としてカウントされる者の総数
             * ジャッカル = Roles.Jackal等ジャッカル陣営の者
             * 
             * 
             * Crewmate :
             * 1. クルー以外のキル陣営(仮)を追放､キルなどして船からいなくさせる
             * 2. タスクを終わらせる
             * 
             * Impostor:
             * 1. クルーメイト =< インポスター かつ ジャッカル == 0 を満たす
             * 2. サボタージュ解除が間に合わない
             * 
             * Jackal:
             * 1. クルーメイト =< ジャッカル かつ インポスター == 0
             * 
             * Jester:
             * 1.追放される｡
             * 
             * Opportunist:
             * 1.生き残った状態で他の陣営が勝利する｡
             */

            if (PlayerControl.LocalPlayer?.GetCustomRole() == null)
            {
                Logger.Warning("CustomRole doesnt work");
            }


            int ImpostorCount = 0;
            int JackalCount = 0;
            int CrewmateCount = 0;
            int PlyaerCount = 0;
            List<PlayerControl> ExiledJesters = [];
            foreach (PlayerControl pc in PlayerControl.AllPlayerControls)
            {
                Teams t = pc.GetCustomRole().CheckCount();
                switch (t)
                {
                    case Teams.Crewmate:
                        CrewmateCount++;
                        break;

                    case Teams.Jackal:
                        JackalCount++;
                        break;

                    case Teams.Impostor:
                        ImpostorCount++;
                        break;

                    case Teams.Jester:
                        if (pc.GetCustomRole().Exiled == true)
                        {
                            ExiledJesters.Add(pc);
                        }
                        break;
                    default:
                        Logger.Warning("This WinCheckRole is not supported");
                        break;
                }
                PlyaerCount++;


            }

            if (ExiledJesters.Count>0)
            {

                //ジェスター勝利
                
                wincondition= WinCondition.Jester;
                WinnerTeams.Add(Teams.Jester);
                ExiledJesters.ForEach(x=> Winners.Add(x));
            }



            if (GameData.Instance.TotalTasks == GameData.Instance.CompletedTasks)
            {
                //クルー勝利
                wincondition = WinCondition.CrewmateByTask;
                WinnerTeams.Add(Teams.Crewmate);
            }
            else if (/*サボ解除失敗時処理をしたい*/CheckAndEndGameForSabotageWin(ShipStatus.Instance))
            {
                //インポスター勝利
                wincondition = WinCondition.ImpostorBySabotage;
                WinnerTeams.Add(Teams.Impostor);
            }
            else if (ImpostorCount == 0 && JackalCount == 0 && CrewmateCount > 0)
            {
                //クルー勝利
                wincondition = WinCondition.Crewmate;
                WinnerTeams.Add(Teams.Crewmate);
            }
            else if (ImpostorCount > 0 && ImpostorCount >= CrewmateCount && JackalCount == 0)
            {
                //インポスター勝利
                wincondition = WinCondition.Impostor;
                WinnerTeams.Add(Teams.Impostor);
            }
            else if (JackalCount > 0 && JackalCount >= CrewmateCount && ImpostorCount == 0)
            {
                //ジャッカル勝利
                wincondition = WinCondition.Jackal;
                WinnerTeams.Add(Teams.Jackal);
            }

            //bool Opportunist()
            //{

            //}







            wincondition = WinCondition.None;
            return new WinData(wincondition,WinnerTeams,Winners);
        }
        private static bool CheckAndEndGameForSabotageWin(ShipStatus __instance)
        {
            if (__instance.Systems == null) return false;
            ISystemType systemType = __instance.Systems.ContainsKey(SystemTypes.LifeSupp) ? __instance.Systems[SystemTypes.LifeSupp] : null;
            if (systemType != null)
            {
                LifeSuppSystemType lifeSuppSystemType = systemType.TryCast<LifeSuppSystemType>();
                if (lifeSuppSystemType != null && lifeSuppSystemType.Countdown < 0f)
                {
                    lifeSuppSystemType.Countdown = 10000f;
                    return true;
                }
            }
            ISystemType systemType2 = __instance.Systems.ContainsKey(SystemTypes.Reactor) ? __instance.Systems[SystemTypes.Reactor] : null;
            if (systemType2 == null)
            {
                systemType2 = __instance.Systems.ContainsKey(SystemTypes.Laboratory) ? __instance.Systems[SystemTypes.Laboratory] : null;
            }
            if (systemType2 != null)
            {
                ICriticalSabotage criticalSystem = systemType2.TryCast<ICriticalSabotage>();
                if (criticalSystem != null && criticalSystem.Countdown < 0f)
                {
                    criticalSystem.ClearSabotage();
                    return true;
                }
            }
            return false;
        }
    }
}
