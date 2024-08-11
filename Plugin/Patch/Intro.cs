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

            Logger.Info(DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Count() + " te");
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {

                __instance.BackgroundBar.material.color = TeamColor();
                __instance.TeamTitle.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].CustomTeam.ColoredTeamName;

            })));
        }
        public static Color TeamColor() => DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].CustomTeam.Color;

    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    public static class IntroShowRole
    {
        public static void Prefix(IntroCutscene __instance)
        {
            __instance.RoleBlurbText.color = RoleColor();
            __instance.RoleBlurbText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].ColoredIntro;
            __instance.RoleText.color = RoleColor();
            __instance.RoleText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].ColoredRoleName;
            __instance.YouAreText.color = RoleColor();
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {
                __instance.RoleBlurbText.color = RoleColor();
                __instance.RoleBlurbText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].ColoredIntro;
                __instance.RoleText.color = RoleColor();
                __instance.RoleText.text = DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].ColoredRoleName;
                __instance.YouAreText.color = RoleColor();
            })));
        }
        public static void Postfix(IntroCutscene __instance)
        {

        }
        public static Color RoleColor()
        {
            Logger.Info(DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Count() + " da");
            return DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].Color;

        }

    }
}
