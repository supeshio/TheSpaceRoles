using HarmonyLib;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.OnGameStart))]
    public static class RoleTextManager
    {
        public static TextMeshPro RoleText;
        public static void Postfix(PlayerControl __instance)
        {
            if (AmongUsClient.Instance.ClientId != __instance.Data.ClientId) return;
            //if (AmongUsClient.Instance.AmHost) DataBase.AllPlayerControls().Do(x => x.RpcSetRole(RoleTypes.Crewmate));

            //DataBase.AllPlayerControls().First(x => x.PlayerId == i).cosmetics.nameText.text = t + $"\n <size=80%>{string.Join("×", rolemaster.Select(x => x.ColoredRoleName()))}";
            var d = __instance.cosmetics.nameText;
            GameObject gameObject = new("roletext");
            RoleText = gameObject.AddComponent<TextMeshPro>();
            RoleText.transform.SetParent(d.transform.parent);
            RoleText.transform.localPosition = new Vector3(d.transform.localPosition.x, d.transform.localPosition.y + 0.25f, d.transform.localPosition.z);
            RoleText.alignment = TextAlignmentOptions.Center;
            RoleText.tag = d.tag;
            RoleText.fontSize = d.fontSize;
            RoleText.sortingOrder = d.sortingOrder;
            RoleText.sortingLayerID = d.sortingLayerID;
            RoleText.transform.localScale = Vector3.one;
            RoleText.m_sharedMaterial = d.m_sharedMaterial;
            RoleText.fontStyle = d.fontStyle;

            TheSpaceRoles.RoleTextManager.RoleText.text = $"<size=85%>{string.Join("</color>×", DataBase.AllPlayerRoles[__instance.PlayerId].Select(x => x.ColoredRoleName)/*+"</color>"*/)}";


            foreach ((int i, CustomRole[] rolemaster) in DataBase.AllPlayerRoles)
            {
                //var t = DataBase.AllPlayerControls().First(x => x.PlayerId == i).cosmetics.nameText.text;

                if (i == PlayerControl.LocalPlayer.PlayerId)
                {

                }
            }

        }
    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    public static class Roletexthudstart
    {
        public static void Postfix(PlayerControl __instance)
        {
            if (TheSpaceRoles.RoleTextManager.RoleText != null)
            {

                if (AmongUsClient.Instance.ClientId != __instance.Data.ClientId) return;
                TheSpaceRoles.RoleTextManager.RoleText.text = $"<size=85%>{string.Join("</color>×", DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Select(x => x.ColoredRoleName)/*+"</color>"*/)}";

            }
        }
    }
}
