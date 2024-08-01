using HarmonyLib;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class StartMenu
    {

        public static void Prefix(VersionShower __instance)
        {
        }
        public static void Postfix(VersionShower __instance)
        {
            TextMeshPro AddtionalText = new GameObject("text").AddComponent<TextMeshPro>();
            AddtionalText.text = TSR.c_name;
            AddtionalText.fontSize = 2;
            AddtionalText.alignment = TextAlignmentOptions.Right;
            AddtionalText.enableWordWrapping = false;
            AddtionalText.transform.SetParent(__instance.transform);
            AddtionalText.transform.localPosition = new Vector3(0, 0, 0);
            AddtionalText.transform.localScale = Vector3.one;


            //spriteredrer.transform.@obj_parent = __instance.transform;

        }
    }
    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class ShowTSR
    {
        public static void Postfix(PingTracker __instance)
        {

            __instance.text.text += $"\n{TSR.c_name}";
        }
    }
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStart
    {
        public static void Postfix(HudManager __instance)
        {
            Data.textMaterial = __instance.Chat.quickChatMenu.timer.text.fontMaterial;
        }
    }
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class MainMenuStartPatch
    {
        public static void Prefix()
        {

            Logger.Info("mainmenu");
            var spriteredrer = new GameObject("TSRlogo").AddComponent<SpriteRenderer>();
            spriteredrer.sprite = Sprites.GetSpriteFromResources("TSRLogo.png", 200f);
            spriteredrer.transform.position = new Vector3(2f, 0.6f, 0);
            spriteredrer.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            spriteredrer.enabled = true;
            spriteredrer.gameObject.SetActive(true);
        }
    }
}
