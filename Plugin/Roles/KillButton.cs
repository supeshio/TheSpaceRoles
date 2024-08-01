using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(KillButton))]
    public static class KillButtons
    {

        private static readonly int Desat = Shader.PropertyToID("_Desat");
        [HarmonyPatch(nameof(KillButton.SetTarget)), HarmonyPostfix]
        public static void SetTargetPlayer(KillButton __instance)
        {
            if (__instance == null) { return; }
            if (__instance.gameObject.active == false) return;
            var player = KillButtonSetTarget(
                GameOptionsManager.Instance.currentNormalGameOptions.KillDistance,
                GetLink.ColorFromTeams(Teams.Impostor), notIncludeTeamIds: [Teams.Impostor]);
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

            //Logger.Info(string.Join(",", DataBase.AllPlayerRoles.Where(x => x.Value.Any(y => y.Role == Roles.Mini)).Select(x => x.Key).ToArray()));
        }

        public static int KillButtonSetTarget(float targetdistance, Color color, Teams[] notIncludeTeamIds = null, int[] notIncludeIds = null, int target = -1, bool canBeTargetInVentPlayer = false, int[] IncludeIds = null)
        {
            var nIids = notIncludeIds?.ToList() ?? [];
            nIids.AddRange(DataBase.AllPlayerRoles.Where(x => x.Value.Any(y => y.Role == Roles.Mini)).Select(x => x.Key).ToList());


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

            return PlayerControlButtonControls.SetTarget(targetdistance, color, notIncludeTeamIds, [.. nIids], target, canBeTargetInVentPlayer);
        }/*
        [HarmonyPatch(nameof(KillButton.CheckClick)), HarmonyPostfix]
        public static void Click(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            CheckedMurderPlayer.RpcMurder(__instance, target, DeathReason.ImpostorKill);
        }*/
    }
}
