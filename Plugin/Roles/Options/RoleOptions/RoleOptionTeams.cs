using Steamworks;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem.PlaybackState;

namespace TheSpaceRoles
{
    public class RoleOptionTeams
    {
        public bool isEnable = true;
        public Teams teams;
        public GameObject @object;
        public TextMeshPro Title_TMP;
        public float num;
        public SpriteRenderer DropDown;
        public PassiveButton DropDownButton;
        public PassiveButton TeamButton;
        //public RoleOptionBehavior behavior;
        public RoleOptionTeams(Teams teams, int num)
        {
            this.num = num;
            this.teams = teams;
            @object = new GameObject("RoleOptionTeams-" + teams.ToString())
            {
                active = true
            };
            @object.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("Teams"));
            RoleOptionsManager.SetNums();
            @object.transform.localScale = Vector3.one;
            @object.layer = HudManager.Instance.gameObject.layer;
            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.team_banner_top.png", 400);
            if (GetLink.ColorFromTeams.ContainsKey(teams))
            {
                renderer.color = GetLink.ColorFromTeams[teams];

            }
            else
            {
                renderer.color = Color.clear;
            }


            //behavior = new GameObject("Title_TMP").AddComponent<RoleOptionBehavior>();
            //behavior.transform.SetParent(@object.transform);
            //behavior.Set(teams);

            /*var v = new GameObject("RolOp");
            v.transform.SetParent(@object.transform);
            var  _behavior = v.AddComponent<RoleOptionBehavior>();
            _behavior.Set(teams,Roles.None);
            */
            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.SetParent(@object.transform);
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = Translation.GetString("team." + teams.ToString() + ".name");
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
            box.size = renderer.bounds.size+new Vector3(0,0.05f,0);
            TeamButton = @object.gameObject.AddComponent<PassiveButton>();
            TeamButton.OnClick = new();
            TeamButton.OnMouseOut = new UnityEvent();
            TeamButton.OnMouseOver = new UnityEvent();
            TeamButton._CachedZ_k__BackingField = 0.1f;
            TeamButton.CachedZ = 0.1f;
            TeamButton.Colliders = new[] { @object.GetComponent<BoxCollider2D>() };
            TeamButton.OnClick.AddListener((System.Action)(() =>
            {
                RoleOptionsDescription.Set(this);
            }));

            TeamButton.OnMouseOver.AddListener((System.Action)(() =>
            {
                if (GetLink.ColorFromTeams.ContainsKey(teams))
                {
                    renderer.color = Helper.ColorEditHSV(GetLink.ColorFromTeams[teams],s:-0.2f);
                }
            }));
            TeamButton.OnMouseOut.AddListener((System.Action)(() =>
            {
                if (GetLink.ColorFromTeams.ContainsKey(teams))
                {
                    renderer.color = GetLink.ColorFromTeams[teams];
                }
            }));
            TeamButton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
            TeamButton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;







            DropDown = new GameObject("DropDown").AddComponent<SpriteRenderer>();
            DropDown.transform.localPosition = new(1.1f, 0, -1);
            DropDown.sprite = Sprites.GetSpriteFromResources("ui.arrow_drop_down.png",50);
            DropDown.color = Palette.White;
            DropDown.material =Data.textMaterial;
            DropDown.gameObject.layer = HudManager.Instance.gameObject.layer;
            DropDown.transform.SetParent(@object.transform);
            var dbox = DropDown.gameObject.AddComponent<BoxCollider2D>();
            dbox.size = new(0.3f,0.3f);
            DropDownButton = DropDown.gameObject.AddComponent<PassiveButton>();
            DropDownButton.OnClick = new();
            DropDownButton.OnMouseOut = new UnityEvent();
            DropDownButton.OnMouseOver = new UnityEvent();
            DropDownButton._CachedZ_k__BackingField = 0.1f;
            DropDownButton.CachedZ = 0.1f;
            DropDownButton.Colliders = new[] { DropDown.GetComponent<BoxCollider2D>() };
            DropDownButton.OnClick.AddListener((System.Action)(() =>
            {
                isEnable = !isEnable;
                DropDown.sprite = isEnable ? Sprites.GetSpriteFromResources("ui.arrow_drop_down.png", 50) : Sprites.GetSpriteFromResources("ui.arrow_drop_up.png", 50);
                Logger.Info(isEnable.ToString(),teams.ToString());
            }));

            DropDownButton.OnMouseOver.AddListener((System.Action)(() =>
            {
                DropDown.color = Palette.AcceptedGreen;
            }));
            DropDownButton.OnMouseOut.AddListener((System.Action)(() =>
            {
                DropDown.color = Color.white;
            }));
            DropDownButton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
            DropDownButton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;
        }
        public void SetPos(float num)
        {
            this.num = num;

            @object.transform.localPosition = new(-2.0f, 2f - 0.36f * num, 0);
        }
        public void Dragging()
        {

            var renderer = @object.GetComponent<SpriteRenderer>();
            Color color = GetLink.ColorFromTeams.ContainsKey(teams) ? GetLink.ColorFromTeams[teams] : Helper.ColorFromColorcode("#00000000");

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
    }
}
