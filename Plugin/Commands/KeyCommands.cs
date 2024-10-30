using HarmonyLib;
using InnerNet;
using System.Collections.Generic;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public class KeyCommands
    {
        [HarmonyPatch(typeof(ShipStatus), "Start")]
        public static class Resetundocount
        {
            public static void Prefix()
            {
                chattexts.Clear();
                undocount = 1;
            }
        }

        public static int undocount = 1;

        public static List<string> chattexts = new();
        //[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        //[HarmonyPostfix]
        //public static void PlayerPostfix()
        //{

        //    //Debug
        //    //if (TSR.DebugMode.Value && Input.GetKeyDown(KeyCode.N))
        //    //{
        //    //    Logger.Info("N");
        //    //    KillAnimationPatch.AnimCancel = true;
        //    //    HudManager.Instance.KillOverlay.ShowKillAnimation(PlayerControl.AllPlayerControls[Helper.Random(0, PlayerControl.AllPlayerControls.Count - 1)].Data, PlayerControl.LocalPlayer.Data);
        //    //}
        //}



        [HarmonyPatch(typeof(GameManager), "FixedUpdate")]
        [HarmonyPostfix]
        public static void Postfix(GameManager __instance)
        {
            try
            {
                if (((InnerNetClient)AmongUsClient.Instance).AmHost && (int)((InnerNetClient)AmongUsClient.Instance).GameState == 2)
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
                if ((int)((InnerNetClient)AmongUsClient.Instance).GameState != 2 || (int)((InnerNetClient)AmongUsClient.Instance).NetworkMode == 2)
                {
                    if (PlayerControl.LocalPlayer?.Collider?.offset != null)


                        if (Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)305))
                        {
                            PlayerControl.LocalPlayer.Collider.offset = new Vector2(0f, 1000f);
                        }
                        else
                        {
                            PlayerControl.LocalPlayer.Collider.offset = new Vector2(0f, -0.3636f);
                        }

                }

            }
            catch
            {
            }
        }

        [HarmonyPatch(typeof(GameStartManager), "Update")]
        [HarmonyPostfix]
        public static void GameStartAndCancel(GameStartManager __instance)
        {
            try
            {
                if (((InnerNetClient)AmongUsClient.Instance).AmHost)
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
            }
        }

        [HarmonyPatch(typeof(ChatController), "Update")]
        [HarmonyPostfix]
        public static void Command_(ChatController __instance)
        {
            try
            {
                if (((Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)122)) || Input.GetKeyDown((KeyCode)273)) && undocount > 0)
                {
                    undocount--;
                    __instance.freeChatField.textArea.SetText(chattexts[undocount], "");
                }
                if (((Input.GetKey((KeyCode)306) && Input.GetKeyDown((KeyCode)121)) || Input.GetKeyDown((KeyCode)274)) && undocount < chattexts.Count - 1)
                {
                    undocount++;
                    __instance.freeChatField.textArea.SetText(chattexts[undocount], "");
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
            }
        }
    }
}
