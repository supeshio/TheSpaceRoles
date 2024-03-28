using HarmonyLib;
using UnityEngine;

namespace TheSpaceRoles
{
    public class CustomColor
    {
        public string colorName;
        public Color mainColor;
        public Color shadowColor;
    }
    [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
    public static class CustomColors
    {
        public static void Postfix(PlayerTab __instance)
        {

        }
    }
}