using AmongUs.GameOptions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(Vent), nameof(Vent.SetOutline))]
    class VentSetOutlinePatch
    {
        static void Postfix(Vent __instance)
        {
            // Vent outline set role color
            if ((bool)PlayerControl.LocalPlayer.GetCustomRole().CanUseVent)
            {

                var color = PlayerControl.LocalPlayer.GetCustomRole().Color;
                string[] outlines = new[] { "_OutlineColor", "_AddColor" };
                foreach (var name in outlines)
                    __instance.myRend.material.SetColor(name, color);
            }
        }
    }
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] NetworkedPlayerInfo pinf, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            float num = float.MaxValue;
            PlayerControl pc = pinf.Object;
            couldUse = Helper.GetCustomRole(pc.PlayerId).CanUseVent == true;
            PlayerControl @object = pc;

            if (@object == null)
            {
                __result = 0f;
                canUse = couldUse = true;
                return true;
            }
            if (@object.inVent && Vent.currentVent != null && __instance != null)
            {
                if (Vent.currentVent.Id == __instance.Id)
                {
                    __result = 0f;
                    canUse = couldUse = true;
                    return false;
                }

            }
            var usableDistance = __instance.UsableDistance;
            bool roleCouldUse = Helper.GetCustomRole(pc.PlayerId).CanUseVent ?? Helper.GetCustomRole(pc.PlayerId).CustomTeam.CanUseVent;
            couldUse = (@object.inVent || roleCouldUse) && !pc.Data.IsDead && (@object.CanMove || @object.inVent);
            canUse = couldUse;
            if (canUse)
            {
                Vector2 truePosition = @object.GetTruePosition();
                Vector3 position = __instance.transform.position;
                num = Vector2.Distance(truePosition, position);

                canUse &= num <= usableDistance && !PhysicsHelpers.AnythingBetween(truePosition, position, Constants.ShipOnlyMask, false);
            }
            __result = num;
            //Vent.currentVent = VentPatch.SetTargetVent();
            return false;
        }
    }
    [HarmonyPatch]
    public static class VentUsePatch
    {
        [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
        [HarmonyPrefix]
        public static bool VentUse(Vent __instance)
        {
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out bool canUse, out bool couldUse);
            bool canMoveInVents = !PlayerControl.LocalPlayer.IsRole(Roles.MadMate);
            if (!canUse) return false; // No need to execute the native method as using is disallowed anyways

            bool isEnter = !PlayerControl.LocalPlayer.inVent;

            if (isEnter)
            {
                PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(__instance.Id);
            }
            else
            {
                PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(__instance.Id);
            }
            __instance.SetButtons(isEnter && canMoveInVents);

            return false;

        }
    }
    public static class VentPatch
    {
        public static Vent SetTargetVent(List<Vent> untargetablePlayers = null, PlayerControl targetingPlayer = null, bool forceout = false)
        {
            Vent result = null;
            float num = GameOptionsData.KillDistances[Mathf.Clamp(GameManager.Instance.LogicOptions.currentGameOptions.GetInt(Int32OptionNames.KillDistance), 0, 2)];
            if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
            if (targetingPlayer.Data.IsDead || targetingPlayer.inVent) return result;

            if (untargetablePlayers == null)
            {
                untargetablePlayers = new();
            }

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            var allPlayers = ShipStatus.Instance.AllVents;
            for (int i = 0; i < allPlayers.Count; i++)
            {
                Vent ventInfo = allPlayers[i];
                if (untargetablePlayers.Any(x => x == ventInfo))
                {
                    continue;
                }
                if (ventInfo)
                {
                    Vector2 vector = new Vector2(ventInfo.transform.position.x, ventInfo.transform.position.y) - truePosition;
                    float magnitude = Vector2.Distance(ventInfo.transform.position, truePosition);
                    if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                    {
                        result = ventInfo;
                        num = magnitude;
                    }
                }
            }
            return result;
        }
    }
}
