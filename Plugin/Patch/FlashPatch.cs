using System;
using UnityEngine;

namespace TheSpaceRoles
{
    //[HarmonyPatch(typeof(AlertFlash),nameof(AlertFlash.Flash))]
    public static class FlashPatch
    {
        /// <summary>
        /// TORから
        /// フラッシュを焚きます
        /// </summary>
        /// <param name="color_">色</param>
        /// <param name="duration"></param>
        public static void ShowFlash(Color? color_ = null, float duration = 1f)
        {
            Color color = color_ ?? Palette.ImpostorRed;

            if (HudManager.Instance == null || HudManager.Instance.FullScreen == null) return;
            HudManager.Instance.FullScreen.gameObject.SetActive(true);
            HudManager.Instance.FullScreen.enabled = true;
            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) =>
            {
                var renderer = HudManager.Instance.FullScreen;
                //renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(1 - (p * p / duration * duration)));

                if (p < 0.5)
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(p * 2 * 0.75f));
                }
                else
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - p) * 2 * 0.75f));
                }
                if (p == 1f && renderer != null) renderer.enabled = false;
            })));
        }
        /// <summary>
        /// TOR改造
        /// 二次関数フラッシュを焚きます
        /// </summary>
        /// <param name="color_">色</param>
        /// <param name="duration"></param>
        public static void ShowV2Flash(Color? color_ = null, float duration = 1f)
        {
            Color color = color_ ?? Palette.ImpostorRed;

            if (HudManager.Instance == null || HudManager.Instance.FullScreen == null) return;
            HudManager.Instance.FullScreen.gameObject.SetActive(true);
            HudManager.Instance.FullScreen.enabled = true;
            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) =>
            {
                var renderer = HudManager.Instance.FullScreen;
                //renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(1 - (p * p / duration * duration)));

                if (p < 0.5)
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((p * p) * 3f));
                }
                else
                {
                    if (renderer != null)
                        renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01((1 - (p * p)) * 3f));
                }
                if (p == 1f && renderer != null) renderer.enabled = false;
            })));
        }
        /// <summary>
        /// TOR改造
        /// 0秒フラッシュを焚きます
        /// </summary>
        /// <param name="color_">色</param>
        /// <param name="duration"></param>
        public static void Show0Flash(Color? color_ = null, float duration = 2f)
        {
            Color color = color_ ?? Palette.ImpostorRed;

            if (HudManager.Instance == null || HudManager.Instance.FullScreen == null) return;
            HudManager.Instance.FullScreen.gameObject.SetActive(true);
            HudManager.Instance.FullScreen.enabled = true;
            HudManager.Instance.StartCoroutine(Effects.Lerp(duration, new Action<float>((p) =>
            {
                var renderer = HudManager.Instance.FullScreen;
                renderer.color = new Color(color.r, color.g, color.b, Mathf.Clamp01(1 - (p * p / duration * duration)));
                if (p == 1f && renderer != null) renderer.enabled = false;
            })));
        }

    }
}