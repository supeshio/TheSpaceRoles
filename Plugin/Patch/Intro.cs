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

            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) =>
            {

                __instance.TeamTitle.color = TeamColor();
                __instance.TeamTitle.text = Translation.GetString($"team.{DataBase.AllPlayerTeams[PlayerControl.LocalPlayer.PlayerId]}.name");

            })));
        }
        public static void Postfix(IntroCutscene __instance)
        {


        }

        public static Color TeamColor() => GetLink.ColorFromTeams[DataBase.AllPlayerTeams[PlayerControl.LocalPlayer.PlayerId]];
    }
    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole))]
    public static class IntroShowRole
    {
        public static void Prefix(IntroCutscene __instance)
        {
            __instance.StartCoroutine(Effects.Lerp(0.1f, new Action<float>((p) =>
            {

                __instance.RoleBlurbText.color = RoleColor();
                __instance.RoleBlurbText.text = Translation.GetString($"role.{DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].Role}.intro");
                __instance.RoleText.color = RoleColor();
                __instance.RoleText.text = string.Join("×", DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Select(x => Translation.GetString($"role.{x.Role}.name")));
                __instance.YouAreText.color = RoleColor();
            })));
        }
        public static void Postfix(IntroCutscene __instance)
        {

        }
        public static Color RoleColor()
        {
            if (DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].Color == new Color(0, 0, 0))
            {
                return IntroShowTeam.TeamColor();
            }

            return DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId][0].Color;

        }

    }
}
