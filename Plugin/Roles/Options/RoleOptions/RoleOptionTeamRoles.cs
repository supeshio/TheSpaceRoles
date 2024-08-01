using Il2CppMicrosoft.Reflection;
using Il2CppSystem;
using Il2CppSystem.Dynamic.Utils;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class RoleOptionTeamRoles:MonoBehaviour
    {

        public static List<RoleOptionTeamRoles> RoleOptionsInTeam = new();
        public GameObject @object;
        public TextMeshPro Title_TMP;
        public bool Virtual;
        public Roles role;
        public Teams team;
        public int num = 0;
        public PassiveButton AddedRoleButton;
        public RoleOptionTeamRoles(RoleOptionTeams teams, Roles role, bool Virtual = false)
        {
            this.role = role;
            this.team = teams.teams;
            this.Virtual = Virtual;
            @object = new(team.ToString() + "_" + role.ToString());
            @object.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("AddedRoles"));
            //var op = @object.AddComponent<RoleOptionBehavior>();
            //op.Set(role, team);
            //optionBehavior.Set(teams: team);
            @object.transform.localPosition = new(0.6f, 2f + (RoleOptionsInTeam.Count * -0.36f), 1000f);
            @object.layer = Data.UILayer;


            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.team_role_banner.png", 400);
            if (GetLink.ColorFromTeams.ContainsKey(teams.teams))
            {

                renderer.color = GetLink.ColorFromTeams[teams.teams];
            }
            else
            {

                renderer.color = Color.yellow;
            }


            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.SetParent(@object.transform);
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = GetLink.GetCustomRole(role).RoleName;
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
            var box = @object.AddComponent<BoxCollider2D>();
            box.size = renderer.bounds.size;
            AddedRoleButton = @object.gameObject.AddComponent<PassiveButton>();
            AddedRoleButton.OnClick = new();
            AddedRoleButton.OnMouseOut = new UnityEvent();
            AddedRoleButton.OnMouseOver = new UnityEvent();
            AddedRoleButton._CachedZ_k__BackingField = 0.1f;
            AddedRoleButton.CachedZ = 0.1f;
            AddedRoleButton.Colliders = new[] { @object.GetComponent<BoxCollider2D>() };
            AddedRoleButton.OnClick.AddListener((System.Action)(() =>
            {
                RoleOptionsDescription.Set(this);
            }));

            AddedRoleButton.OnMouseOver.AddListener((System.Action)(() =>
            {
                if (GetLink.ColorFromTeams.ContainsKey(team))
                {
                    renderer.color = Helper.ColorEditHSV(GetLink.ColorFromTeams[team], s: -0.2f);
                }
            }));
            AddedRoleButton.OnMouseOut.AddListener((System.Action)(() =>
            {
                if (GetLink.ColorFromTeams.ContainsKey(team))
                {
                    renderer.color = GetLink.ColorFromTeams[team];
                }
            }));
            AddedRoleButton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
            AddedRoleButton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;


        }
        public void SetPos(float num)
        {

            @object.transform.localPosition = new(-2.0f, 2f - 0.36f * num, -1f);
        }
        public void Remove()
        {
            UnityEngine.Object.Destroy(@object);

            RoleOptionsInTeam.Remove(this);
        }
        public void Dragging()
        {

            var renderer = @object.GetComponent<SpriteRenderer>();
            Color color = GetLink.ColorFromTeams.ContainsKey(team) ? GetLink.ColorFromTeams[team] : Color.clear;

            var drag = RoleOptionsHolder.roleOptions.First(x => x.MouseHolding).roles;
            if (!GetLink.GetCustomRole(drag).teamsSupported.Contains(team))
            {

                color = ColorEditHSV(color, v: -0.6f);
            }
            renderer.color = color;
        }
    }
}
