using HarmonyLib;
using InnerNet;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public void ResetStart()
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
        public virtual void MeetingEnd(MeetingHud meeting) { }
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
            string r = "";
            string f = "<b>" + RoleData.GetCustomTeamFromTeam(team).ColoredTeamName + "</b>";





            r += $"{Translation.GetString("canvisibleteam", [f])}\n";
            r += Description();

            return r;
        }
        public string Description()
        {
            return $"{Translation.GetString($"role.{Role}.description")}\n ";
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
        [HarmonyPatch(typeof(MeetingHud))]
        private static class MeetingHudVote
        {
            [HarmonyPatch(nameof(MeetingHud.CheckForEndVoting)), HarmonyPostfix]
            private static void CheckForVoting(MeetingHud __instance)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.MeetingEnd(__instance));

            }
            [HarmonyPatch(nameof(MeetingHud.Start)), HarmonyPostfix]
            private static void Start(MeetingHud __instance)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.MeetingStart(__instance));

            }
            [HarmonyPatch(nameof(MeetingHud.Update)), HarmonyPostfix]
            private static void Update(MeetingHud __instance)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.MeetingUpdate(__instance));

            }
            [HarmonyPatch(nameof(MeetingHud.CoStartCutscene)), HarmonyPostfix]
            private static void CustScene(MeetingHud __instance)
            {
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.BeforeMeetingStart(__instance));

            }
        }
        [HarmonyPatch(typeof(ActionButton), nameof(ActionButton.SetEnabled))]
        private static class MeetingEndPlayerStart
        {
            static void Postfix(ActionButton __instance)
            {
                if (AmongUsClient.Instance.GameState == InnerNetClient.GameStates.Started)
                {

                    if (DataBase.AllPlayerRoles != null && DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
                    {
                        DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.ResetStart());
                    }
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

    [HarmonyPatch(typeof(HudManager))]
    public static class HudManagerGame
    {
        public static bool OnGameStarted = false;
        public static bool IsGameStarting = false;
        [HarmonyPatch(nameof(HudManager.OnGameStart)), HarmonyPostfix]
        private static void ButtonCreate(HudManager __instance)
        {

            if (!OnGameStarted) return;
            IsGameStarting = true;
            OnGameStarted = false;

            ButtonCooldownEnabled = false;
            ButtonCooldown = 10f;
            DataBase.buttons.Clear();
            if (DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId) || AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {
                //var k = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Select(x => x.Role.ToString()).ToArray();
                //Logger.Info(string.Join(",", k));

                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.HudManagerStart(__instance));
                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.ResetStart());
                DataBase.ButtonsPositionSetter();
            }
        }
        public static float ButtonCooldown;
        public static bool ButtonCooldownEnabled;
        [HarmonyPatch(nameof(HudManager.Update)), HarmonyPostfix]
        private static void Update()
        {
            if (!IsGameStarting) return;
            if (DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {

                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.Update());
                DataBase.AllPlayerRoles.Do(y => y.Value.Do(x => x.APUpdate()));
                DataBase.AllPlayerRoles.Do(y => y.Value.Do(x => x.VentUpdate()));
            }
        }

    }

}