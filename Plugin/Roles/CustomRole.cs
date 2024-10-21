using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public abstract class CustomRole
    {
        public int PlayerId;
        public string PlayerName;
        public PlayerControl PlayerControl;
        public Teams team = Teams.None;
        public CustomTeam CustomTeam;
        public Roles Role;
        public Color Color = new(0, 0, 0);
        public bool HasKillButton = false;
        public bool HasAbilityButton = false;
        public int[] AbilityButtonType = [];
        public bool? CanUseVent = null;
        public bool? CanUseAdmin = null;
        public bool? CanUseCamera = null;
        public bool? CanUseVital = null;
        public bool? CanUseDoorlog = null;
        public bool? CanUseBinoculars = null;
        public bool? CanRepairSabotage = null;
        public bool? CanUseVentMoving = null;
        public bool? HasTask = null;
        public List<CustomOption> Options = new List<CustomOption>();
        public void Init()
        {
            CanUseVent = CanUseVent == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseVent : CanUseVent;
            CanUseAdmin = CanUseAdmin == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseAdmin : CanUseAdmin;
            CanUseBinoculars = CanUseBinoculars == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseBinoculars : CanUseBinoculars;
            CanUseCamera = CanUseCamera == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseCamera : CanUseCamera;
            CanUseDoorlog = CanUseDoorlog == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseDoorlog : CanUseDoorlog;
            CanUseBinoculars = CanUseBinoculars == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseBinoculars : CanUseBinoculars;
            CanRepairSabotage = CanRepairSabotage == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseBinoculars : CanRepairSabotage;
            CanUseVentMoving = CanUseVentMoving == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseVentMoving : CanUseVentMoving;
            HasTask = HasTask == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).HasTask : HasTask;

        }
        public void ButtonReset()
        {
            ActionBool(FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton, (bool)CanUseVent);
            ActionBool(FastDestroyableSingleton<HudManager>.Instance.KillButton, (bool)HasKillButton);
        }
        protected void ActionBool(ActionButton button, bool show_hide)
        {
            if (show_hide || DataBase.AllPlayerRoles[PlayerId].Any(x => x.HasKillButton))
            {
                //button.enabled = true;
                //button.gameObject.SetActive(true);
                //button.canInteract = true;
                button.canInteract = true;
                button.Show();
            }
            else
            {
                button.canInteract = false;
                button.enabled = false;
                button.Hide();
            }
        }
        public void VentUpdate()
        {
            if ((bool)CanUseVent)
            {
                //Vent.currentVent?.SetButtons(true);
                if (Input.GetKeyDown(KeyCode.V) || KeyboardJoystick.player.GetButtonDown(50))
                {
                    HudManager.Instance.ImpostorVentButton.DoClick();
                }
            }
        }
        private static Vent SetTargetVent(List<Vent> untarget = null, bool forceout = false)
        {
            return VentPatch.SetTargetVent(untargetablePlayers: untarget, forceout: forceout);
        }
        public bool Dead = false;
        public bool Exiled = false;

        public virtual void OptionCreate() { }
        public virtual void HudManagerStart(HudManager hudManager) { }

        public virtual void MeetingUpdate(MeetingHud meeting) { }
        public virtual void BeforeMeetingStart(MeetingHud meeting) { }
        public virtual void MeetingStart(MeetingHud meeting) { }
        public virtual void CheckForEndVoting(MeetingHud meeting, ref Dictionary<byte, int> dictionary) { }
        public virtual void VotingResultChange(MeetingHud meeting,ref List<MeetingHud.VoterState> states) { return; }
        public virtual void VotingResultChangePost(MeetingHud meeting, ref List<MeetingHud.VoterState> states) { return ; }
        public virtual void Killed() { }
        public virtual void WasKilled() { }
        public virtual void Die() { }
        public virtual void Update() { }
        public virtual void APUpdate() { }
        public string ColoredRoleName => ColoredText(Color, Translation.GetString("role." + Role.ToString() + ".name"));
        public string RoleName => Translation.GetString("role." + Role.ToString() + ".name");

        public string ColoredIntro => ColoredText(Color, Translation.GetString("intro.cosmetic", [Translation.GetString("role." + Role.ToString() + ".intro")]));
        public string RoleDescription()
        {
            return $"{Translation.GetString("canvisibleteam", ["<b>" + RoleData.GetCustomTeamFromTeam(team).ColoredTeamName + "</b>"])}\n{Description()}";
        }
        public string Description()
        {
            return $"{Translation.GetString($"role.{Role}.description")}\n";
        }


        /// <summary>
        /// プレイヤーid入れて初期化
        /// </summary>
        /// <param name="playerId">PlayerControl pc.playerId</param>
        public void ReSet(int playerId)
        {
            PlayerId = playerId;
            PlayerControl = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId);
            PlayerName = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).name.Replace("<color=.*>", string.Empty).Replace("</color>", string.Empty); ;
            CustomTeam = RoleData.GetCustomTeamFromTeam(team);
            Init();
        }
        [HarmonyPatch(typeof(ActionButton), nameof(ActionButton.SetEnabled))]
        private static class MeetingEndPlayerStart
        {
            static void Postfix(ActionButton __instance)
            {
                //if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
                //{

                //    if (DataBase.AllPlayerRoles != null && DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
                //    {
                //        DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].ButtonReset();
                //    }
                //}
                //else
                //{
                //}
                if (PlayerControl.LocalPlayer != null)
                {

                    if (DataBase.AllPlayerRoles != null && DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
                    {
                        DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].ButtonReset();
                    }
                }
                else
                {

                    ActionButton button = DestroyableSingleton<KillButton>.Instance;
                    button.canInteract = false;
                    button.enabled = false;
                    button.Hide();
                    button = DestroyableSingleton<VentButton>.Instance;
                    button.canInteract = false;
                    button.enabled = false;
                    button.Hide();
                }

            }
        }
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
        private static class PlayerControlExiledPatch
        {
            static void Postfix(PlayerControl __instance)
            {

                DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.CustomTeam.WasExiled());
                DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Exiled = true);
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
        private static class PlayerControlDiePatch
        {
            static void Postfix(PlayerControl __instance)
            {

                DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Die());
                DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => x.Dead = true);
                DataBase.AllPlayerRoles[__instance.PlayerId].Do(x => Logger.Info(x.PlayerId.ToString()));
                Logger.Info(__instance.PlayerId + "_" + __instance.Data.PlayerName);
            }
        }
    }
    [HarmonyPatch]
    public static class HudManagerGame
    {
        public static bool IsGameStarting = false;
        [HarmonyPatch(typeof(HudManager))]
        [HarmonyPatch(nameof(HudManager.OnGameStart)), HarmonyPostfix]
        private static void ButtonCreate(HudManager __instance)
        {

            //IsGameStarting = true;

            ButtonCooldownEnabled = false;
            ButtonCooldown = 10f;
            DataBase.buttons.Clear();
            if (PlayerControl.LocalPlayer?.PlayerId == null) return;
            if (DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId) || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {
                //var k = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Select(x => x.Role.ToString()).ToArray();
                //Logger.Info(string.Join(",", k));

                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.HudManagerStart(__instance));
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.ButtonReset());
                DataBase.ButtonsPositionSetter();
            }
        }
        public static float ButtonCooldown;
        public static bool ButtonCooldownEnabled;
        [HarmonyPatch(typeof(HudManager))]
        [HarmonyPatch(nameof(HudManager.Update)), HarmonyPostfix]
        private static void Update()
        {
            //Logger.Message($"{IsGameStarting}");
            if (!IsGameStarting) return;
            if (PlayerControl.LocalPlayer?.PlayerId == null) return;

            DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].Update();
            DataBase.AllPlayerRoles.Do(y => y.Value.Do(x => x.APUpdate()));
            DataBase.AllPlayerRoles.Do(y => y.Value.Do(x => x.VentUpdate()));
        }
        [HarmonyPatch(typeof(ShipStatus),nameof(ShipStatus.Start)), HarmonyPostfix]
        private static void StartGame()
        {
            IsGameStarting = true;
            Logger.Message("shipstatus", "start");

        }
    }

}