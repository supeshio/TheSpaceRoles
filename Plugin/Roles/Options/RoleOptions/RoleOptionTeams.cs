using Steamworks;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{
    public class RoleOptionTeams
    {
        public Teams teams;
        public GameObject @object;
        public TextMeshPro Title_TMP;
        public float num;
        public RoleOptionTeams(Teams teams,int num)
        {
            this.num = num;
            this.teams = teams;
            @object = new GameObject(teams.ToString())
            {
                active = true
            };
            @object.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("Teams"));
            RoleOptionsManager.SetNums();
            @object.transform.localScale = Vector3.one;
            @object.layer = HudManager.Instance.gameObject.layer;
            @object.tag = "teams";
            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.team_banner_top.png", 400);
            if(GetLink.ColorFromTeams.ContainsKey(teams))
            {
                renderer.color = GetLink.ColorFromTeams[teams];

            }
            else
            {
                renderer.color = Color.clear;
            }


            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.SetParent(@object.transform);
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = Translation.GetString("team."+teams.ToString()+".name");
            Title_TMP.color = Color.white;
            Title_TMP.fontSize = Title_TMP.fontSizeMax = 2f;
            Title_TMP.fontSizeMin = 1f;
            Title_TMP.alignment = TextAlignmentOptions.Center;
            Title_TMP.enableWordWrapping = false;
            Title_TMP.outlineWidth = 0.8f;
            Title_TMP.autoSizeTextContainer = false;
            Title_TMP.enableAutoSizing = true;
            Title_TMP.transform.localPosition = new Vector3(0f, 0f, -1);
            Title_TMP.transform.localScale = Vector3.one;
            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
            Title_TMP.m_sharedMaterial = Data.textMaterial;
            Title_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Title_TMP.rectTransform.sizeDelta = new Vector2(2.4f, 0.5f);
            var box = @object.AddComponent<BoxCollider2D>();
            box.size = renderer.bounds.size;
        }
        public void SetPos(float num)
        {
            this.num = num;

            @object.transform.localPosition = new(-2.0f, 2f - 0.36f * num, 0);
        }
        public void Dragging()
        {

            var renderer = @object.GetComponent<SpriteRenderer>();
            Color color = GetLink.ColorFromTeams.ContainsKey(teams) ? GetLink.ColorFromTeams[teams] : Color.clear;
            
            var drag = RoleOptionsHolder.roleOptions.First(x => x.MouseHolding).roles;
            if (!GetLink.GetCustomRole(drag).teamsSupported.Contains(teams))
            {

                color = Helper.ColorEditHSV(color, v: -0.6f);
            }
            renderer.color = color;
        }
        public static void Drag()
        {
            try
            {

                if (RoleOptions.DragMode)
                {
                    foreach (var item in RoleOptionTeamsHolder.TeamsHolder)
                    {
                        item.Dragging();
                    }
                    foreach (var item in RoleOptionTeamRoles.RoleOptionsInTeam)
                    {
                        item.Dragging();
                    }
                }
                else
                {
                    foreach (var item in RoleOptionTeamsHolder.TeamsHolder)
                    {
                        var renderer = item.@object.GetComponent<SpriteRenderer>();
                        renderer.color = GetLink.ColorFromTeams.ContainsKey(item.teams) ? GetLink.ColorFromTeams[item.teams] : Color.clear;
                    }
                    foreach (var item in RoleOptionTeamRoles.RoleOptionsInTeam)
                    {
                        var renderer = item.@object.GetComponent<SpriteRenderer>();
                        renderer.color = GetLink.ColorFromTeams.ContainsKey(item.team) ? GetLink.ColorFromTeams[item.team] : Color.clear;
                    }
                }
            }
            catch
            {

            }

        }
        public void VirtualAddRole(Roles roles)
        {/*
            RoleOptionsInTeam.DoIf(x=>x.Virtual,z => {GameObject.Destroy(z.@object); RoleOptionsInTeam.RemoveAll(y=>y.@object.name==z.@object.name); });
            RoleOptionsInTeam.Add(new(this,roles,Virtual:true));*/
        }
    }
}
