using AmongUs.GameOptions;
using System.Text.RegularExpressions;
using TSR.Game;
using TSR.Game.Options.OptionControlUI;
using TSR.Game.Role;

namespace TSR.Patch.Command;

[SmartPatch(typeof(ChatController), nameof(ChatController.SendChat))]
public static class ChatControllerPatch
{
    public static bool Prefix(ChatController __instance)
    {
        if (__instance.timeSinceLastMessage > 3f)
        {
            KeyCommands.Chattexts.Add(__instance.freeChatField.textArea.text);
            KeyCommands.Undocount = KeyCommands.Chattexts.Count;
        }
        return AddChat(__instance, __instance.freeChatField.textArea.text);
    }

    public static bool AddChat(ChatController __instance, string chat)
    {
        string[] chats = chat.Split(' ');
        if (!chat.StartsWith("/"))
        {
            return true;
        }
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
                //string freeplaykeycommands =
                //    "<b><color=#f5f512><size=110%>フリープレイ限定</size></color></b>\n" +
                //    " F1 : 役職リストを表示 \n" +
                //    " F2 : プレイヤーリストを表示 \n";
                //if(FGameManager.NetworkMode() == NetworkModes.FreePlay)
                //{
                //    chatcommands += freeplaychatcommands;
                //    //keycommands += freeplaykeycommands;
                //}
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
                    addchat += "文字数は1字以上にしてください";
                }
                break;

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
            case "/fakelevel":
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
            case "//":
                OptionUIManager.Create();
                //Resources.FindObjectsOfTypeAll(Il2CppType.Of<MeetingHud>()).Select(x => ((GameObject)x).GetComponent<MeetingHud>().meetingContents.FindChild("PhoneUI").GetComponent<SpriteRenderer>().sprite).ToList()[0];
                break;
        }
        if (FGameManager.NetworkMode() == NetworkModes.FreePlay)
        {
            switch (chats[0])
            {

                case "/setrole":
                    // /setrole [id] [roleId]
                    if (int.TryParse(chats[1], out int q))
                    {
                        
                        if (TeamBaseRegister.TeamBaseTypeMap.ContainsKey(chats[2]))
                        {
                            
                            RoleAssigner.RpcChangeRole(FPlayerControl.AllPlayer[q],RoleBase.GetRoleBase(chats[2]));
                            addchat += $"[{FPlayerControl.AllPlayer[q].PlayerName}] を [{chats[2]}] に設定したよ";
                        }
                        else
                        {
                            addchat += chats[2] + " はrolelistに含まれてないかも..";
                        }
                    }
                    else
                    {
                        addchat += chats[1] + " はplayerlistに含まれてないかも..";
                    }
                    break;
                case "/rolelist":
                    addchat += "役職一覧 \n";
                    foreach(var k in RoleBaseRegister.RoleBaseTypeMap)
                    {
                        var rolebase = RoleBase.GetRoleBase(k.Key);
                        addchat += k.Key + " : " +Helper.ColoredText( rolebase.RoleColor(),rolebase.RoleName) + " \n";
                    }
                    break;
                case "/playerlist":
                    addchat += "役職一覧 \n";
                    foreach (var k in FPlayerControl.AllPlayer)
                    {
                        addchat += k.PlayerId + " : " + Helper.ColoredText(k.TeamColor,k.PlayerName) + " \n";
                    }
                    break;
                case "/trans":
                    string transchat = chat.Substring(7);
                    addchat += Helper.ColoredText(Helper.ColorFromColorcode(0xfa4f5a), "[翻訳] ") + Translation.Get(transchat);
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
}

[SmartPatch(typeof(HudManager), "Update")]
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