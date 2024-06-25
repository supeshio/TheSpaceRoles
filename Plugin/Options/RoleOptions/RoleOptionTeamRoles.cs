//using HarmonyLib;
//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Events;
//using static TheSpaceRoles.Helper;

//namespace TheSpaceRoles
//{
//    public class RoleOptionTeamRoles
//    {

//        public static System.Collections.Generic.List<RoleOptionTeamRoles> RoleOptionsInTeam = [];
//        public GameObject @object;
//        public TextMeshPro Title_TMP;
//        public TextMeshPro Value_TMP;
//        public bool Virtual;
//        public Roles role;
//        public Teams team;
//        public int memberCount = 1;
//        public PassiveButton AddedRoleButton;
//        public PassiveButton rbutton;
//        public PassiveButton lbutton;

//        public RoleOptionTeamRoles(Teams teams, Roles role)
//        {
//            this.role = role;
//            this.team = teams;
//            @object = new(team.ToString() + "_" + role.ToString());
//            @object.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("Teams"));
//            //var op = @object.AddComponent<RoleOptionBehavior>();
//            //op.Set(role, team);
//            //optionBehavior.Set(teams: team);
//            @object.transform.localPosition = new(0.6f, 2f + (RoleOptionsInTeam.Count * -0.36f), 0f);
//            @object.layer = Data.UILayer;


//            var renderer = @object.AddComponent<SpriteRenderer>();
//            renderer.sprite = Sprites.GetSpriteFromResources("ui.team_role_banner.png", 400);
//            if (GetLink.CustomTeamLink.Any(x => x.Team == teams))
//            {

//                renderer.color = GetLink.ColorFromTeams(teams);
//            }
//            else
//            {

//                renderer.color = Color.yellow;
//            }


//            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
//            Title_TMP.transform.SetParent(@object.transform);
//            Title_TMP.fontStyle = FontStyles.Bold;
//            Title_TMP.text = GetLink.GetCustomRole(role).RoleName;
//            Title_TMP.color = GetLink.GetCustomRole(role).Color;
//            Title_TMP.fontSize = Title_TMP.fontSizeMax = 1.8f;
//            Title_TMP.fontSizeMin = 0.4f;
//            Title_TMP.alignment = TextAlignmentOptions.Center;
//            Title_TMP.enableWordWrapping = false;
//            Title_TMP.outlineWidth = 0.8f;
//            Title_TMP.autoSizeTextContainer = false;
//            Title_TMP.enableAutoSizing = true;
//            Title_TMP.transform.localPosition = new Vector3(-0.2f, 0f, -1f);
//            Title_TMP.transform.localScale = Vector3.one;
//            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
//            Title_TMP.m_sharedMaterial = Data.textMaterial;
//            Title_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
//            Title_TMP.rectTransform.sizeDelta = new Vector2(1.8f, 0.5f);



//            Value_TMP = new GameObject("Value_TMP").AddComponent<TextMeshPro>();
//            Value_TMP.transform.SetParent(@object.transform);
//            Value_TMP.fontStyle = FontStyles.Bold;
//            Value_TMP.color = GetLink.GetCustomRole(role).Color;
//            Value_TMP.fontSize = Value_TMP.fontSizeMax = 1.8f;
//            Value_TMP.fontSizeMin = 0.4f;
//            Value_TMP.alignment = TextAlignmentOptions.Center;
//            Value_TMP.enableWordWrapping = false;
//            Value_TMP.outlineWidth = 0.8f;
//            Value_TMP.autoSizeTextContainer = false;
//            Value_TMP.transform.localPosition = new Vector3(0.6f, 0f, -2f);
//            Value_TMP.transform.localScale = Vector3.one;
//            Value_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
//            Value_TMP.m_sharedMaterial = Data.textMaterial;
//            Value_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
//            Value_TMP.rectTransform.sizeDelta = new Vector2(1f, 0.5f);


//            var box = @object.AddComponent<BoxCollider2D>();
//            box.size = renderer.bounds.size;
//            AddedRoleButton = @object.AddComponent<PassiveButton>();
//            AddedRoleButton.OnClick = new();
//            AddedRoleButton.OnMouseOut = new UnityEvent();
//            AddedRoleButton.OnMouseOver = new UnityEvent();
//            AddedRoleButton._CachedZ_k__BackingField = 0.1f;
//            AddedRoleButton.CachedZ = 0.1f;
//            AddedRoleButton.Colliders = new[] { @object.GetComponent<BoxCollider2D>() };
//            AddedRoleButton.OnClick.AddListener((System.Action)(() =>
//            {
//                RoleOptionsInTeam.Do(x => { x.CountNone(); });
//                RoleOptionsDescription.Set(teams, this.role);
//                RoleOptionOptions.Check(teams, this.role);

//            }));

//            AddedRoleButton.OnMouseOver.AddListener((System.Action)(() =>
//            {
//                renderer.color = Helper.ColorEditHSV(GetLink.ColorFromTeams(team), s: -0.2f);

//            }));
//            AddedRoleButton.OnMouseOut.AddListener((System.Action)(() =>
//            {
//                renderer.color = GetLink.ColorFromTeams(team);

//            }));
//            AddedRoleButton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
//            AddedRoleButton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;



//            var right = new GameObject("right").AddComponent<SpriteRenderer>();
//            right.sprite = Sprites.GetSpriteFromResources("ui.double_right.png", 80);
//            right.gameObject.layer = HudManager.Instance.gameObject.layer;
//            right.transform.SetParent(@object.transform);
//            right.transform.localScale = Vector3.one;
//            right.transform.localPosition = new Vector3(1.1f, 0, -1);
//            right.color = Color.white;
//            right.material = Data.textMaterial;
//            right.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(0.3f, 0.3f);
//            rbutton = right.gameObject.AddComponent<PassiveButton>();
//            rbutton.OnClick = new();
//            rbutton.OnMouseOut = new UnityEvent();
//            rbutton.OnMouseOver = new UnityEvent();
//            rbutton._CachedZ_k__BackingField = 0.1f;
//            rbutton.CachedZ = 0.1f;
//            rbutton.Colliders = new[] { right.GetComponent<BoxCollider2D>() };
//            rbutton.OnClick.AddListener((System.Action)(() => { SetCount(memberCount + 1); }));
//            rbutton.OnMouseOver.AddListener((System.Action)(() => { right.color = Palette.AcceptedGreen; }));
//            rbutton.OnMouseOut.AddListener((System.Action)(() => { right.color = Color.white; }));
//            rbutton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
//            rbutton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;


//            var left = new GameObject("left").AddComponent<SpriteRenderer>();
//            left.sprite = Sprites.GetSpriteFromResources("ui.double_left.png", 80);
//            left.gameObject.layer = HudManager.Instance.gameObject.layer;
//            left.transform.SetParent(@object.transform);
//            left.transform.localScale = Vector3.one;
//            left.color = Color.white;
//            left.transform.localPosition = new Vector3(-1.1f, 0, -1);
//            left.material = Data.textMaterial;
//            left.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(0.3f, 0.3f);
//            lbutton = left.gameObject.AddComponent<PassiveButton>();
//            lbutton.OnClick = new();
//            lbutton.OnMouseOut = new UnityEvent();
//            lbutton.OnMouseOver = new UnityEvent();
//            lbutton._CachedZ_k__BackingField = 0.1f;
//            lbutton.CachedZ = 0.1f;
//            lbutton.Colliders = new[] { left.GetComponent<BoxCollider2D>() };
//            lbutton.OnClick.AddListener((UnityAction)(() => { SetCount(memberCount - 1); }));
//            lbutton.OnMouseOver.AddListener((UnityAction)(() => { left.color = Palette.AcceptedGreen; }));
//            lbutton.OnMouseOut.AddListener((UnityAction)(() => { left.color = Color.white; }));
//            lbutton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
//            lbutton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;

//            CustomOptionsHolder.CreateRoleOptions(team, role);



//            if (CustomOption.GetRoleOption("spawncount", role, team).entry.Value < 1)
//            {
//                CustomOption.SetRoleOption("spawncount", role, team, 1);
//                CustomOption.SetRoleOption("spawnrate", role, team, 10);

//            }
//            memberCount = CustomOption.GetRoleOption("spawncount", role, team).entry.Value;
//            SetCount(memberCount);
//        }
//        public void SetCount(int count)
//        {
//            if (CustomOption.GetRoleOption("spawncount", role, team).selection != count)
//            {

//                CustomOption.SetRoleOption("spawncount", role, team, count);
//            }
//            memberCount = count;
//            Value_TMP.text = Translation.GetString("people_count", [memberCount.ToString()]);
//            Value_TMP.m_sharedMaterial = Data.textMaterial;
//            Value_TMP.material = Data.textMaterial;
//            Value_TMP.fontMaterial = Data.textMaterial;
//        }
//        public void CountNone()
//        {

//            if (memberCount > 0)
//            {

//            }
//            else
//            {
//                Remove();
//                RoleOptionsDescription.SetDescription("", "", "");
//                foreach (var op in CustomOptionsHolder.Options)
//                {
//                    foreach (var opt in op)
//                    {
//                        opt.@object.active = false;
//                    }
//                }
//            }
//        }
//        public void CheckCount()
//        {
//            if (CustomOption.GetRoleOption("spawncount", role, team).selection != this.memberCount)
//            {
//                SetCount(CustomOption.GetRoleOption("spawncount", role, team).entry.Value);
//            }

//        }
//        public void SetPos(float num)
//        {

//            @object.transform.localPosition = new(-2.0f, 2f - 0.36f * num, -1f);
//        }
//        public void Remove()
//        {
//            UnityEngine.Object.Destroy(@object);

//            RoleOptionsInTeam.Remove(this);
//        }
//        public void Dragging()
//        {

//            var renderer = @object.GetComponent<SpriteRenderer>();
//            Color color = GetLink.CustomTeamLink.Any(x => x.Team == team) ? GetLink.ColorFromTeams(team) : Color.clear;

//            var drag = RoleOptionsHolder.roleOptions.First(x => x.MouseHolding).roles;
//            if (!GetLink.GetCustomRole(drag).teamsSupported.Contains(team))
//            {

//                color = ColorEditHSV(color, v: -0.6f);
//            }
//            renderer.color = color;
//        }
//    }
//}
