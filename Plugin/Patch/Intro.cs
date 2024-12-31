using HarmonyLib;
using System;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowTeam))]
    public static class IntroShowTeam
    {
        public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.IEnumerator __result)
        {

            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {

                __instance.BackgroundBar.material.color = TeamColor();
                __instance.TeamTitle.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).CustomTeam.ColoredTeamName;

            })));
        }
        public static Color TeamColor() => Helper.GetCustomRole(PlayerControl.LocalPlayer).CustomTeam.Color;

    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    public static class IntroShowRole
    {
        public static void Prefix(IntroCutscene __instance)
        {
            __instance.RoleBlurbText.color = RoleColor();
            __instance.RoleBlurbText.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).ColoredIntro;
            __instance.RoleText.color = RoleColor();
            __instance.RoleText.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).ColoredRoleName;
            __instance.YouAreText.color = RoleColor();
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {
                __instance.RoleBlurbText.color = RoleColor();
                __instance.RoleBlurbText.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).ColoredIntro;
                __instance.RoleText.color = RoleColor();
                __instance.RoleText.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).ColoredRoleName;
                __instance.YouAreText.color = RoleColor();
            })));
        }
        public static void Postfix(IntroCutscene __instance)
        {

        }
        public static Color RoleColor()
        {
            return Helper.GetCustomRole(PlayerControl.LocalPlayer).Color;

        }

    }
}
