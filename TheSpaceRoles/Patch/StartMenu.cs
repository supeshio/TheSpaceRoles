using TMPro;
using UnityEngine;

namespace TSR.Patch
{
    [SmartPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionShowerPatch
    {
        private static void Prefix(VersionShower __instance)
        {
        }

        private static void Postfix(VersionShower __instance)
        {
            TextMeshPro text = new GameObject("VersionText").AddComponent<TextMeshPro>();
            text.text = $"v{TSR.version}";
            text.color = TSR.color;
            text.fontSize = 3;
            text.alignment = TextAlignmentOptions.Right;
            text.enableWordWrapping = false;
            text.transform.SetParent(__instance.transform);
            text.transform.localPosition = new Vector3(0, 1.0f, -1f);
            text.transform.localScale = Vector3.one;
        }
    }

    //[SmartPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    //public static class ShowTSR
    //{
    //    public static void Postfix(PingTracker __instance)
    //    {
    //        __instance.text.text += $"\n{TSR.c_name_v}";
    //        __instance.text.alignment = TextAlignmentOptions.Bottom;
    //    }
    //}
    [SmartPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStart
    {
        public static void Postfix(HudManager __instance)
        {
            GameStartManager.Instance.HostInfoPanel.playerName.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Normal;

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
            TSRText.gameObject.layer = Helper.UILayer;//UILayer
            TSRText.enabled = true;
            TSRText.gameObject.active = true;
            TSRText.material = DestroyableSingleton<PingTracker>.Instance.text.material;
            TSRText.font = DestroyableSingleton<PingTracker>.Instance.text.font;
            TSRText.outlineColor = Color.black;
            TSRText.outlineWidth = 0.1f;
        }
    }

    //[SmartPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    //public static class MainMenuStartPatch
    //{
    //    public static void Prefix()
    //    {
    //        Logger.Info("Opening MainMenu...");
    //        var spriteredrer = new GameObject("TSRlogo").AddComponent<SpriteRenderer>();
    //        spriteredrer.sprite = Sprites.GetSpriteFromResources("TSRLogo.png", 180f);
    //        spriteredrer.transform.position = new Vector3(2f, 0f, 0);
    //        spriteredrer.transform.localScale = Vector3.one;
    //        spriteredrer.enabled = true;
    //        spriteredrer.gameObject.SetActive(true);
    //    }
    //}
}