using HarmonyLib;
using TSR.Game;
using UnityEngine;

namespace TSR
{
    public static class IntroCutscenePatch
    {
        [SmartPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowTeam)), HarmonyPostfix]
        public static void ShowTeam(IntroCutscene __instance)
        {
            __instance.TeamTitle.text = FPlayerControl.LocalPlayer.FTeam.TeamName;
            __instance.TeamTitle.color = FPlayerControl.LocalPlayer.FTeam.TeamColor();
            __instance.BackgroundBar.GetComponent<SpriteRenderer>().color = FPlayerControl.LocalPlayer.FTeam.TeamColor();
        }

        [SmartPatch(typeof(IntroCutscene), nameof(IntroCutscene.ShowRole)), HarmonyPostfix]
        public static void ShowRole(IntroCutscene __instance)
        {
            __instance.RoleBlurbText.text = FPlayerControl.LocalPlayer.FRole.RoleName;
            __instance.RoleBlurbText.color = FPlayerControl.LocalPlayer.FTeam.TeamColor();
            __instance.RoleText.text = FPlayerControl.LocalPlayer.FRole.RoleIntro;
            __instance.RoleText.color = FPlayerControl.LocalPlayer.FTeam.TeamColor();
            __instance.BackgroundBar.GetComponent<SpriteRenderer>().color = FPlayerControl.LocalPlayer.FTeam.TeamColor();
        }

        //[HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41),nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
        //public static void Shlp(IntroCutscene._ShowRole_d__41 __instance)
        //{
        //    __instance.__4__this
        //}
    }
}