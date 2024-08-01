using BepInEx.Configuration;
using Il2CppSystem.Collections.Generic;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace TheSpaceRoles
{
    public static class Helper
    {
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
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text+"</color>";
        }

        public static int Random(int a, int b)
        {

            System.Random r = new System.Random();
            return r.Next(a, b + 1);
        }
        public static void AllAddChat(string Chat, ChatController __instance, string chpname = null)
        {
            string name = (PlayerControl.LocalPlayer).name;
            Logger.Info("show chat", "", "AllAddChat");
            string name2 = (PlayerControl.LocalPlayer).name;
            if (chpname == null)
            {
                PlayerControl.LocalPlayer.RpcSetName($"<size=180%>{TSR.cs_name}");
            }
            else
            {
                PlayerControl.LocalPlayer.RpcSetName(chpname);
            }
            PlayerControl.LocalPlayer.RpcSendChat("\n<size=90%>" + Chat);
            PlayerControl.LocalPlayer.RpcSetName(name);
            __instance.freeChatField.Clear();
        }

        public static void AddChat(string Chat, ChatController __instance)
        {
            string name = (PlayerControl.LocalPlayer).name;
            PlayerControl.LocalPlayer.SetName($"<size=180%>{TSR.cs_name}", false);
            __instance.AddChat(PlayerControl.LocalPlayer, "\n<size=90%>" + Chat, true);
            PlayerControl.LocalPlayer.SetName(name, false);
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
            using (MemoryStream stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, src);
                stream.Position = 0;

                return (T)formatter.Deserialize(stream);
            }
        }
        public static System.Collections.Generic.IEnumerable<T> GetFastEnumerator<T>(this List<T> list) where T : Il2CppSystem.Object => new Il2CppListEnumerable<T>(list);
    }

    public unsafe class Il2CppListEnumerable<T> : System.Collections.Generic.IEnumerable<T>, System.Collections.Generic.IEnumerator<T> where T : Il2CppSystem.Object
    {
        private struct Il2CppListStruct
        {
#pragma warning disable CS0169
            private IntPtr _unusedPtr1;
            private IntPtr _unusedPtr2;
#pragma warning restore CS0169

#pragma warning disable CS0649
            public IntPtr _items;
            public int _size;
#pragma warning restore CS0649
        }

        private static readonly int _elemSize;
        private static readonly int _offset;
        private static Func<IntPtr, T> _objFactory;

        static Il2CppListEnumerable()
        {
            _elemSize = IntPtr.Size;
            _offset = 4 * IntPtr.Size;

            var constructor = typeof(T).GetConstructor(new[] { typeof(IntPtr) });
            var ptr = Expression.Parameter(typeof(IntPtr));
            var create = Expression.New(constructor!, ptr);
            var lambda = Expression.Lambda<Func<IntPtr, T>>(create, ptr);
            _objFactory = lambda.Compile();
        }

        private readonly IntPtr _arrayPointer;
        private readonly int _count;
        private int _index = -1;

        public Il2CppListEnumerable(List<T> list)
        {
            var listStruct = (Il2CppListStruct*)list.Pointer;
            _count = listStruct->_size;
            _arrayPointer = listStruct->_items;
        }

        object IEnumerator.Current => Current;
        public T Current { get; private set; }

        public bool MoveNext()
        {
            if (++_index >= _count) return false;
            var refPtr = *(IntPtr*)IntPtr.Add(IntPtr.Add(_arrayPointer, _offset), _index * _elemSize);
            Current = _objFactory(refPtr);
            return true;
        }

        public void Reset()
        {
            _index = -1;
        }

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
