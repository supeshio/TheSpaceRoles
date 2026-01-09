using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TSR
{
    public static unsafe class FastDestroyableSingleton<T> where T : MonoBehaviour
    {
        private static readonly IntPtr _fieldPtr;
        private static readonly Func<IntPtr, T> _createObject;

        static FastDestroyableSingleton()
        {
            _fieldPtr = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DestroyableSingleton<T>>.NativeClassPtr, nameof(DestroyableSingleton<T>._instance));
            var constructor = typeof(T).GetConstructor(new[] { typeof(IntPtr) });
            var ptr = Expression.Parameter(typeof(IntPtr));
            var create = Expression.New(constructor!, ptr);
            var lambda = Expression.Lambda<Func<IntPtr, T>>(create, ptr);
            _createObject = lambda.Compile();
        }

        public static T Instance
        {
            get
            {
                IntPtr objectPointer;
                IL2CPP.il2cpp_field_static_get_value(_fieldPtr, &objectPointer);
                return objectPointer == IntPtr.Zero ? DestroyableSingleton<T>.Instance : _createObject(objectPointer);
            }
        }
    }

    public static class Helper
    {
        public static readonly Color32 invisible = ColorFromColorcode("#00000000");
        public static readonly Color32 white = Color.white;
        public static readonly Color32 black = Color.black;
        public static readonly Color32 magenta = Color.magenta;
        public const int UILayer = 5;
        public static byte MaxFrequency(this List<byte> self, out bool tie)
        {
            if (self == null || self.Count == 0)
            {
                throw new ArgumentException("List is null or empty");
            }

            // Dictionary to count occurrences of each byte
            Dictionary<byte, int> frequency = new();

            // Count occurrences of each byte
            foreach (var num in self)
            {
                if (frequency.ContainsKey(num))
                {
                    frequency[num]++;
                }
                else
                {
                    frequency[num] = 1;
                }
            }

            // Find the most frequent byte and check for ties
            int maxFrequency = 0;
            byte mostFrequentNumber = 0;
            tie = false;

            foreach (var pair in frequency)
            {
                if (pair.Value > maxFrequency)
                {
                    maxFrequency = pair.Value;
                    mostFrequentNumber = pair.Key;
                    tie = false; // Reset tie if a new max frequency is found
                }
                else if (pair.Value == maxFrequency)
                {
                    tie = true; // Set tie if another byte has the same max frequency
                }
            }

            return mostFrequentNumber;
        }

        public static bool InArea(Vector3 Position, Vector3 startPos, Vector3 endPos)
        {
            if (startPos.x > endPos.x)
            {
                if (startPos.x > Position.x && Position.x > endPos.x)
                {
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (startPos.x < Position.x && Position.x < endPos.x)
                {
                }
                else
                {
                    return false;
                }
            }
            if (startPos.y > endPos.y)
            {
                if (startPos.y > Position.y && Position.y > endPos.y)
                {
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (startPos.y < Position.y && Position.y < endPos.y)
                {
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static Il2CppSystem.Collections.Generic.List<T> ToIl2CppList<T>(this List<T> t)
        {
            Il2CppSystem.Collections.Generic.List<T> v = new();
            foreach (T t2 in t)
            {
                v.Add(t2);
            }
            return v;
        }

        public static List<T> ToList<T>(this Il2CppSystem.Collections.Generic.List<T> t)
        {
            return [.. t];
        }

        public static UnityEngine.Color32 ColorFromColorcode(string colorcode)
        {
            if (ColorUtility.TryParseHtmlString(colorcode, out Color color))
            {
                return color;
            }
            else
            {
                return Color.magenta;
            }
        }

        public static UnityEngine.Color ColorFromColorcode(int colorcode)
        {
            ColorUtility.TryParseHtmlString("#" + colorcode.ToString(), out Color color);
            return color;
        }

        public static UnityEngine.Color ColorEditHSV(Color color, float h = 0, float s = 0, float v = 0, float a = 1)
        {
            Color.RGBToHSV(color, out float h1, out float s1, out float v1);
            h1 += h;
            s1 += s;
            v1 += v;
            color = Color.HSVToRGB(h1, s1, v1);
            color.a = a;
            return color;
        }
        public static string ColoredText(Color color, string text)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text + "</color>";
        }

        public static System.Random r = new((int)Environment.TickCount);

        /// <summary>
        /// ランダムな数値を出す｡
        /// </summary>
        /// <param name="a"></param>
        /// 最小値
        /// <param name="b"></param>
        /// 最大値-1
        /// <returns></returns>
        public static int Random(int a, int b)
        {
            return r.Next(a, b);
        }

        /// <summary>
        /// ランダムな数値を出す｡
        /// </summary>
        /// <param name="b">最大値-1</param>
        /// <returns></returns>
        public static int Random(int b)
        {
            return r.Next(b);
        }

        public static void AllAddChat(string Chat, string chpname = null)
        {
            string name = (PlayerControl.LocalPlayer).name;
            Logger.Info("show chat", "", "AllAddChat");
            if (chpname == null)
            {
                PlayerControl.LocalPlayer.RpcSetName($"<size=180%>{TSR.cs_name_v}");
            }
            else
            {
                PlayerControl.LocalPlayer.RpcSetName(chpname);
            }
            PlayerControl.LocalPlayer.RpcSendChat("\n<size=90%>" + Chat);
            PlayerControl.LocalPlayer.RpcSetName(name);
            DestroyableSingleton<ChatController>.Instance.freeChatField.Clear();
        }

        public static void RpcRepairSystem(this ShipStatus shipStatus, SystemTypes systemType, byte amount)
        {
            shipStatus.RpcUpdateSystem(systemType, amount);
        }

        public static void AddChat(string Chat)
        {
            string name = (PlayerControl.LocalPlayer).name;
            PlayerControl.LocalPlayer.SetName($"<size=180%>{TSR.cs_name_v}");
            DestroyableSingleton<ChatController>.Instance.AddChat(PlayerControl.LocalPlayer, "\n<size=90%>" + Chat, true);
            PlayerControl.LocalPlayer.SetName(name);
        }

        public static T ClassLib<T>() where T : Il2CppObjectBase
        {
            return Resources.FindObjectsOfTypeAll(Il2CppType.Of<T>())[0].Cast<T>();
        }

        public static Material TextMaterial_Ping() => ClassLib<PingTracker>().text.material;

        public static TMPro.TMP_FontAsset TextFont_Ping() => ClassLib<PingTracker>().text.font;

        public static void Set(this PassiveButton passiveButton, bool ClickSound = true, bool HoverSound = true, Action OnClick = null, Action OnHoverIn = null, Action OnHoverOut = null)
        {
            passiveButton.gameObject.layer = 5;
            if (ClickSound) passiveButton.ClickSound = ClassLib<ChatInputFieldButton>().button.GetComponent<PassiveButton>().ClickSound;

            if (HoverSound) passiveButton.HoverSound = ClassLib<ChatInputFieldButton>().button.GetComponent<PassiveButton>().HoverSound;
            var sp = passiveButton.gameObject.GetComponent<SpriteRenderer>();
            var box = passiveButton.gameObject.AddComponent<BoxCollider2D>();
            box.size = sp.size;
            passiveButton.Colliders = new[] { box };
            passiveButton.OnClick = new();
            passiveButton.OnMouseOver = new();
            passiveButton.OnMouseOut = new();
            passiveButton.OnClick.AddListener((UnityAction)(() => { OnClick?.Invoke(); }));
            passiveButton.OnMouseOver.AddListener((UnityAction)(() => { OnHoverIn?.Invoke(); }));
            passiveButton.OnMouseOut.AddListener((UnityAction)(() => { OnHoverOut?.Invoke(); }));
            passiveButton._CachedZ_k__BackingField = 0.1f;
            passiveButton.CachedZ = 0.1f;
        }

        public static void SetHoverColor(this PassiveButton passiveButton, Color32 HoverColor, Color32? HoverOut = null, Func<bool> UseHoverColor = null, Func<bool> UseEnableColor = null, Color32? EnableColor = null)
        {
            passiveButton.OnMouseOver.AddListener((UnityAction)(() =>
            {
                //Logger.Info(UseEnableColor?.Invoke().ToString());
                if (UseEnableColor?.Invoke() == true)
                {
                    passiveButton.SetColor(EnableColor ?? Palette.AcceptedGreen);
                }
                else if (UseHoverColor == null || (bool)UseHoverColor?.Invoke())
                {
                    passiveButton.SetColor(HoverColor);
                }
            }));
            passiveButton.OnMouseOut.AddListener((UnityAction)(() =>
            {
                if (UseEnableColor?.Invoke() == true)
                {
                    passiveButton.SetColor(EnableColor ?? Palette.AcceptedGreen);
                }
                else
                if (UseHoverColor == null || (bool)UseHoverColor?.Invoke())
                {
                    passiveButton.SetColor(HoverOut ?? ColorFromColorcode("#fff"));
                }
            }));
        }

        public static void SetEnableColor(this PassiveButton passiveButton, Color32 EnableColor)
        {
            passiveButton.OnClick.AddListener((UnityAction)(() => { passiveButton.GetComponent<SpriteRenderer>().color = EnableColor; }));
        }

        public static void SetColor(this PassiveButton passiveButton, Color32 Color)
        {
            passiveButton.GetComponent<SpriteRenderer>().color = Color;
        }

        public static void InitText(this TextMeshPro Text, string text, Vector2 ContainerSize, Color32 color, float FontSize, float? FontSizeMin = null, float? FontSizeMax = null, TextAlignmentOptions Align = TextAlignmentOptions.Left)
        {
            Text.text = text;
            Text.color = color;
            Text.fontSize = FontSize;
            Text.fontSizeMax = FontSizeMax ?? FontSize;
            Text.fontSizeMin = FontSizeMin ?? FontSize;
            Text.alignment = Align;
            Text.enableWordWrapping = false;
            Text.transform.localPosition = Vector3.zero;
            Text.gameObject.layer = 5;//UILayer
            Text.enableAutoSizing = true;
            Text.transform.localScale = Vector3.one;
            Text.textContainer.size = ContainerSize;
        }
    }
}