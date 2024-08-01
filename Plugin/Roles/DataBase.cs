using System;
using System.Collections.Generic;
using System.Linq;

namespace TheSpaceRoles
{
    public static class DataBase
    {
        /// <summary>
        /// playerId,RoleMaster型で役職の型を入れれる
        /// </summary>
        public static Dictionary<int, CustomRole[]> AllPlayerRoles = [];//playerId,roles

        /// <summary>
        /// playerId,Teams型で陣営型を入れれる
        /// </summary>
        public static Dictionary<int, Teams> AllPlayerTeams = [];//playerId,Teams
        public static Dictionary<int, DeathReason> AllPlayerDeathReasons = [];

        public static List<CustomButton> buttons = [];
        public static PlayerControl[] AllPlayerControls()
        {
            return PlayerControl.AllPlayerControls.ToArray().Where(x => !x.isDummy).ToArray();
        }
        /// <summary>
        /// VoteAreaのすべてのプレイヤー
        /// </summary>
        /// <returns>nullの可能性あり</returns>
        public static PlayerVoteArea[] AllPlayerMeeting()
        {
            return MeetingHud.Instance.playerStates.ToArray();
        }
        /// <summary>
        /// RESET!!!!!!
        /// </summary>
        public static void ButtonsPositionSetter()
        {

            //List<Tuple<int,ActionButton>> actionButton = [];
            //List<ActionButton> ac = [.. HudManager.Instance.transform.transform.FindChild("Buttons").FindChild("BottomRight").GetComponentsInChildren<ActionButton>()];
            //for (int i = 0; i < ac.Count; i++)
            //{
            //    var v = ac[i].gameObject;
            //    if (v.active)
            //    {
            //        if (v.gameObject.name == "Dummy")
            //        {
            //            ac.RemoveAt(i);
            //            GameObject.Destroy(v.gameObject);
            //            continue;
            //        }
            //        actionButton.Add((i,ac[i]).ToTuple());
            //    }
            //    else
            //    {

            //    }
            //} 
            //for (int i = 0;i < actionButton.Count; i++)
            //{
            //    if (actionButton[i].Item2.gameObject.name == "Dummy") continue;
            //    int pos = 0;
            //    Logger.Message
            //        (actionButton[i].Item2.gameObject.name);
            //    if (buttons.Any(x => x.Name == actionButton[i].Item2.gameObject.name))
            //    {
            //        pos= (int)buttons.First(x => x.Name == actionButton[i].Item2.gameObject.name).ButtonPosition;
            //        Logger.Message(actionButton[i].Item2.gameObject.name + " で一致");
            //    }
            //    else
            //    {
            //        pos = actionButton[i].Item2.gameObject.name switch
            //        {
            //            "UseButton" => 0,
            //            "PetButton" => 0,
            //            "ReportButton" => 1,
            //            "SabotageButton" => 2,
            //            "KillButton" => 3,
            //            "AdminButton (HideNSeek)" => 2,
            //            "AbilityButton" => 4,
            //            "VentButton" => 5,
            //            "Dummy" => -1,
            //            _ => 6
            //        };
            //    }
            //    Logger.Info(actionButton[i].ToString());
            //    if (pos> actionButton[i].Item1)
            //    {      
            //        List<string> list = new List<string>();
            //        for (int j = 0; j < pos - actionButton[i].Item1; j++)
            //        {
            //            Logger.Message(actionButton[i].Item2.gameObject.name);
            //            if (!list.Contains(actionButton[i].Item2.gameObject.name))
            //            {

            //                list.Add(actionButton[i].Item2.gameObject.name);
            //                Logger.Message($"{pos - actionButton[i].Item1} ) {j}");
            //                var action = GameObject.Instantiate((ActionButton)HudManager.Instance.UseButton);
            //                action.transform.parent = HudManager.Instance.transform.transform.FindChild("Buttons").FindChild("BottomRight");
            //                action.gameObject.name = "Dummy";
            //                action.gameObject.SetActive(true);
            //                action.canInteract = false;
            //                action.graphic.color = Helper.ColorFromColorcode("#00000000");
            //                action.transform.SetSiblingIndex(actionButton[i].Item1 - 1);
            //                actionButton.Insert(i - 1, (actionButton[i].Item1, action).ToTuple());
            //            }
            //        }
            //    }
            //    else if(pos < i) 
            //    {

            //    }
            //}
        }

        public static void ResetAndPrepare()
        {
            AllPlayerTeams.Clear();
            AllPlayerRoles.Clear();
            AllPlayerDeathReasons.Clear();
            //buttons.Do(x => GameObject.Destroy(x.actionButton));
            buttons.Clear();

            ButtonsPositionSetter();
            HudManagerGame.IsGameStarting = false;

            HudManagerGame.OnGameStarted = true;

        }

        public static Dictionary<Teams, int> GetPlayerCountInTeam()
        {


            Dictionary<Teams, int> result = [];

            foreach (Teams team in Enum.GetValues(typeof(Teams)))
            {
                result.Add(team, 0);
            }
            foreach (var p in AllPlayerRoles)
            {
                if (p.Value.Any(x => !x.Dead))
                {
                    result[p.Value[0].Team.Team]++;
                }
            }
            return result;
        }
        /// <summary>
        /// Impostor,Jackalじゃないやつの総数
        /// </summary>
        /// <returns></returns>
        public static int GetAsCrewmatePlayerCount()
        {
            int i = 0;
            foreach (var p in AllPlayerRoles)
            {
                if (p.Value.Any(x => !x.Dead) && p.Value[0].Role != Roles.Impostor && p.Value[0].Role != Roles.Jackal)
                {
                    i++;
                }
            }
            return i;
        }
        public static int AlivingNotKillPlayer()
        {
            int i = 0;
            foreach (var team in GetPlayerCountInTeam().Where(x => new Teams[] { Teams.Impostor, Teams.Jackal }.Contains(x.Key)))
            {
                i += team.Value;
            }
            return i;
        }
        public static int AlivingKillPlayer()
        {
            int i = 0;
            foreach (var team in GetPlayerCountInTeam().Where(x => new Teams[] { Teams.Impostor, Teams.Jackal }.Contains(x.Key)))
            {
                i += team.Value;
            }
            return i;
        }
    }

}
