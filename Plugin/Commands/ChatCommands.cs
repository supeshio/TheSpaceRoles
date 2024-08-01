using AmongUs.GameOptions;
using HarmonyLib;
using InnerNet;
using System;
using System.Text.RegularExpressions;
using TheSpaceRoles.Plugin;
using UnityEngine;

namespace TheSpaceRoles
{

    [HarmonyPatch(typeof(ChatController), nameof(ChatController.SendChat))]
    public static class ChatControllerPatch
    {
        public static bool Prefix(ChatController __instance)
        {
            if (__instance.timeSinceLastMessage > 3f)
            {
                KeyCommands.chattexts.Add(__instance.freeChatField.textArea.text);
                KeyCommands.undocount = KeyCommands.chattexts.Count;
            }
            return AddChat(__instance, __instance.freeChatField.textArea.text);
        }
        public static bool AddChat(ChatController __instance, string chat)
        {
            string[] chats = chat.Split(' ');
            if (chat.StartsWith("/"))
            {
                string addchat = "";
                string rpcaddchat = "";
                switch (chats[0])
                {
                    case "/help":
                    case "/h":
                    case "/?":
                        string chatcommands =
                            "チャットコマンド \n" +
                            "/help,h,? : ヘルプを表示 \n" +
                            "/help,h,? key,k: キーコマンドの表示 \n" +
                            "/help,h,? chat,c : チャットコマンドの表示 \n " +
                            "/ver,v : バージョンを表示 \n" +
                            "/name,n {新しい名前} \n" +
                            "/gameend,ge : 廃村にする \n" +
                            "/fakelevel,fl {level(整数)} : レベルを変える \n" +
                            "/lobbytimer,lt [true/false] : ロビータイマーをつける\n";
                        string keycommands =
                            "キーコマンド \n" +
                            "左右Shift + H : 廃村 \n" +
                            "左右Shift + S : 会議スキップ \n" +
                            " C : 試合スタートをキャンセル \n" +
                            " V : 試合すぐに始める\n" +
                            "Ctrl + C : コピー\n" +
                            "Ctrl + X : カット\n" +
                            "Ctrl + V : ペースト\n" +
                            "Ctrl + Z , 上矢印キー : 元に戻す\n" +
                            "Ctrl + Y , 下矢印キー : やり直し\n";
                        if (chats.Length == 1)
                        {
                            addchat += chatcommands + keycommands;
                            break;
                        }
                        else
                        {

                            string commands_;
                            switch (chats[1])
                            {
                                case "chat":
                                case "c":
                                    commands_ = chatcommands;
                                    break;
                                case "key":
                                case "k":
                                    commands_ = keycommands;
                                    break;
                                default:
                                    commands_ = chatcommands + keycommands;
                                    break;
                            }
                            addchat += commands_;
                        }
                        break;
                    case "/version":
                    case "/ver":
                    case "/v":
                        addchat += "<color=#303030> </color>";
                        break;
                    case "/name":
                    case "/n":
                        string s;
                        if (chats[0] == "/name")
                        {
                            s = chat.Substring(6);
                        }
                        else
                        {
                            s = chat.Substring(3);
                        }
                        s = s.Replace("\\n", "<br>");
                        if (s.Length > 0)
                        {
                            PlayerControl.LocalPlayer.RpcSetName(s);
                            Logger.Info("changed name : " + s, "", "Addedchat");
                            addchat = addchat + "名前を " + s + "<\\color> に変更しました";
                        }
                        else
                        {
                            Logger.Info("couldn't change name : " + s, "", "Addedchat");
                            addchat += "文字数は0字以上にしてください";
                        }
                        break;
                    case "/namecolor":
                    case "/nc":
                        {
                            string name = (PlayerControl.LocalPlayer).name;
                            name = Regex.Replace(name, "<color[^>]*?>", string.Empty);
                            name = Regex.Replace(name, "<\\color[^>]*?>", string.Empty);
                            string color = chats[1];
                            if (color.StartsWith("#") || color.StartsWith("0x"))
                            {
                                color = "#" + color;
                            }
                            var c = Helper.ColorFromColorcode(color);
                            PlayerControl.LocalPlayer.cosmetics.nameText.color = c;

                            break;
                        }
                    case "/gameend":
                    case "/ge":
                        if (((InnerNetClient)AmongUsClient.Instance).AmHost)
                        {
                            ((Behaviour)GameManager.Instance).enabled = false;
                            GameManager.Instance.RpcEndGame((GameOverReason)3, false);
                            GameManager.Instance.RpcEndGame((GameOverReason)8, false);
                        }
                        break;
                    case "/skip":
                    case "/s":
                        if (((InnerNetClient)AmongUsClient.Instance).AmHost)
                        {
                            MeetingHud.Instance.RpcClose();
                            Logger.Message("会議スキップ", "", "Addedchat");
                        }
                        break;
                    case "/fl":
                    case "/falelevel":
                        if (chats[1] == "max")
                        {
                            PlayerControl.LocalPlayer.RpcSetLevel(4294967294u);
                            addchat = "見かけのLevelを" + chats[1] + "にしました。";
                        }
                        try
                        {
                            if (Regex.IsMatch(chats[1], "^[0-9]+$"))
                            {
                                PlayerControl.LocalPlayer.RpcSetLevel(uint.Parse(chats[1]) - 1);
                                addchat = "見かけのLevelを" + chats[1] + "にしました。";
                            }
                        }
                        catch (System.Exception)
                        {
                            addchat = $"その値は無効です。{uint.MinValue}～{uint.MaxValue}の整数の範囲で指定してください。";
                        }
                        break;
                    case "/kc":
                    case "/killcool":
                        try
                        {
                            GameOptionsManager.Instance.CurrentGameOptions.SetFloat((FloatOptionNames)1, float.Parse(chats[1]));
                            addchat += "killcool を " + chats[1] + "にしました";
                        }
                        catch
                        {
                            addchat += "killcoolを" + chats[1] + "にすることを失敗しました";
                        }
                        break;
                    case "/lobbytimer":
                    case "/lt":
                        (addchat, TSR.LobbyTimer.Value) = Helper.ChatBool(chats, "LobbyTimer", TSR.LobbyTimer, ref addchat);
                        break;
                    case "/\\":
                        for (int i = 0; i < Palette.PlayerColors.Length; i++)
                        {
                            string st = "";
                            if (i <= 18)
                            {
                                st = Palette.ColorNames[i].ToString();
                            }
                            else
                            {
                                st =
                                CustomColor.ColorStrings[i - 18 + 50000];
                            }
                            addchat += Helper.ColoredText(Palette.PlayerColors[i], "■") + Helper.ColoredText(Palette.ShadowColors[i], "■") + Helper.ColoredText(Palette.PlayerColors[i], "<b>" + st + "</b>\n");
                        }
                        break;
                    case "//":
                        for (int i = 0; i < Palette.PlayerColors.Length; i++)
                        {
                            addchat += Helper.ColoredText(Palette.PlayerColors[i], "■") + Helper.ColoredText(Palette.ShadowColors[i], "■") + Helper.ColoredText(Palette.PlayerColors[i], "<b>" + Palette.ColorNames[i] + "</b>\n");
                        }
                        break;
                }
                if (addchat != "")
                {
                    Helper.AddChat(addchat, __instance);
                }
                if (rpcaddchat != "")
                {
                    Helper.AllAddChat(addchat, __instance);
                }
                __instance.freeChatField.Clear();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [HarmonyPatch(typeof(HudManager), "Update")]
    public static class EnableChat
    {
        public static void Postfix(HudManager __instance)
        {
            //IL_001a: Unknown result type (might be due to invalid IL or missing references)
            //IL_0020: Invalid comparison between Unknown and I4
            if (!__instance.Chat.isActiveAndEnabled)
            {
                AmongUsClient instance = AmongUsClient.Instance;
                if (instance != null && (int)instance.NetworkMode == 2)
                {
                    __instance.Chat.SetVisible(true);
                }
            }
        }
    }

}
