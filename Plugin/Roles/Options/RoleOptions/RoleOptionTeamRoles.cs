using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class RoleOptionTeamRoles
    {

        public static List<RoleOptionTeamRoles> RoleOptionsInTeam = new();
        public GameObject @object;
        public TextMeshPro Title_TMP;
        public bool Virtual;
        public Roles role;
        public Teams team;
        public RoleOptionTeamRoles(RoleOptionTeams teams, Roles role,bool Virtual = false)
        {
            this.role = role;
            this.team = teams.teams;
            this.Virtual = Virtual;
            @object = new(team.ToString()+"_"+role.ToString());
            @object.transform.parent = HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("AddedRoles");

            @object.transform.localPosition = new(0f, 2f - (RoleOptionsInTeam.Count  * -0.3f), -1f);
            @object.layer = Data.UILayer;

            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.team_role_banner.png",400); 
            renderer.color =Virtual ? ColorEditHSV(GetLink.ColorFromTeams[teams.teams], a: 0.5f) : GetLink.ColorFromTeams[teams.teams];
            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.parent = @object.transform;
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = role.ToString();
            Color color = Virtual?ColorEditHSV(GetLink.GetCustomRole(role).Color,a:0.5f):GetLink.GetCustomRole(role).Color;
            Title_TMP.color = GetLink.GetCustomRole(role).Color;
            Title_TMP.fontSize = Title_TMP.fontSizeMax = 2f;
            Title_TMP.fontSizeMin = 1f;
            Title_TMP.alignment = TextAlignmentOptions.Center;
            Title_TMP.enableWordWrapping = false;
            Title_TMP.outlineWidth = 0.8f;
            Title_TMP.autoSizeTextContainer = false;
            Title_TMP.enableAutoSizing = true;
            Title_TMP.transform.localPosition = new Vector3(0f, 0f, -1f);
            Title_TMP.transform.localScale = Vector3.one;
            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
            Title_TMP.m_sharedMaterial = Data.textMaterial;
            Title_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Title_TMP.rectTransform.sizeDelta = new Vector2(2.4f, 0.5f);

        }
    }
}
