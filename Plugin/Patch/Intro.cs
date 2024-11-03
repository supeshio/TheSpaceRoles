using HarmonyLib;
using System;
using System.Linq;
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
                __instance.TeamTitle.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].CustomTeam.ColoredTeamName;

            })));
        }
        public static Color TeamColor() => DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].CustomTeam.Color;

    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    public static class IntroShowRole
    {
        public static void Prefix(IntroCutscene __instance)
        {
            __instance.RoleBlurbText.color = RoleColor();
            __instance.RoleBlurbText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].ColoredIntro;
            __instance.RoleText.color = RoleColor();
            __instance.RoleText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].ColoredRoleName;
            __instance.YouAreText.color = RoleColor();
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {
                __instance.RoleBlurbText.color = RoleColor();
                __instance.RoleBlurbText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].ColoredIntro;
                __instance.RoleText.color = RoleColor();
                __instance.RoleText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].ColoredRoleName;
                __instance.YouAreText.color = RoleColor();
            })));
        }
        public static void Postfix(IntroCutscene __instance)
        {

        }
        public static Color RoleColor()
        {
            return DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Color;

        }

    }
}
