using HarmonyLib;
using UnityEngine;

namespace TSR.Patch
{
    [SmartPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    public static class MainMenuStartPatch
    {
        public static void Prefix()
        {
            Logger.Info("Opening MainMenu...");
            var spriteRenderer = new GameObject("TSRlogo").AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Assets.AssetLoader.Sprites["TSRLogo"];
            spriteRenderer.transform.position = new Vector3(2f, 0f, 0);
            spriteRenderer.transform.localScale = Vector3.one * 0.5f;
            spriteRenderer.enabled = true;
            spriteRenderer.gameObject.SetActive(true);
        }
    }
}