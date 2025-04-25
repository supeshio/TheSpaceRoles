using AmongUs.GameOptions;
using HarmonyLib;
using InnerNet;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static TheSpaceRoles.Helper;

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
                        string chatcommands = "";
                        chatcommands += "<b><color=#12c2f5><size=120%>チャットコマンド</size></color></b> \n";
                        chatcommands += "/help,h,? : ヘルプを表示 \n";
                        chatcommands += "/help,h,? key,k: キーコマンドの表示 \n";
                        chatcommands += "/help,h,? chat,c : チャットコマンドの表示 \n ";
                        chatcommands += "/ver,v : バージョンを表示 \n";
                        chatcommands += "/name,n {新しい名前} \n";
                        chatcommands += "/gameend,ge : 廃村にする \n";
                        chatcommands += "/fakelevel,fl {level(整数)} : レベルを変える \n";
                        chatcommands += "/lobbytimer,lt [true/false] : ロビータイマーをつける\n";
                        if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
                        {

                            chatcommands += "<color=#f5f512><size=110%>フリープレイ限定</size></color>\n";
                            chatcommands += "/setrole : 役職ID or 役職名から役職を設定する\n";
                            chatcommands += "/playerlist : プレイヤー名と役職名のリストを表示\n";
                            chatcommands += "/rolelist : 役職名と役職IDのリストを表示\n";
                        }

                            ;
                        string keycommands =
                            "<b><color=#12c2f5><size=120%>キーコマンド</size></color></b> \n" +
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
                            s = chat[6..];
                        }
                        else
                        {
                            s = chat[3..];
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
                            //PlayerControl.LocalPlayer.cosmetics.nameText.color = c;

                            break;
                        }
                    case "/gameend":
                    case "/ge":
                        if (AmongUsClient.Instance.AmHost)
                        {
                            GameManager.Instance.enabled = false;
                            GameManager.Instance.RpcEndGame((GameOverReason)3, false);
                            GameManager.Instance.RpcEndGame((GameOverReason)8, false);
                        }
                        break;
                    case "/skip":
                    case "/s":
                        if (AmongUsClient.Instance.AmHost)
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
                    //case "/debug":
                    //    (addchat, TSR.DebugMode.Value) = Helper.ChatBool(chats, "DebugMode", TSR.LobbyTimer, ref addchat);
                    //    break;
                    case "/setrole":
                        if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
                        {

                            if (int.TryParse(chats[1], out int roleId))
                            {

                                DataBase.AllPlayerData.Remove(PlayerControl.LocalPlayer.PlayerId);
                                DataBase.ResetButtons();
                                if (RoleData.GetCustomRoles[roleId].Team == Teams.Impostor)
                                {
                                    PlayerControl.LocalPlayer.RpcSetRole(RoleTypes.Impostor, true);
                                }
                                else
                                {


                                    PlayerControl.LocalPlayer.RpcSetRole(RoleTypes.Crewmate, true);
                                }
                                RoleSelect.SendRpcSetRole(RoleData.GetCustomRoles[roleId].Role, PlayerControl.LocalPlayer.PlayerId);
                                PlayerControl.LocalPlayer.Init();
                                PlayerControl.LocalPlayer.ButtonResetStart();
                                Helper.GetCustomRole(PlayerControl.LocalPlayer).HudManagerStart(HudManager.Instance);
                                addchat += $"役職を設定しました。\n Role : {RoleData.GetCustomRoles[roleId].ColoredRoleName}";
                                break;
                            }
                            else
                            {
                                foreach (var role in RoleData.GetCustomRoles)
                                {
                                    if (role.RoleName == chats[1])
                                    {
                                        DataBase.AllPlayerData.Remove(PlayerControl.LocalPlayer.PlayerId);
                                        DataBase.ResetButtons();
                                        if (role.Team == Teams.Impostor)
                                        {
                                            PlayerControl.LocalPlayer.RpcSetRole(RoleTypes.Impostor, true);
                                        }
                                        else
                                        {

                                            PlayerControl.LocalPlayer.RpcSetRole(RoleTypes.Crewmate, true);
                                        }

                                        RoleSelect.SendRpcSetRole(role.Role, PlayerControl.LocalPlayer.PlayerId);
                                        PlayerControl.LocalPlayer.Init();
                                        PlayerControl.LocalPlayer.ButtonResetStart();
                                        Helper.GetCustomRole(PlayerControl.LocalPlayer).HudManagerStart(HudManager.Instance);
                                        addchat += $"役職を設定しました。\n Role : {role.ColoredRoleName}";

                                        break;
                                    }
                                }


                            }
                            addchat += "役職を設定できませんでした。";
                            break;
                        }
                        break;
                    case "/rolelist":
                        int i = 0;

                        foreach (var name in RoleData.GetCustomRoles.Select(x => x.ColoredRoleName))
                        {

                            addchat += i++ + " : " + name + ("\n");
                        }

                        break;

                }
                if (TSR.DebugMode.Value)
                {

                    switch (chats[0])
                    {

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
                            GameStartManager.Instance.HostInfoPanel.playerName.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Normal;
                            addchat += Path.GetDirectoryName(Application.dataPath).ToString() + "\n";
                            for (int i = 0; i < Palette.PlayerColors.Length; i++)
                            {
                                addchat += Helper.ColoredText(Palette.PlayerColors[i], "■") + Helper.ColoredText(Palette.ShadowColors[i], "■") + Helper.ColoredText(Palette.PlayerColors[i], "<b>" + Palette.ColorNames[i] + "</b>\n");
                            }
                            var ob = GameObject.Instantiate(FastDestroyableSingleton<NotificationPopper>.Instance.notificationMessageOrigin, FastDestroyableSingleton<NotificationPopper>.Instance.transform).GetComponent<LobbyNotificationMessage>();
                            ob.SetUp($"<b>血液型</b> を <b>ab</b> に設定する", ob.Icon.sprite, Color.white, (Il2CppSystem.Action)(() => { }));
                            //NiceGuesser.instance.TargetReset(HudManager.Instance.MeetingPrefab);
                            break;
                        case "/ms":
                        case "/meetingskip":
                            MeetingHud.Instance.discussionTimer = 0;
                            MeetingHud.Instance.resultsStartedAt = 0;
                            MeetingHud.Instance.lastSecond = 0;
                            addchat += "meetingskip" + "\n";
                            break;
                        case "/m5":
                            addchat += $"{DataBase.AllPlayerData.Join(x => x.Key + ":" + x.Value.CustomRole.ColoredRoleName + "\n")}";
                            break;
                        case "/":
                            if (AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay) break;
                            addchat += "count : " + DataBase.AllPlayerData.Count + "\n";
                            addchat += "playerlist : \n";
                            addchat += DataBase.AllPlayerData.Select(x => PlayerControl.AllPlayerControls.ToArray().First(z => z.PlayerId == x.Key).Data.PlayerName + ":" + x.Value.CustomRole.ColoredRoleName).Joinsep("\n");
                            addchat += "assignedRole:" + DataBase.AssignedRoles().Select(x => x.ToString()).Joinsep("\n");
                            addchat +=$"MyPhysics.Speed {PlayerControl.LocalPlayer.MyPhysics.TrueSpeed} {PlayerControl.LocalPlayer.MyPhysics.SpeedMod}";
                            break;
                        case "/vent":
                            if (AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay) break;
                            PlayerControl.LocalPlayer.MyPhysics.RpcEnterVent(int.Parse(chats[1]));

                            break;
                        case "/vvent":
                            if (AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay) break;
                            PlayerControl.LocalPlayer.MyPhysics.RpcExitVent(int.Parse(chats[1]));
                            break;
                        case "/alert":
                            DestroyableSingleton<AlertFlash>.Instance.Flash();
                            break;
                        case "/alertc":
                            FlashPatch.ShowFlash(Helper.ColorFromColorcode(chats[1]));
                            break;
                        case "/alert2":
                            FlashPatch.ShowV2Flash(Helper.ColorFromColorcode(chats[1]));
                            break;
                        case "/alert0":
                            FlashPatch.Show0Flash(Helper.ColorFromColorcode(chats[1]));
                            break;
                        case "/map":
                            switch (chats[1])
                            {
                                case "s":
                                case "sabo":
                                case "sab":
                                    HudManager.Instance.InitMap();
                                    MapBehaviour.Instance.ShowSabotageMap();
                                    break;
                                case "ad":
                                case "a":
                                    HudManager.Instance.InitMap();
                                    MapBehaviour.Instance.ShowCountOverlay(true, true, true);
                                    break;
                                default:

                                    HudManager.Instance.InitMap();
                                    MapBehaviour.Instance.ShowNormalMap();
                                    break;

                            }
                            break;
                        case "/kill":

                            var ph = (string player) =>
                            {
                                if(player == ":me:")
                                {
                                    return PlayerControl.LocalPlayer;
                                }else if(Int32.TryParse(player, out int k))
                                {
                                    return Helper.GetPlayerById(k);
                                }else
                                {
                                    return DataBase.AllPlayerControls().FirstOrDefault(x=>x.Data.PlayerName==player);
                                }
                            };
                            if (chats[2] != null)
                            {

                                string p1 = chats[1];
                                string p2 = chats[2];
                                PlayerControl c1 = ph(p1);
                                PlayerControl c2 = ph(p2);
                                CheckedMurderPlayer.RpcMurder(c1, c2, DeathReason.KillByCommand);

                            }
                            else
                            {

                                string p2 = chats[1];
                                PlayerControl c2 = ph(p2);
                                CheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, c2, DeathReason.KillByCommand);
                            }
                            break;
                        case "/report":

                            var ph3 = (string player) =>
                            {
                                if (player == ":me:")
                                {
                                    return PlayerControl.LocalPlayer;
                                }
                                else if (Int32.TryParse(player, out int k))
                                {
                                    return Helper.GetPlayerById(k);
                                }
                                else
                                {
                                    return DataBase.AllPlayerControls().FirstOrDefault(x => x.Data.PlayerName == player);
                                }
                            };
                            if (chats[2] != null)
                            {

                                string p1 = chats[1];
                                string p2 = chats[2];
                                PlayerControl c1 = ph3(p1);
                                PlayerControl c2 = ph3(p2);
                                c1.CmdReportDeadBody(c2.Data);

                            }
                            else
                            {

                                string p2 = chats[1];
                                PlayerControl c2 = ph3(p2);
                                PlayerControl.LocalPlayer.CmdReportDeadBody(c2.Data);
                            }
                            break;
                        case "/sec":
                            LateTask.AddTask(Int32.Parse(chats[1]),() => Helper.AddChat(chats[1] + "秒後にメッセージ"));
                            break;
                        case "/db":
                            DeathGhost.ShowGhost(PlayerControl.LocalPlayer.GetTruePosition(), PlayerControl.LocalPlayer.PlayerId);
                            LateTask.AddTask(0, () => Helper.AddChat("ShowGhost"));
                            break;
                        case "/anim":
                            PlayerControl.LocalPlayer.PlayAnimation(byte.Parse(chats [1]));
                            break;
                        case "/sr":
                            if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
                            {

                                if (int.TryParse(chats[1], out int roleId))
                                {
                                    int playerId = PlayerControl.LocalPlayer.PlayerId;
                                    if (int.TryParse(chats[2], out playerId))
                                    {

                                        DataBase.AllPlayerData.Remove(playerId);
                                        DataBase.ResetButtons();
                                        if (RoleData.GetCustomRoles[roleId].Team == Teams.Impostor)
                                        {
                                            Helper.GetPlayerById(playerId).RpcSetRole(RoleTypes.Impostor, true);
                                        }
                                        else
                                        {


                                            Helper.GetPlayerById(playerId).RpcSetRole(RoleTypes.Crewmate, true);
                                        }
                                        RoleSelect.SendRpcSetRole(RoleData.GetCustomRoles[roleId].Role, Helper.GetPlayerById(playerId).PlayerId);
                                        Helper.GetPlayerById(playerId).Init();
                                        Helper.GetPlayerById(playerId).ButtonResetStart();
                                        Helper.GetCustomRole(Helper.GetPlayerById(playerId)).HudManagerStart(HudManager.Instance);
                                        addchat += $"役職を設定しました。\n Role : {RoleData.GetCustomRoles[roleId].ColoredRoleName}";
                                        break;
                                    }
                                }
                                
                                addchat += "役職を設定できませんでした。";
                                break;
                            }
                            break;
                        case "/opt":
                            if (int.TryParse(chats[1],out int k))
                            {

                                OptionTeamCrew.SetPlayers(k);
                                addchat += $"optplayers sets {k}";
                            }
                            else
                            {
                                addchat += $"optplayers_set is faild";

                            }
                                break;

                    }
                }
                if (addchat != "")
                {
                    Helper.AddChat(addchat);
                }
                if (rpcaddchat != "")
                {
                    Helper.AllAddChat(addchat);
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
