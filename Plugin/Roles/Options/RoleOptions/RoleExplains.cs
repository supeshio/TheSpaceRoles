using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{
    public enum SelectingType
    {
        None,
        Team,
        Role,
        AddedRole,

    }
    public static class RoleOptionsDescription
    {
        public static TextMeshPro Title;//役職名/陣営名
        public static TextMeshPro Intro;//intro
        public static TextMeshPro LeftText;
        public static TextMeshPro RightText;
        public static TextMeshPro Description;
        public static void StartExplain()
        {
            /*Added:
             * 陣営 /TA
             * 役職 /RA
             * 勝利条件
             * 占い結果()
             * 霊媒結果()
             * 能力
             */

            if (Description != null) return;

            GameObject g = new GameObject("Description");
            g.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings"));
            g.transform.localPosition = Vector3.zero;

            Description = new GameObject("Description").AddComponent<TextMeshPro>();
            Description.gameObject.layer = Data.UILayer;
            Description.text = $"\r\n{new Sheriff().ColoredRoleName}はキルボタンを持ち、{Helper.ColoredText(GetLink.ColorFromTeams[Teams.Impostor],Translation.GetString("team.impostor.name"))}や{Helper.ColoredText(GetLink.GetOtherRolesColor, Translation.GetString("team.other.name"))}をキルすることができる。\r\nしかし誤って{Helper.ColoredText(GetLink.ColorFromTeams[Teams.Crewmate], Translation.GetString("team.crewmate.name"))}をキルしようとしてしまうと自爆してしまう。\r\n怪しい人をキルして{Helper.ColoredText(GetLink.ColorFromTeams[Teams.Crewmate], Translation.GetString("team.crewmate.name"))}を勝利に導こう。";
            Description.transform.SetParent(g.transform);
            Description.transform.localPosition = new Vector3(2.4f,1.2f, 0);
            Description.m_sharedMaterial = Data.textMaterial;
            Description.rectTransform.pivot = new Vector2(0.5f, 1f);
            Description.rectTransform.sizeDelta = new Vector2(5f, 5f);
            Description.fontSize = 1.2f;
            Description.fontSizeMax = 2f;
            Description.fontSizeMin = 1.0f;
            Description.alignment = TextAlignmentOptions.Top;
            Description.enableWordWrapping = true;
            Description.autoSizeTextContainer = false;
            Description.enableAutoSizing = false;

            Title = new GameObject("Title").AddComponent<TextMeshPro>();
            Title.gameObject.layer = Data.UILayer;
            Title.text ="シェリフ/クルーメイト";
            Title.transform.SetParent(g.transform);
            Title.transform.localPosition = new Vector3(2.4f, 2f, 0);
            Title.m_sharedMaterial = Data.textMaterial;
            Title.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Title.rectTransform.sizeDelta = new Vector2(5f, 1f);
            Title.fontSize = 4f;
            Title.fontSizeMax =10f;
            Title.fontSizeMin = 1.0f;
            Title.alignment = TextAlignmentOptions.Center;
            Title.enableWordWrapping = true;
            Title.autoSizeTextContainer = false;
            Title.enableAutoSizing = false;


            LeftText = new GameObject("LeftText").AddComponent<TextMeshPro>();
            LeftText.gameObject.layer = Data.UILayer;
            LeftText.text = "占い結果:白";
            LeftText.transform.SetParent(g.transform);
            LeftText.transform.localPosition = new Vector3(1.4f,1.2f, 0);
            LeftText.m_sharedMaterial = Data.textMaterial;
            LeftText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            LeftText.rectTransform.sizeDelta = new Vector2(2.5f, 1.2f);
            LeftText.fontSize = 1.4f;
            LeftText.fontSizeMax = 10f;
            LeftText.fontSizeMin = 1.0f;
            LeftText.alignment = TextAlignmentOptions.Center;
            LeftText.enableWordWrapping = true;
            LeftText.autoSizeTextContainer = false;
            LeftText.enableAutoSizing = false;


            RightText = new GameObject("RightText").AddComponent<TextMeshPro>();
            RightText.gameObject.layer = Data.UILayer;
            RightText.text = "霊媒結果:白";
            RightText.transform.SetParent(g.transform);
            RightText.transform.localPosition = new Vector3(3.4f, 1.2f, 0);
            RightText.m_sharedMaterial = Data.textMaterial;
            RightText.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            RightText.rectTransform.sizeDelta = new Vector2(2.5f, 1f);
            RightText.fontSize = 1.4f;
            RightText.fontSizeMax = 10f;
            RightText.fontSizeMin = 1.0f;
            RightText.alignment = TextAlignmentOptions.Center;
            RightText.enableWordWrapping = true;
            RightText.autoSizeTextContainer = false;
            RightText.enableAutoSizing = false;

            Intro = new GameObject("Intro").AddComponent<TextMeshPro>();
            Intro.gameObject.layer = Data.UILayer;
            Intro.text = "正義の鉄槌を";
            Intro.transform.SetParent(g.transform);
            Intro.transform.localPosition = new Vector3(2.4f, 1.5f, 0);
            Intro.m_sharedMaterial = Data.textMaterial;
            Intro.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Intro.rectTransform.sizeDelta = new Vector2(2.5f, 1f);
            Intro.fontSize = 2.8f;
            Intro.fontSizeMax = 10f;
            Intro.fontSizeMin = 1.0f;
            Intro.alignment = TextAlignmentOptions.Center;
            Intro.enableWordWrapping = true;
            Intro.autoSizeTextContainer = false;
            Intro.enableAutoSizing = false;
        }

        public static SelectingType selecting = SelectingType.None;
        public static RoleOptionTeams selectedTeam;
        public static RoleOptions selectedRole;
        public static RoleOptionTeamRoles selectedAddedRole;
        public static string GetExplaination()
        {
            return selecting switch
            {
                SelectingType.None => string.Empty,
                SelectingType.Team => Translation.GetString("team."+selectedTeam.teams.ToString()+".description"),
                SelectingType.Role => Translation.GetString("role." + selectedRole.ToString() + ".description"),
                SelectingType.AddedRole => Translation.GetString("role." + selectedAddedRole.role.ToString() + ".description"),
                _ => string.Empty

            };
        }
        public static void Set(RoleOptions select)
        {
            selectedRole = select;
            selecting = SelectingType.Role;
            
        }
        public static void Set(RoleOptionTeams select)
        {
            selectedTeam = select;
            selecting = SelectingType.Team;
        }
        public static void Set(RoleOptionTeamRoles select)
        {
            selectedAddedRole = select;
            selecting = SelectingType.Team;
        }
        public static void Reset()
        {
            selectedRole = null;
            selectedTeam = null;
            selectedRole = null;
            selecting = SelectingType.None;
        }
    }
}
