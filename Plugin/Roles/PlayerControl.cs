using HarmonyLib;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
    //[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnEnable))]
    public static class RoleTextManager
    {
        public static Dictionary<int,TextMeshPro> RoleTexts=[];
        public static void Postfix(PlayerControl __instance)
        {

            //if (PlayerControl.LocalPlayer?.PlayerId == null) return;
            //if (DataBase.AllPlayerRoles == null || !DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId)) return;
            //if (AmongUsClient.Instance.AmHost) DataBase.AllPlayerControls().Do(x => x.RpcSetRole(RoleTypes.Crewmate));

            //DataBase.AllPlayerControls().First(x => x.PlayerId == i).cosmetics.nameText.text = t + $"\n <size=80%>{string.Join("×", rolemaster.Select(x => x.ColoredRoleName()))}";
            if (RoleTexts.ContainsKey(__instance.PlayerId))
            {
                GameObject.Destroy(RoleTexts[__instance.PlayerId]);
            }
            var d = __instance.cosmetics.nameText;
            GameObject gameObject = new("roletext");
            TextMeshPro RoleText = gameObject.AddComponent<TextMeshPro>();
            RoleText.transform.SetParent(__instance.transform);
            RoleText.transform.localPosition = new Vector3(0, 1.25f, 0);
            RoleText.alignment = TextAlignmentOptions.Center;
            RoleText.tag = d.tag;
            RoleText.fontSize = d.fontSize;
            RoleText.sortingOrder = d.sortingOrder;
            RoleText.sortingLayerID = d.sortingLayerID;
            RoleText.transform.localScale = Vector3.one;
            RoleText.m_sharedMaterial = d.m_sharedMaterial;
            RoleText.fontStyle = d.fontStyle;
            RoleText.fontSizeMin = RoleText.fontSizeMax = RoleText.fontSize = 2f;
            if(RoleTexts.ContainsKey(__instance.PlayerId))
            {
                RoleTexts[__instance.PlayerId] = RoleText;
            }
            else
            {
                RoleTexts.Add(__instance.PlayerId, RoleText);
            }

            //RoleText.text = DataBase.AllPlayerRoles[__instance.PlayerId][0].ColoredRoleName;



        }
        public static void TextChange(int playerId)
        {
            PlayerControl pc = Helper.GetPlayerById(playerId);
            if (!RoleTextManager.RoleTexts.ContainsKey(pc.PlayerId)) return;
            if (PlayerControl.LocalPlayer?.PlayerId == null) return;
            try
            {
                if (RoleTextManager.RoleTexts?[pc.PlayerId] != null)
                {
                    if (PlayerControl.LocalPlayer.PlayerId != pc.PlayerId && AmongUsClient.Instance.NetworkMode != NetworkModes.FreePlay) return;
                    RoleTextManager.RoleTexts[pc.PlayerId].text = DataBase.AllPlayerRoles[pc.PlayerId].ColoredRoleName;



                }
            }
            catch {}

        }
    }
}
