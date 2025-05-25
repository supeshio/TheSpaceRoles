using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    [Harmony]
    public static class ShipStatusPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
        public static bool Prefix(ref float __result, ShipStatus __instance, [HarmonyArgument(0)] NetworkedPlayerInfo player)
        {
            LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;
            if (Helper.GetPlayerById(player.PlayerId) == null)
            {
                return true;
            }
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
            {
                Dictionary<ChangeLightReason, float> lights = [];
                foreach (var v in DataBase.AllPlayerData.Values.Select(x => x.CustomRole))
                {
                    if (pc == null) continue;
                    var k = v.GetOtherLight(pc, ShipStatus.Instance, num);
                    if (k.Item2 < 0)
                    {
                        continue;
                    }
                    else
                    {
                        lights.Add(k.Item1, k.Item2);
                    }
                }
                Tuple<ChangeLightReason, float> light;
                try
                {

                    light = pc.GetCustomRole().GetLightMod(__instance, num);
                }
                catch
                {

                    light = Tuple.Create(ChangeLightReason.None, -1f);
                }
                lights.Add(light.Item1, light.Item2);

                __result = lights.MaxBy(x => x.Key).Value;
            }

            return false;
        }

    }
    public enum ChangeLightReason
    {
        None = 0,
        Impostor,
        Crewmate,
        Other,
        LightDown,
        TricksterLightdown,
        LighterLight,
    }
}