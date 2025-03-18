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
        public Roles Role=> CustomRole.Role;
        public Teams Team => CustomRole.Team;
        public CustomTeam CustomTeam => CustomRole.CustomTeam;
        public DeathReason DeathReason;
        public Vector2 DeathPosition = Vector2.zero;
        public int DeathMeetingCount = int.MinValue;
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

        //public static Dictionary<int, CustomRole> AllPlayerRoles = [];//playerId,roles
        public static int MeetingCount;
        public static Dictionary<int, PlayerData> AllPlayerData = [];

        public static CustomRole[] GetCustomRoles()
        {
            return AllPlayerData.Values.Select(x => x.CustomRole).ToArray();
        }



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
        //public static Dictionary<int, DeathReason> AllPlayerDeathReasons = [];

        public static List<CustomButton> Buttons = [];
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

        public static void ResetAndPrepare()
        {
            MeetingCount = 0;
            AllPlayerData.Clear();
            ResetButtons();
            DeathGhost.DisapperGhosts();
            Logger.Info("DataBaseReseted");

        }
        public static void EndGame()
        {
            HudManagerGame.IsGameStarting = false;

        }
        public static void ResetButtons()
        {
            Buttons.ToArray().Do(x => { try { x.actionButton.gameObject.SetActive(false); GameObject.Destroy(x.actionButton.gameObject); } catch { } });
            Buttons.Clear();
        }

        public static Dictionary<Teams, int> GetPlayerCountInTeam()
        {

            Dictionary<Teams, int> result = [];

            foreach (Teams team in Enum.GetValues(typeof(Teams)))
            {
                result.Add(team, 0);
            }
            foreach (var p in GetCustomRoles())
            {
                if (!p.Dead)
                {
                    result[p.CustomTeam.Team]++;
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
            foreach (var p in AllPlayerData)
            {
                //Logger.Info(p.Value[0].Role.ToString());
                if (!p.Value.CustomRole.Dead && p.Value.CustomRole.Team != Teams.Impostor && p.Value.CustomRole.Team != Teams.Jackal)
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
