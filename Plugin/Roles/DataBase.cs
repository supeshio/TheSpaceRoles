using HarmonyLib;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    public class PlayerData
    {
        public PlayerControl pc;
        public CustomRole CustomRole;
        public int PlayerId => pc.PlayerId;
        public string Name;
        public int ColorId;
        public string HatId;
        public string SkinId;
        public string VisorId;
        public string PetId;
        public string NamePlateId;
        public PlayerData(PlayerControl pc,CustomRole customRole)
        {
            this.pc = pc;
            this.Name = pc.CachedPlayerData.PlayerName;
            this.ColorId = pc.CachedPlayerData.DefaultOutfit.ColorId;
            this.PetId = pc.CachedPlayerData.DefaultOutfit.PetId;
            this.SkinId = pc.CachedPlayerData.DefaultOutfit.SkinId;
            this.HatId = pc.CachedPlayerData.DefaultOutfit.HatId;
            this.VisorId = pc.CachedPlayerData.DefaultOutfit.VisorId;
            this.NamePlateId = pc.CachedPlayerData.DefaultOutfit.NamePlateId;
            this.CustomRole = customRole;

        }
    }
    public static class DataBase
    {
        /// <summary>
        /// playerId,RoleMaster型で役職の型を入れれる
        /// </summary>
        public static Dictionary<int, CustomRole> AllPlayerRoles = [];//playerId,roles

        public static Dictionary<int, int> AllPlayerColorIds = [];
        public static Dictionary<int, PlayerData> AllPlayerData = [];

        public static List<Roles> AssignedRoles()
        {

            if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {
                return RoleData.GetCustomRoles.Select(x => x.Role).ToList();
            }

            List<Roles> list = [];
            //アサインされた役職を求める。
            foreach (var roleop in CustomOptionsHolder.RoleOptions_Count)
            {
                if (roleop.Value.Selection() != 0)
                {
                    if (!list.Contains(roleop.Key))
                    {
                        list.Add(roleop.Key);
                    }
                }
            }
            foreach (var teamop in CustomOptionsHolder.TeamOptions_Count)
            {
                if (teamop.Value.Selection() != 0)
                {
                    list.Add(RoleData.GetCustomRole_NormalFromTeam(teamop.Key).Role);
                }
            }
            return list;

        }
        /// <summary>
        /// playerId,Teams型で陣営型を入れれる
        /// </summary>
        public static Dictionary<int, Teams> AllPlayerTeams = [];//playerId,Teams
        public static Dictionary<int, DeathReason> AllPlayerDeathReasons = [];

        public static List<CustomButton> buttons = [];
        public static PlayerControl[] AllPlayerControls()
        {
            return PlayerControl.AllPlayerControls.ToArray();
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
            AllPlayerColorIds.Clear();
            AllPlayerData.Clear();
            ResetButtons();

            ButtonsPositionSetter();
            Logger.Info("DataBaseReseted");

        }
        public static void EndGame()
        {
            HudManagerGame.IsGameStarting = false;

        }
        public static void ResetButtons()
        {

            buttons.ToArray().Do(x => { try { GameObject.Destroy(x.gameObject); } catch { } });
            buttons.Clear();
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
                if (!p.Value.Dead)
                {
                    result[p.Value.CustomTeam.Team]++;
                }
            }
            //Logger.Info($"Impostor :{result[Teams.Impostor]},Crewmate :{GetAsCrewmatePlayerCount()}");
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
                //Logger.Info(p.Value[0].Role.ToString());
                if (!p.Value.Dead && p.Value.Team != Teams.Impostor && p.Value.Team != Teams.Jackal)
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
