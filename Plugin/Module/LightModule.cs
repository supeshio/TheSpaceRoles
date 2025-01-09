using AmongUs.Data.Legacy;
using AmongUs.GameOptions;
using BepInEx.Configuration;
using Cpp2IL.Core.Extensions;
using HarmonyLib;
using Hazel;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static HarmonyLib.InlineSignature;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    [Harmony]
    public static class ShipStatusPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] NetworkedPlayerInfo player)
        {
            float ImpostorLightMod = GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
            float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;

            ISystemType systemType = __instance.Systems.ContainsKey(SystemTypes.Electrical) ? __instance.Systems[SystemTypes.Electrical] : null;
            if (systemType == null) return true;
            SwitchSystem switchSystem = systemType.TryCast<SwitchSystem>();
            if (switchSystem == null) return true;

            float num = (float)switchSystem.Value / 255f;
            PlayerControl pc = Helper.GetPlayerById(player.PlayerId) ?? null;
            if (player == null || player.IsDead) // IsDead
                __result = __instance.MaxLightRadius;
            else
                __result = pc.GetCustomRole().GetLightMod(__instance,num);
            return false;
        }

    }
}