using InnerNet;
using System.Collections.Generic;
using TSR.Game.Options.OptionControlUI;
using UnityEngine;
using static AmongUsClient;

namespace TSR.Patch.Command
{
    [SmartPatch]
    public class KeyCommands
    {
        /// <summary>
        /// 使われています!!!!!!!!!!
        /// </summary>
        [SmartPatch(typeof(ShipStatus), "Start")]
        public static class Resetundocount
        {
            public static void Prefix()
            {
                Chattexts.Clear();
                Undocount = 1;
            }
        }

        public static int Undocount = 1;

        public static List<string> Chattexts = [];


        [SmartPatch(typeof(GameManager), "FixedUpdate")]
        public static void Postfix(GameManager __instance)
        {
            try
            {
                if (Instance.AmHost && (int)Instance.GameState == 2)
                {
                    if (Input.GetKey((KeyCode)304) && Input.GetKey((KeyCode)303) && Input.GetKey((KeyCode)104))
                    {
                        __instance.enabled = false;
                        __instance.RpcEndGame((GameOverReason)3, false);
                        __instance.RpcEndGame((GameOverReason)8, false);
                        Logger.Info("廃村処理", "", "Postfix");
                    }
                    if (Input.GetKey((KeyCode)304) && Input.GetKey((KeyCode)303) && Input.GetKey((KeyCode)115))
                    {
                        MeetingHud.Instance.RpcClose();
                    }
                }

                if ((int)Instance.GameState == 2 && (int)Instance.NetworkMode != 2)
                {
                    return;
                }

                if (PlayerControl.LocalPlayer?.Collider == null) return;

                PlayerControl.LocalPlayer.Collider.enabled = !(Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)305));
            }
            catch
            {
                // ignored
            }
        }

        public static void DefaultCommands()
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                OptionUIManager.ShowHide();
            }
        }

        [SmartPatch(typeof(GameStartManager), "Update"), SmartPostfix]
        public static void GameStartAndCancel(GameStartManager __instance)
        {
            try
            {
                if (((InnerNetClient)Instance).AmHost)
                {
                    if (Input.GetKey((KeyCode)118))
                    {
                        __instance.countDownTimer = 0f;
                    }
                    if (Input.GetKey((KeyCode)99))
                    {
                        __instance.ResetStartState();
                    }
                }
            }
            catch
            {
                // ignored
            }
        }

        [SmartPatch(typeof(ChatController), "Update"), SmartPostfix]
        public static void Command_(ChatController __instance)
        {
            try
            {
                if (((Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)122)) || Input.GetKeyDown((KeyCode)273)) && Undocount > 0)
                {
                    Undocount--;
                    __instance.freeChatField.textArea.SetText(Chattexts[Undocount], "");
                }
                if (((Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)121)) || Input.GetKeyDown((KeyCode)274)) && Undocount < Chattexts.Count - 1)
                {
                    Undocount++;
                    __instance.freeChatField.textArea.SetText(Chattexts[Undocount], "");
                }
                if (Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)118))
                {
                    if (Input.GetKey((KeyCode)304))
                    {
                        Helper.AllAddChat(GUIUtility.systemCopyBuffer);
                    }
                    else
                    {
                        __instance.freeChatField.textArea.SetText(__instance.freeChatField.textArea.text + GUIUtility.systemCopyBuffer, "");
                    }
                }
                if (Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)120))
                {
                    GUIUtility.systemCopyBuffer = __instance.freeChatField.textArea.text;
                    ((AbstractChatInputField)__instance.freeChatField).Clear();
                }
                if (Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)99))
                {
                    GUIUtility.systemCopyBuffer = __instance.freeChatField.textArea.text;
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}