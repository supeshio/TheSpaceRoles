using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnEnable))]
    public static class RoleTextManager
    {
        public static TextMeshPro RoleText;
        public static void Postfix(PlayerControl __instance)
        {

            if (PlayerControl.LocalPlayer?.PlayerId == null) return;
            //if (DataBase.AllPlayerRoles == null || !DataBase.AllPlayerRoles.ContainsKey(PlayerControl.LocalPlayer.PlayerId)) return;
            if (RoleText?.text != null) return; 
            //if (AmongUsClient.Instance.AmHost) DataBase.AllPlayerControls().Do(x => x.RpcSetRole(RoleTypes.Crewmate));

            //DataBase.AllPlayerControls().First(x => x.PlayerId == i).cosmetics.nameText.text = t + $"\n <size=80%>{string.Join("×", rolemaster.Select(x => x.ColoredRoleName()))}";
            var d = PlayerControl.LocalPlayer.cosmetics.nameText;
            GameObject gameObject = new("roletext");
            RoleText = gameObject.AddComponent<TextMeshPro>();
            RoleText.transform.SetParent(PlayerControl.LocalPlayer.transform);
            RoleText.transform.localPosition = new Vector3(0, 1.25f, 0);
            RoleText.alignment = TextAlignmentOptions.Center;
            RoleText.tag = d.tag;
            RoleText.fontSize = d.fontSize;
            RoleText.sortingOrder = d.sortingOrder;
            RoleText.sortingLayerID = d.sortingLayerID;
            RoleText.transform.localScale = Vector3.one;
            RoleText.m_sharedMaterial = d.m_sharedMaterial;
            RoleText.fontStyle = d.fontStyle;
            RoleText.fontSizeMin=RoleText.fontSizeMax = RoleText.fontSize = 2f;

            //RoleText.text = DataBase.AllPlayerRoles[__instance.PlayerId][0].ColoredRoleName;



        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class Roletexthudstart
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (TheSpaceRoles.RoleTextManager.RoleText != null)
            {

                if (PlayerControl.LocalPlayer.PlayerId!=__instance.PlayerId) return;
                TheSpaceRoles.RoleTextManager.RoleText.text = DataBase.AllPlayerRoles[__instance.PlayerId].ColoredRoleName;
            }
        }
    }
}
