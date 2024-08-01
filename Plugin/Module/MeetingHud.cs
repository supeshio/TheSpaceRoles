using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class MeetingHudPatch
    {
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Update))]
        public static class MeetingHudUpdate
        {
            public static void Prefix(MeetingHud __instance)
            {
                var players = __instance.playerStates.ToList();
                foreach (var player in players)
                {
                }
            }
        }
        [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
        public static class MeetingHudAwake
        {
            public static void Prefix(MeetingHud __instance)
            {
                var players = __instance.playerStates.ToList();
                foreach (var player in players)
                {
                    var vec = player.transform.position;
                    player.ColorBlindName.transform.position = vec + new Vector3(-1.0f, -0.2f, -1);
                    if (player.transform.FindChild("roletext") != null) continue;
                    GameObject gameObject = new("roletext");
                    TextMeshPro RoleText = gameObject.AddComponent<TextMeshPro>();
                    RoleText.transform.SetParent(player.NameText.transform.parent);
                    RoleText.transform.position = vec + new Vector3(0.305f, -0.18f, -1);
                    RoleText.alignment = TextAlignmentOptions.Center;
                    RoleText.tag = player.NameText.tag; RoleText.fontSizeMin = 0.1f;
                    RoleText.fontSize = RoleText.fontSizeMax = 1.6f;
                    gameObject.layer = Data.UILayer;
                    RoleText.enableWordWrapping = false;
                    RoleText.outlineWidth = 0.8f;
                    RoleText.autoSizeTextContainer = false;
                    RoleText.enableAutoSizing = true;
                    RoleText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    RoleText.rectTransform.sizeDelta = new Vector2(1.5f, 1f);
                    RoleText.sortingOrder = player.NameText.sortingOrder;
                    RoleText.sortingLayerID = player.NameText.sortingLayerID;
                    RoleText.text = $"{string.Join("</color>×", DataBase.AllPlayerRoles[player.TargetPlayerId].Select(x => x.ColoredRoleName)/*+"</color>"*/)}";
                    RoleText.m_sharedMaterial = Data.textMaterial;
                    RoleText.fontStyle = FontStyles.Bold;
                    Logger.Info($"{string.Join("</color>×", DataBase.AllPlayerRoles[player.TargetPlayerId].Select(x => x.ColoredRoleName)/*+"</color>"*/)}");
                    gameObject.SetActive(true);
                    if (!PlayerControl.LocalPlayer.Data.IsDead)
                    {
                        if (PlayerControl.LocalPlayer.PlayerId == player.TargetPlayerId)
                        {
                            player.NameText.color = DataBase.AllPlayerRoles[player.TargetPlayerId][0].Color;
                            gameObject.SetActive(true);
                        }
                        else
                        {
                            player.NameText.color = Color.white;
                            gameObject.SetActive(false);
                        }
                    }
                    else
                    {

                        player.NameText.color = DataBase.AllPlayerRoles[player.TargetPlayerId][0].Color;
                        gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
