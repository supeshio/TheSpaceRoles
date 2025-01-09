using Epic.OnlineServices.Lobby;
using HarmonyLib;
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
        public Teams Team = Teams.None;
        public CustomTeam CustomTeam;
        public Roles Role;
        public Color Color = new(0, 0, 0);
        public Color? MapBackColor = null;
        public bool? HasKillButton = null;
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
        public bool? ImpostorMap = null;
        public bool? AdminMap = null;
        public bool? ShowingMapAllowedToMove = null;
        public bool? ShowingAdminIncludeDeadBodies = null;
        public bool canAssign = true;
        public List<CustomOption> Options = [];
        public void Init()
        {
            OptionCreate();
            HasKillButton = HasKillButton == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).HasKillButton : HasKillButton;
            CanUseVent = CanUseVent == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseVent : CanUseVent;
            CanUseAdmin = CanUseAdmin == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseAdmin : CanUseAdmin;
            CanUseBinoculars = CanUseBinoculars == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseBinoculars : CanUseBinoculars;
            CanUseCamera = CanUseCamera == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseCamera : CanUseCamera;
            CanUseDoorlog = CanUseDoorlog == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseDoorlog : CanUseDoorlog;
            CanUseBinoculars = CanUseBinoculars == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseBinoculars : CanUseBinoculars;
            CanRepairSabotage = CanRepairSabotage == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseBinoculars : CanRepairSabotage;
            CanUseVentMoving = CanUseVentMoving == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).CanUseVentMoving : CanUseVentMoving;
            HasTask = HasTask == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).HasTask : HasTask;
            ImpostorMap = ImpostorMap == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).ImpostorMap : ImpostorMap;
            AdminMap = AdminMap == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).AdminMap : AdminMap;
            ShowingMapAllowedToMove = ShowingMapAllowedToMove == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).ShowingMapAllowedToMove : ShowingMapAllowedToMove;
            ShowingAdminIncludeDeadBodies = ShowingAdminIncludeDeadBodies == null ? RoleData.GetCustomTeamFromTeam(CustomTeam.Team).ShowingAdminIncludeDeadBodies : ShowingAdminIncludeDeadBodies;
            RoleTextManager.TextChange(PlayerId);
        }
        public void ButtonReset()
        {
            ActionBool(FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton, (bool)CanUseVent);
            ActionBool(FastDestroyableSingleton<HudManager>.Instance.KillButton, (bool)HasKillButton);
            FastDestroyableSingleton<HudManager>.Instance.ImpostorVentButton.transform.SetSiblingIndex(HudManager.Instance.AbilityButton.transform.GetSiblingIndex());
        }
        protected void ActionBool(ActionButton button, bool show_hide)
        {
            if (show_hide || (bool)Helper.GetCustomRole(PlayerId).HasKillButton)
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
        public virtual void ShowMap(ref MapBehaviour mapBehaviour) { }
        public virtual void MeetingUpdate(MeetingHud meeting) { }
        public virtual void BeforeMeetingStart(MeetingHud meeting) { }
        public virtual void MeetingStart(MeetingHud meeting) { }
        public virtual void AfterMeetingEnd() { }
        public virtual void CheckForEndVoting(MeetingHud meeting, ref Dictionary<byte, int> dictionary) { }
        public virtual void VotingResultChange(MeetingHud meeting, ref List<MeetingHud.VoterState> states) { return; }
        public virtual void VotingResultChangePost(MeetingHud meeting, ref List<MeetingHud.VoterState> states) { return; }
        public virtual void Killed() { }
        public virtual void WasKilled() { }
        public virtual void Die() { }
        public virtual void APDie(PlayerControl pc) { }
        public virtual void Update() { }
        public virtual void APUpdate() { }
        public virtual void Murder(PlayerControl pc,PlayerControl target) { }
        public virtual float GetLightMod(ShipStatus shipStatus,float num)
        {
            return CustomTeam.GetLightMod(shipStatus,num);

        }
        public string ColoredRoleName => ColoredText(Color, Translation.GetString("role." + Role.ToString() + ".name"));
        public string RoleName => Translation.GetString("role." + Role.ToString() + ".name");

        public string ColoredIntro => ColoredText(Color, Translation.GetString("intro.cosmetic", [Translation.GetString("role." + Role.ToString() + ".intro")]));
        public string RoleDescription()
        {
            return $"{Translation.GetString("canvisibleteam", ["<b>" + RoleData.GetCustomTeamFromTeam(Team).ColoredTeamName + "</b>"])}\n{Description()}";
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
            CustomTeam = RoleData.GetCustomTeamFromTeam(Team);
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

                    if (DataBase.AllPlayerData != null && DataBase.AllPlayerData.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
                    {
                        Helper.GetCustomRole(PlayerControl.LocalPlayer).ButtonReset();
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

                Helper.GetCustomRole(__instance).CustomTeam.WasExiled();
                Helper.GetCustomRole(__instance).Exiled = true;
            }
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Die))]
        private static class PlayerControlDiePatch
        {
            static void Postfix(PlayerControl __instance)
            {

                Helper.GetCustomRole(__instance).Die();
                DataBase.GetCustomRoles().Do(x => x.APDie(__instance));
                Helper.GetCustomRole(__instance).Dead = true;
                Helper.GetCustomRole(__instance).PlayerId.ToString();
                Logger.Info(__instance.PlayerId + "_" + __instance.Data.PlayerName);
            }
        }
        [HarmonyPatch(typeof(PlayerControl),nameof(PlayerControl.MurderPlayer))]
        private static class MurderPlayerPatch
        {
            static void Postfix(PlayerControl __instance,[HarmonyArgument(0)]PlayerControl target)
            {
                Logger.Info($"{__instance.Data.PlayerName} - {target.Data.PlayerName}","Murder");
                DataBase.AllPlayerData.Do(x=>x.Value.CustomRole.Murder(__instance, target));
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
            DataBase.ResetButtons();
            ButtonCooldownEnabled = false;
            ButtonCooldown = 10f;
            DataBase.buttons.Clear();
            if (PlayerControl.LocalPlayer?.PlayerId == null) return;
            if (DataBase.AllPlayerData.ContainsKey(PlayerControl.LocalPlayer.PlayerId))
            {
                //var k = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Select(x => x.Role.ToString()).ToArray();
                //Logger.Info(string.Join(",", k));

                Helper.GetCustomRole(PlayerControl.LocalPlayer).HudManagerStart(__instance);
                Helper.GetCustomRole(PlayerControl.LocalPlayer).ButtonReset();
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
            if (DataBase.AllPlayerData == null || !DataBase.AllPlayerData.ContainsKey(PlayerControl.LocalPlayer.PlayerId)) return;

            Helper.GetCustomRole(PlayerControl.LocalPlayer).Update();
            DataBase.AllPlayerData.Do(y => y.Value.CustomRole.APUpdate());
            DataBase.AllPlayerData.Do(y => y.Value.CustomRole.VentUpdate());
        }
        [HarmonyPatch(typeof(ExileController))]
        [HarmonyPatch(nameof(ExileController.ReEnableGameplay)), HarmonyPostfix]
        private static void StartPC(ExileController __instance)
        {
            if (!IsGameStarting) return;
            if (PlayerControl.LocalPlayer?.PlayerId == null) return;
            if (DataBase.AllPlayerData == null || !DataBase.AllPlayerData.ContainsKey(PlayerControl.LocalPlayer.PlayerId)) return;
            Helper.GetCustomRole(PlayerControl.LocalPlayer).AfterMeetingEnd();
        }
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start)), HarmonyPostfix]
        private static void StartGame()
        {
            CustomOptionsHolder.CreateCustomOptions();
            IsGameStarting = true;
            Logger.Message("shipstatus", "start");

        }
        [HarmonyPatch(typeof(MapBehaviour), nameof(MapBehaviour.Show))]
        private static class MapShow
        {
            static bool Prefix(MapBehaviour __instance, [HarmonyArgument(0)] MapOptions mapOptions)
            {
                var map = __instance;
                bool re = false;
                var f = Helper.GetCustomRole(PlayerControl.LocalPlayer);


                f.ShowMap(ref map);
                if (mapOptions.Mode == MapOptions.Modes.Normal)
                {
                    return true;
                }


                if (mapOptions.Mode == MapOptions.Modes.CountOverlay)
                {
                    //re = true;
                    //map.ShowCountOverlay(false, true, true);
                    return false;
                }



                if ((bool)f.ImpostorMap)
                {
                    map.ShowSabotageMap();
                    re = true;
                    if ((bool)f.AdminMap)
                    {

                        map.countOverlay.enabled = true;
                        map.countOverlay.gameObject.SetActive(true);
                        map.countOverlay.SetOptions((bool)f.ShowingMapAllowedToMove, (bool)f.ShowingAdminIncludeDeadBodies);
                        map.countOverlayAllowsMovement = true;
                        map.taskOverlay.Hide();
                        map.countOverlay.showLivePlayerPosition = true;
                        map.countOverlay.transform.SetLocalZ(-10f);
                        //map.infectedOverlay.allButtons.Do(x => x.transform.localPosition += new Vector3(0, -0.1f, 0));
                        map.infectedOverlay.allButtons.Do(x => x.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f));
                        map.ColorControl.baseColor = Palette.ImpostorRed;
                        map.countOverlay.BackgroundColor.baseColor = Palette.ImpostorRed;
                    }
                }
                else
                {
                    map.ShowNormalMap();
                    re = true;
                    if ((bool)f.AdminMap)
                    {
                        map.countOverlay.BackgroundColor.baseColor = invisible;
                        map.countOverlay.enabled = true;
                        map.countOverlayAllowsMovement = (bool)f.ShowingMapAllowedToMove;
                        map.countOverlay.includeDeadBodies = true;
                        map.countOverlay.showLivePlayerPosition = true;
                    }
                }
                if (f.MapBackColor != null)
                {
                    map.ColorControl.baseColor = (Color)f.MapBackColor;
                    map.countOverlay.BackgroundColor.baseColor = (Color)f.MapBackColor;
                }
                //if ((bool)f.AdminMap)
                //{
                //    map.ShowCountOverlay((bool)f.ShowingMapAllowedToMove, true, (bool)f.ShowingMapAllowedToMove);
                //    map.countOverlay.enabled = true;
                //    map.countOverlayAllowsMovement = (bool)f.ShowingMapAllowedToMove;
                //    map.countOverlay.includeDeadBodies = true;
                //    map.countOverlay.showLivePlayerPosition = true;

                //}
                return !re;

            }
        }
    }

}