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
            var color = PlayerControl.LocalPlayer.GetCustomRoles().First(x => x.CanUseVent == true).Color;
            string[] outlines = new[] { "_OutlineColor", "_AddColor" };
            foreach (var name in outlines)
                __instance.myRend.material.SetColor(name, color);
        }
    }
    [HarmonyPatch(typeof(Vent), nameof(Vent.CanUse))]
    public static class VentCanUsePatch
    {
        public static bool Prefix(Vent __instance, ref float __result, [HarmonyArgument(0)] NetworkedPlayerInfo pinf, [HarmonyArgument(1)] out bool canUse, [HarmonyArgument(2)] out bool couldUse)
        {
            PlayerControl pc = pinf.Object;
            couldUse = DataBase.AllPlayerRoles[pc.PlayerId].Any(x => x.CanUseVent == true);
            Vent.currentVent = VentPatch.SetTargetVent();
            canUse = (couldUse && !pinf.IsDead && Vent.currentVent) || pc.inVent;
            //Vent.currentVent = VentPatch.SetTargetVent();
            return false;
        }
    }
    [HarmonyPatch(typeof(Vent), nameof(Vent.Use))]
    public static class VentUsePatch
    {
        public static bool Prefix(Vent __instance)
        {
            __instance.CanUse(PlayerControl.LocalPlayer.Data, out bool canUse, out bool couldUse);
            bool canMoveInVents = PlayerControl.LocalPlayer.GetCustomRoles().All(x => x.CanUseVentMoving == true);
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
            if (ShipStatus.Instance == null) return result;
            if (targetingPlayer == null) targetingPlayer = PlayerControl.LocalPlayer;
            if (targetingPlayer.Data.IsDead || targetingPlayer.inVent) return result;

            if (untargetablePlayers == null)
            {
                untargetablePlayers = new();
            }

            Vector2 truePosition = targetingPlayer.GetTruePosition();
            var allPlayers = ShipStatus.Instance.AllVents;
            //Logger.Info(num.ToString() + $"((({targetingPlayer.Data.IsDead || targetingPlayer.inVent} {allPlayers.Count}");
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
                    float magnitude = vector.magnitude;
                    //Logger.Info(allPlayers[i].name + $"({ventInfo.Id})" + ":" + $"{magnitude <= num}");
                    if (magnitude <= num && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                    {
                        result = ventInfo;
                        num = magnitude;
                    }
                }
            }
            if (result == null)
                Logger.Info("null");
            return result;
        }
    }
}
