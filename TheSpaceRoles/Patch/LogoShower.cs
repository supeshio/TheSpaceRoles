using HarmonyLib;
using UnityEngine;

namespace TSR.Patch
{
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class MainMenuStartPatch
    {
        public static void Prefix()
        {
            Logger.Info("Opening MainMenu...");
            var spriteredrer = new GameObject("TSRlogo").AddComponent<SpriteRenderer>();
            spriteredrer.sprite = Assets.AssetLoader.Sprites["TSRLogo"];
            Logger.Fatal(Assets.AssetLoader.Sprites["TSRLogo"].bounds.size.x.ToString());
            spriteredrer.transform.position = new Vector3(2f, 0f, 0);
            spriteredrer.transform.localScale = Vector3.one * 0.5f;
            spriteredrer.enabled = true;
            spriteredrer.gameObject.SetActive(true);
        }
    }
}