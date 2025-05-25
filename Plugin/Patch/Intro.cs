using HarmonyLib;
using System;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class IntroShowTeam
    {
        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
        class BeginCrewmatePatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay)
            {

            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay)
            {
                SetupIntroTeam(__instance, ref teamToDisplay);
            }
        }

        [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
        class BeginImpostorPatch
        {
            public static void Prefix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
            {
                //yourTeam = PlayerControl.AllPlayerControls;
            }

            public static void Postfix(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> yourTeam)
            {
                SetupIntroTeam(__instance, ref yourTeam);
            }
        }
        //public static void Prefix(IntroCutscene __instance ,ref Il2CppSystem.Collections.Generic.List<PlayerControl> teamToDisplay )
        //{

        //    //__instance.StartCoroutine(Effects.Lerp(10f, new Action<float>((p) =>
        //    //{

        //    //    __instance.BackgroundBar.material.color = TeamColor();
        //    //    __instance.TeamTitle.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).CustomTeam.ColoredTeamName;
        //    //})));
        //}
        public static void SetupIntroTeam(IntroCutscene __instance, ref Il2CppSystem.Collections.Generic.List<PlayerControl> team)
        {
            Color color = Color.cyan;
            string TeamTitle = __instance.TeamTitle.text;
            string ImpostorText = __instance.ImpostorText.text;


            color = TeamColor();
            try
            {

                TeamTitle = Helper.GetCustomRole(PlayerControl.LocalPlayer).CustomTeam.ColoredTeamName;
                //TeamTitle = new JackalTeam().ColoredTeamName;
            }
            catch (Exception e)
            {
                Logger.Fatal(e.Message, e.Source);
            }

            __instance.BackgroundBar.material.color = __instance.BackgroundBar.material.color = color;
            //__instance.TeamTitle.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).CustomTeam.ColoredTeamName;
            __instance.TeamTitle.text = TeamTitle;

            __instance.ImpostorText.text = ImpostorText;
            Logger.Info(TeamTitle, "SetupTeamIntro");
        }
        public static Color TeamColor()
        {
            var local = PlayerControl.LocalPlayer;
            if (local == null)
            {
                Logger.Fatal("LocalPlayer is null", "TeamColor");
                return Color.white;
            }

            var role = Helper.GetCustomRole(local);
            if (role?.CustomTeam == null)
            {
                Logger.Fatal("CustomRole or CustomTeam is null", "TeamColor");
                return Color.white;
            }

            return role.CustomTeam.Color;
        }

    }


















    [HarmonyPatch(typeof(IntroCutscene._ShowRole_d__41), nameof(IntroCutscene._ShowRole_d__41.MoveNext))]
    public static class IntroShowRole
    {
        public static void Prefix(IntroCutscene._ShowRole_d__41 __instance)
        {

            __instance.__4__this.RoleBlurbText.gameObject.SetActive(false);
            __instance.__4__this.RoleText.gameObject.SetActive(false);
            __instance.__4__this.YouAreText.gameObject.SetActive(false);
        }
        private static int last;
        public static void Postfix(IntroCutscene._ShowRole_d__41 __instance)
        {

            if (__instance.__4__this.GetInstanceID() == last)
                return;
            last = __instance.__4__this.GetInstanceID();
            __instance.__4__this.RoleBlurbText.gameObject.SetActive(true);
            __instance.__4__this.RoleText.gameObject.SetActive(true);
            __instance.__4__this.YouAreText.gameObject.SetActive(true);

            __instance.__4__this.RoleBlurbText.color = RoleColor();
            __instance.__4__this.RoleBlurbText.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).ColoredIntro;
            __instance.__4__this.RoleText.color = RoleColor();
            __instance.__4__this.RoleText.text = Helper.GetCustomRole(PlayerControl.LocalPlayer).ColoredRoleName;
            __instance.__4__this.YouAreText.color = RoleColor();
            __instance.__4__this.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {
            })));
        }
        public static Color RoleColor()
        {
            var local = PlayerControl.LocalPlayer;
            if (local == null)
            {
                Logger.Fatal("LocalPlayer is null", "RoleColor");
                return Color.white;
            }

            var role = Helper.GetCustomRole(local);
            if (role == null)
            {
                Logger.Fatal("CustomRole is null", "RoleColor");
                return Color.white;
            }

            return role.Color;
        }


    }
}
