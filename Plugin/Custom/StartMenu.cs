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
            AddtionalText.text = TSR.c_name_v;
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

            __instance.text.text += $"\n{TSR.c_name_v}";
            __instance.text.alignment = TextAlignmentOptions.Bottom;
        }
    }
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStart
    {
        public static void Postfix(HudManager __instance)
        {
            Data.textMaterial = __instance.Chat.quickChatMenu.timer.text.fontMaterial;
            CustomOptionsHolder.CreateCustomOptions();
            //var spriteredrer = new GameObject("TSRlogo").AddComponent<SpriteRenderer>();
            //spriteredrer.sprite = Sprites.GetSpriteFromResources("TSRLogo.png", 400f);
            //spriteredrer.transform.SetParent(__instance.transform);
            //spriteredrer.transform.position = new Vector3(1.6f, 2.4f, 0);
            //spriteredrer.transform.localScale = Vector3.one;
            //spriteredrer.enabled = true;
            //spriteredrer.gameObject.SetActive(true);
            //spriteredrer.gameObject.layer = 5;
            //var sp = new GameObject("TSRlogoBack").AddComponent<SpriteRenderer>();
            //sp.sprite = Sprites.GetSpriteFromResources("ui.Logo_Back.png", 340f);
            //sp.color = Helper.ColorFromColorcode("#ffffff22");
            //sp.transform.SetParent(spriteredrer.transform);
            //sp.transform.localPosition = new(0,0,1);
            //spriteredrer.transform.localScale = Vector3.one;
            //sp.enabled = true;
            //sp.gameObject.SetActive(true);
            //sp.gameObject.layer = 5;

            TextMeshPro TSRText = new GameObject("TSR").AddComponent<TextMeshPro>();
            TSRText.text = TSR.c_name_v;
            TSRText.fontSize = 2;
            TSRText.alignment = TextAlignmentOptions.Midline;
            TSRText.enableWordWrapping = false;
            TSRText.transform.SetParent(__instance.transform);
            TSRText.transform.position = new Vector3(1.6f, 2.6f, 0);
            TSRText.transform.localScale = Vector3.one;
            TSRText.gameObject.layer = 5;
            TSRText.enabled = true;
            TSRText.gameObject.active = true;
            TSRText.material = DestroyableSingleton<PingTracker>.Instance.text.material;
            TSRText.font = DestroyableSingleton<PingTracker>.Instance.text.font;
            TSRText.outlineColor = Color.black;
            TSRText.outlineWidth = 0.1f;
        }
    }
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class MainMenuStartPatch
    {
        public static void Prefix()
        {

            Logger.Info("Opening MainMenu...");
            var spriteredrer = new GameObject("TSRlogo").AddComponent<SpriteRenderer>();
            spriteredrer.sprite = Sprites.GetSpriteFromResources("TSRLogo.png", 180f);
            spriteredrer.transform.position = new Vector3(2f, 0f, 0);
            spriteredrer.transform.localScale = Vector3.one;
            spriteredrer.enabled = true;
            spriteredrer.gameObject.SetActive(true);
        }
    }
}
