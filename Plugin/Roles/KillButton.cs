using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(KillButton))]
    public static class KillButtons
    {

        [HarmonyPatch(nameof(KillButton.DoClick)), HarmonyPrefix]
        public static bool KillPlayer(KillButton __instance)
        {
            if (__instance.canInteract)
            {
                Logger.Message("Kill");
                var player = KillButtonSetTarget(
                    GameOptionsManager.Instance.currentNormalGameOptions.KillDistance,
                    RoleData.GetColorFromTeams(Teams.Impostor), notIncludeTeamIds: [Teams.Impostor]);
                if (!__instance.isCoolingDown && __instance.gameObject.active)
                {
                    CheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, Helper.GetPlayerById(player), DeathReason.ImpostorKill);
                    __instance.ResetCoolDown();
                    return false;
                }

            }
            return true;
        }
        private static readonly int Desat = Shader.PropertyToID("_Desat");
        [HarmonyPatch(typeof(KillButton))]
        [HarmonyPatch(nameof(KillButton.SetTarget)), HarmonyPostfix]
        public static void SetTargetPlayer(KillButton __instance, [HarmonyArgument(0)] ref PlayerControl target)
        {
            if (__instance == null) { return; }
            if (__instance.gameObject.active == false) return;
            var player = KillButtonSetTarget(
                GameOptionsManager.Instance.currentNormalGameOptions.KillDistance,
                RoleData.GetColorFromTeams(Teams.Impostor), notIncludeTeamIds: [Teams.Impostor]);
            if (player != -1)
            {
                __instance.currentTarget = DataBase.AllPlayerControls().First(x => x.PlayerId == player);

                __instance.graphic.material.SetFloat(Desat, 0f);
                __instance.graphic.color = __instance.buttonLabelText.color = Palette.EnabledColor;
            }
            else
            {

                __instance.currentTarget = null;
                __instance.graphic.material.SetFloat(Desat, 1f);
                __instance.graphic.color = __instance.buttonLabelText.color = Palette.DisabledClear;

            }
            if (player == -1)
            {
                return;
            }
            target = Helper.GetPlayerById(player);
            //Logger.Info(string.Join(",", DataBase.AllPlayerRoles.Where(x => x.Value.Any(y => y.Role == Roles.Mini)).Select(x => x.Key).ToArray()));
        }

        public static int KillButtonSetTarget(float targetdistance, Color color, Teams[] notIncludeTeamIds = null, int[] notIncludeIds = null, int target = -1, bool canBeTargetInVentPlayer = false, int[] IncludeIds = null)
        {
            var nIids = notIncludeIds?.ToList() ?? [];
            nIids.AddRange(DataBase.AllPlayerData.Where(x => x.Value.CustomRole.Role == Roles.NiceMini).Select(x => x.Key).ToList());


            if (IncludeIds != null)
            {
                foreach (var item in IncludeIds)
                {
                    if (nIids.Contains(item))
                    {
                        nIids.Remove(item);
                    }
                }

            }

            return CustomButton.SetTarget(targetdistance, color, notIncludeTeamIds, [.. nIids], target, canBeTargetInVentPlayer);
        }/*
        [HarmonyPatch(nameof(KillButton.CheckClick)), HarmonyPostfix]
        public static void Click(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            CheckedMurderPlayer.RpcMurder(__instance, target, DeathReason.ImpostorKill);
        }*/
    }
}
