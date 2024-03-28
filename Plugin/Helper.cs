using BepInEx.Configuration;
using System;
using System.IO;
using System.Linq;
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
        public static string ColoredText(Color color, string text)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + text;
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
    }

}
