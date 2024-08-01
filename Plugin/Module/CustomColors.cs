using AmongUs.Data.Legacy;
using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using System.Collections.Generic;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public class CustomColor
    {
        public static readonly List<int> ORDER = new List<int>() { 7, 37, 14, 5, 33, 41, 25,
                                                                    4, 30, 0, 35, 3, 27, 17,
                                                                    13, 23, 8, 32, 38, 1, 21,
                                                                    40, 31, 10, 34, 22, 28, 36,
                                                                    2, 11, 26, 29, 20, 19, 18,
                                                                    12, 9, 24, 16, 15, 6, 39,
                                                                    };
        public static uint pickableColors = (uint)Palette.ColorNames.Length;
        public static List<CustomColor> customColors = new();
        public static Dictionary<int, string> ColorStrings = new Dictionary<int, string>();

        public string colorName;
        public Color32 mainColor;
        public Color32 shadowColor;
        public CustomColor(string colorName, Color mainColor, Color shadowColor)
        {
            this.colorName = colorName;
            this.mainColor = mainColor;
            this.shadowColor = shadowColor;
        }
        public static void Load()
        {
            List<StringNames> longList = [.. Palette.ColorNames];
            List<Color32> colorList = [.. Palette.PlayerColors];
            List<Color32> shadowList = [.. Palette.ShadowColors];

            List<CustomColor> colors =
            [
                new("Tarmarind", new Color32(48, 28, 34,byte.MaxValue), new Color32(30, 11, 16,byte.MaxValue)),
                new("Army", new Color32(39, 45, 31,byte.MaxValue), new Color32(11, 30, 24,byte.MaxValue)),
                new("Olive", new Color32(154, 140, 61,byte.MaxValue), new Color32(104, 95, 40,byte.MaxValue)),
                new("Turquoise", new Color32(22,132,176,byte.MaxValue), new Color32(15,89,117,byte.MaxValue)),
                new("Mint", new Color32(111,192,156,byte.MaxValue), new Color32(65, 148, 111,byte.MaxValue)),
                new("Lavender", new Color32(173, 126, 201,byte.MaxValue),new Color32(131,58,203,byte.MaxValue)),
                new("Nougat", new Color32(160,101,56,byte.MaxValue),new Color32(115,15,78,byte.MaxValue)),
                new("Peach", new Color32(255, 164, 119,byte.MaxValue),new Color32(238,128,100,byte.MaxValue)),
                new("Wasabi", new Color32(112,143,46,byte.MaxValue),new Color32(72,92,26,byte.MaxValue)),
                new("HotPink", new Color32(255,51,102,byte.MaxValue),new Color32(232,0,58,byte.MaxValue)),
                new("Petrol", new Color32(0,99,105,byte.MaxValue),new Color32(0,61,54,byte.MaxValue)),
                new("Lemon", new Color32(0xDB, 0xFD, 0x2F,byte.MaxValue),new Color32(0x74, 0xE5, 0x10,byte.MaxValue)),
                new("Signal Orange", new Color32(0xF7, 0x44, 0x17,byte.MaxValue),new Color32(0x9B, 0x2E, 0x0F,byte.MaxValue)),
                new("Teal", new Color32(0x25, 0xB8, 0xBF,byte.MaxValue),new Color32(0x12, 0x89, 0x86,byte.MaxValue)),
                new("Blurple", new Color32(61,44,142,byte.MaxValue),new Color32(25,14,90,byte.MaxValue)),
                new("Sunrise", new Color32(0xFF, 0xCA, 0x19,byte.MaxValue),new Color32(0xDB, 0x44, 0x42,byte.MaxValue)),
                new("Ice", new Color32(0xA8, 0xDF, 0xFF,byte.MaxValue), new Color32(0x59, 0x9F, 0xC8,byte.MaxValue)),
                new("Fuchsia", new Color32(164, 17, 129,byte.MaxValue),new Color32(104,3,79,byte.MaxValue)),
                new("RoyalGreen", new Color32(9,82,33,byte.MaxValue),new Color32(0,46,8,byte.MaxValue)),
                new("Slime", new Color32(244,255,188,byte.MaxValue),new Color32(167,239,112,byte.MaxValue)),
                new("Navy", new Color32(9,43,119,byte.MaxValue),new Color32(0,13,56,byte.MaxValue)),
                new("Darkness", new Color32(36,39,40,byte.MaxValue),new Color32(10,10,10,byte.MaxValue)),
                new("Ocean", new Color32(55,159,218,byte.MaxValue),new Color32(62,92,158,byte.MaxValue)),
                new("Sundown", new Color32(252,194,100,byte.MaxValue),new Color32(197,98,54,byte.MaxValue)),
            ];

            int id = 50000;
            foreach (CustomColor cc in colors)
            {
                longList.Add((StringNames)id);
                ColorStrings[id++] = cc.colorName;
                colorList.Add(cc.mainColor);
                shadowList.Add(cc.shadowColor);
                Logger.Info(cc.colorName);
            }

            Palette.ColorNames = longList.ToArray();
            Palette.PlayerColors = colorList.ToArray();
            Palette.ShadowColors = shadowList.ToArray();
            pickableColors += (uint)colors.Count;
        }

        [HarmonyPatch]
        public static class CustomColorPatches
        {
            [HarmonyPatch(typeof(TranslationController), nameof(TranslationController.GetString), new[] {
                typeof(StringNames),
                typeof(Il2CppReferenceArray<Il2CppSystem.Object>)
            })]
            private class ColorStringPatch
            {
                [HarmonyPriority(Priority.Last)]
                public static bool Prefix(ref string __result, [HarmonyArgument(0)] StringNames name)
                {
                    if ((int)name >= 50000)
                    {
                        string text = ColorStrings[(int)name];
                        if (text != null)
                        {
                            __result = text;
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
        [HarmonyPatch(typeof(PlayerTab), nameof(PlayerTab.OnEnable))]
        private static class PlayerTabEnablePatch
        {
            public static void Postfix(PlayerTab __instance)
            { // Replace instead
                Il2CppArrayBase<ColorChip> chips = __instance.ColorChips.ToArray();

                int cols = 7; // TODO: Design an algorithm to dynamically position chips to optimally fill space
                for (int i = 0; i < ORDER.Count; i++)
                {
                    int pos = ORDER[i];
                    if (pos < 0 || pos > chips.Length)
                        continue;
                    ColorChip chip = chips[pos];
                    int row = i / cols, col = i % cols; // Dynamically do the positioning
                    chip.transform.localPosition = new Vector3(-0.975f + (col * 0.5f), 1.475f - (row * 0.5f), chip.transform.localPosition.z);
                    chip.transform.localScale *= 0.76f;
                }
                for (int j = ORDER.Count; j < chips.Length; j++)
                { // If number isn't in order, hide it
                    ColorChip chip = chips[j];
                    chip.transform.localScale *= 0f;
                    chip.enabled = false;
                    chip.Button.enabled = false;
                    chip.Button.OnClick.RemoveAllListeners();
                }
            }
        }
        [HarmonyPatch(typeof(LegacySaveManager), nameof(LegacySaveManager.LoadPlayerPrefs))]
        private static class LoadPlayerPrefsPatch
        { // Fix Potential issues with broken colors
            private static bool needsPatch = false;
            public static void Prefix([HarmonyArgument(0)] bool overrideLoad)
            {
                if (!LegacySaveManager.loaded || overrideLoad)
                    needsPatch = true;
            }
            public static void Postfix()
            {
                if (!needsPatch) return;
                LegacySaveManager.colorConfig %= pickableColors;
                needsPatch = false;
            }
        }
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckColor))]
        private static class PlayerControlCheckColorPatch
        {
            private static bool isTaken(PlayerControl player, uint color)
            {
                foreach (GameData.PlayerInfo p in GameData.Instance.AllPlayers.GetFastEnumerator())
                    if (!p.Disconnected && p.PlayerId != player.PlayerId && p.DefaultOutfit.ColorId == color)
                        return true;
                return false;
            }
            public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte bodyColor)
            { // Fix incorrect color assignment
                uint color = (uint)bodyColor;
                if (isTaken(__instance, color) || color >= Palette.PlayerColors.Length)
                {
                    int num = 0;
                    while (num++ < 50 && (color >= pickableColors || isTaken(__instance, color)))
                    {
                        color = (color + 1) % pickableColors;
                    }
                }
                __instance.RpcSetColor((byte)color);
                return false;
            }
        }
    }
}