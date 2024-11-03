using BepInEx.Configuration;
using HarmonyLib;
using Il2CppInterop.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TheSpaceRoles
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
        public static Color invisible = ColorFromColorcode("#00000000");
        public static string Joinsep(this IEnumerable<object> list, string separator)
        {
            return string.Join(separator, list);
        }
        public static string Joinsep(this List<object> list, string separator)
        {
            return string.Join(separator, list);
        }
        public static string Joinsep(this object[] list, string separator)
        {
            return string.Join(separator, list);
        }
        public static Roles GetRole(this PlayerControl p)
        {
            return DataBase.AllPlayerRoles[p.PlayerId].Role;
        }
        public static void Init(this PlayerControl p)
        {
            DataBase.AllPlayerRoles[p.PlayerId].Init();
        }
        public static void ButtonResetStart(this PlayerControl p)
        {
            DataBase.AllPlayerRoles[p.PlayerId].ButtonReset();
        }
        public static PlayerControl GetPlayerById(int id)
        {
            return PlayerControl.AllPlayerControls.ToArray().First(x => x.PlayerId == id);
        }
        public static CustomRole GetCustomRole(this PlayerControl p)
        {
            return DataBase.AllPlayerRoles[p.PlayerId];
        }
        public static bool IsRole(this PlayerControl p, Roles role)
        {
            return DataBase.AllPlayerRoles[p.PlayerId].Role == role;
        }
        public static bool IsTeam(this PlayerControl p, Teams team)
        {
            return DataBase.AllPlayerRoles[p.PlayerId].team == team;
        }
        public static byte MaxFrequency(this List<byte> self, out bool tie)
        {
            if (self == null || self.Count == 0)
            {
                throw new ArgumentException("List is null or empty");
            }

            // Dictionary to count occurrences of each byte
            Dictionary<byte, int> frequency = new Dictionary<byte, int>();

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

        public static int MaxFrequency(this List<int> self, out bool tie)
        {
            if (self == null || self.Count == 0)
            {
                throw new ArgumentException("List is null or empty");
            }

            // Dictionary to count occurrences of each number
            Dictionary<int, int> frequency = new Dictionary<int, int>();

            // Count occurrences of each number
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

            // Find the most frequent number and check for ties
            int maxFrequency = 0;
            int mostFrequentNumber = 0;
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
                    tie = true; // Set tie if another number has the same max frequency
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

        public static UnityEngine.Color ColorFromColorcode(string colorcode)
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
        public static System.Random r = new System.Random((int)Environment.TickCount);
        public static int Random(int a, int b)
        {
            return r.Next(a, b + 1);
        }
        public static int RandomNext(int b)
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

        public static void AddChat(string Chat)
        {
            string name = (PlayerControl.LocalPlayer).name;
            PlayerControl.LocalPlayer.SetName($"<size=180%>{TSR.cs_name_v}");
            DestroyableSingleton<ChatController>.Instance.AddChat(PlayerControl.LocalPlayer, "\n<size=90%>" + Chat, true);
            PlayerControl.LocalPlayer.SetName(name);
        }

        public static Tuple<string, bool> ChatBool(string[] str, string text, ConfigEntry<bool> config, ref string AddChat)
        {
            try
            {
                if (str.Length > 1)
                {
                    if (str[1] == "true" || str[1] == "t")
                    {
                        config.Value = true;
                    }
                    else if (str[1] == "false" || str[1] == "f")
                    {
                        config.Value = false;
                    }
                    else if (config.Value)
                    {
                        config.Value = false;
                    }
                    else
                    {
                        config.Value = true;
                    }
                }
                else if (config.Value)
                {
                    config.Value = false;
                }
                else
                {
                    config.Value = true;
                }
            }
            catch
            {
                if (config.Value)
                {
                    config.Value = false;
                }
                else
                {
                    config.Value = true;
                }
            }
            AddChat = AddChat + text + "を" + config.Value + "にしました";
            return Tuple.Create(AddChat, config.Value);
        }
        public static PlayerControl GetPlayerControlFromId(int id) => DataBase.AllPlayerControls().First(x => x.PlayerId == id);


        public static void SetPlayerScale(PlayerControl pc, float scale)
        {
            pc.transform.FindChild("BodyForms").localScale = Vector3.one * scale;
            pc.transform.FindChild("Cosmetics").localScale = Vector3.one * scale * 0.5f;
            pc.GetComponent<CircleCollider2D>().radius = 0.2234f * scale;

        }

        public static T JsonSerializerByteClone<T>(T src)
        {
            ReadOnlySpan<byte> b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes<T>(src);
            return System.Text.Json.JsonSerializer.Deserialize<T>(b);
        }
        public static T DeepCopy<T>(this T src)
        {
            using MemoryStream stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, src);
            stream.Position = 0;

            return (T)formatter.Deserialize(stream);
        }
    }
}
