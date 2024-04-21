using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static Il2CppSystem.Xml.XmlWellFormedWriter.AttributeValueCache;
using static TheSpaceRoles.Translation;
using Enum = System.Enum;

namespace TheSpaceRoles
{
    public class RoleOptions
    {
        public static bool DragMode = false;
        public Roles roles;
        public string GetRoleName => GetString($"role.{roles}.name");
        public GameObject @object;
        public int num = 0;
        public TextMeshPro Title_TMP;
        public RoleOptions(Roles roles, int i)
        {
            this.roles = roles;
            //Logger.Info(roles.ToString());
            this.num = i;
            @object = new GameObject(roles.ToString());
            @object.active = true;
            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.role_option.png", 400);
            renderer.color = Helper.ColorFromColorcode("#222222");
            @object.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("Roles"));
            @object.active = true;
            @object.layer = HudManager.Instance.gameObject.layer;
            @object.transform.localPosition = new(-4.4f, 2f - 0.36f * num, 0);

            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.SetParent(@object.transform);
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = GetRoleName;
            Title_TMP.color = GetLink.GetCustomRole(roles).Color;
            Title_TMP.fontSize = Title_TMP.fontSizeMax = 2f;
            Title_TMP.fontSizeMin = 1f;
            Title_TMP.alignment = TextAlignmentOptions.Left;
            Title_TMP.enableWordWrapping = false;
            Title_TMP.outlineWidth = 0.8f;
            Title_TMP.autoSizeTextContainer = false;
            Title_TMP.enableAutoSizing = true;
            Title_TMP.transform.localPosition = new Vector3(0f, 0, -1);
            Title_TMP.transform.localScale = Vector3.one;
            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
            Title_TMP.m_sharedMaterial = Data.textMaterial;
            Title_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Title_TMP.rectTransform.sizeDelta = new Vector2(1.2f, 0.5f);
            var box = @object.AddComponent<BoxCollider2D>();
            box.size = renderer.bounds.size;
            var Button = @object.AddComponent<PassiveButton>();
            Button.OnClick = new();
            Button.OnMouseOut = new UnityEvent();
            Button.OnMouseOver = new UnityEvent();
            Button._CachedZ_k__BackingField = 0.1f;
            Button.CachedZ = 0.1f;
            Button.Colliders = new[] { @object.GetComponent<BoxCollider2D>() };
            Button.OnClick.AddListener((System.Action)(() =>
            {
                RoleOptionsDescription.Set(this);
            }));

            Button.OnMouseOver.AddListener((System.Action)(() =>
            {
                renderer.color = RoleOptionsHolder.selectedRoles == roles ? Helper.ColorFromColorcode("#cccccc") : Helper.ColorFromColorcode("#555555");
                MouseOver = true;
            }));
            Button.OnMouseOut.AddListener((System.Action)(() =>
            {
                renderer.color = RoleOptionsHolder.selectedRoles == roles ? Helper.ColorFromColorcode("#cccccc") : Helper.ColorFromColorcode("#222222");
                MouseOver = false;
            }));
            Button.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
            Button.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;

        }
        public bool MouseOver = false;
        public bool MouseHolding = false;
        public float timer = 0;
        public GameObject HoldinggameObject;
        public Vector3 mousePos = Vector3.zero;
        public static Teams? SelectedTeams;
        private static Vector3 GetMouse => Camera.allCameras.First(x => x.name == "UI Camera").ScreenToWorldPoint(Input.mousePosition);
        public void MouseOverUpdate()
        {
            if (!@object.active) return;
            if (MouseOver)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    RoleOptionsHolder.roleOptions.Do(x => x.@object.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#222222"));

                    RoleOptionsHolder.selectedRoles = roles;

                    var renderer = @object.GetComponent<SpriteRenderer>();



                    renderer.color = Helper.ColorFromColorcode("#cccccc");

                    MouseHolding = true;
                    DragMode = true;
                    HoldinggameObject = UnityEngine.Object.Instantiate(@object);
                    mousePos = @object.transform.position - GetMouse;
                    HoldinggameObject.transform.SetParent(@object.transform.parent);
                    //HoldinggameObject.transform.FindChild("Title_TMP").GetComponent<TextMeshPro>().color = Helper.ColorEditHSV(GetLink.GetCustomRole(roles).Color, a: 0.95f);
                    HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#2222227f");
                }
            }
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                if (MouseHolding)
                {

                    HoldinggameObject.name = "Hold:" + roles.ToString();
                    HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#2222227f");

                    HoldinggameObject.transform.position = new(mousePos.x + GetMouse.x, mousePos.y + GetMouse.y, HoldinggameObject.transform.parent.position.z - 3);
                    UnityEngine.Object.Destroy(HoldinggameObject.GetComponent<PassiveButton>());
                    UnityEngine.Object.Destroy(HoldinggameObject.GetComponent<BoxCollider2D>());
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit2D hit2d = Physics2D.Raycast(ray.origin, ray.direction);

                    SelectedTeams = null;
                    HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#2222227f");
                    foreach (var item in RoleOptionTeamsHolder.TeamsHolder)
                    {
                        if(item.@object.transform == hit2d.transform)
                        {
                            if (GetLink.GetCustomRole(roles).teamsSupported.Contains(item.teams))
                            {

                                SelectedTeams = item.teams;
                                HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorEditHSV(GetLink.ColorFromTeams[(Teams)item.teams], a: 0.8f, v: -0.1f);
                                break;
                            }
                        }
                        if (item.DropDown.transform == hit2d.transform)
                        {
                            if (GetLink.GetCustomRole(roles).teamsSupported.Contains(item.teams))
                            {

                                SelectedTeams = item.teams;
                                HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorEditHSV(GetLink.ColorFromTeams[(Teams)item.teams], a: 0.8f, v: -0.1f);
                                break;
                            }
                        }
                    }
                    foreach (var item in RoleOptionTeamRoles.RoleOptionsInTeam)
                    {
                        if (item.@object.transform.position == hit2d.transform.position)
                        {
                            if (GetLink.GetCustomRole(roles).teamsSupported.Contains(item.team))
                            {
                                SelectedTeams = item.team;
                                HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorEditHSV(GetLink.ColorFromTeams[(Teams)item.team], a: 0.8f, v: -0.1f);
                                break;
                            }

                        }
                    }

                    //var roleoptionbehavior = hit2d.transform?.gameObject?.GetComponentInChildren<RoleOptionBehavior>();
                    //if (roleoptionbehavior != null)
                    //{
                    //    SelectedTeams = roleoptionbehavior.Teams;

                    //    HoldinggameObject.GetComponent<SpriteRenderer>().color = GetLink.ColorFromTeams[(Teams)roleoptionbehavior.Teams];
                    //}
                    //else
                    //{

                    //    SelectedTeams = null;
                    //    HoldinggameObject.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#2222227f");
                    //}


                }
            }
            else
            {
                if (HoldinggameObject != null)
                {

                    if (SelectedTeams != null)
                    {
                        if (RoleOptionTeamRoles.RoleOptionsInTeam.Where(x=>x.team==SelectedTeams).Any(x=>x.role==roles))
                        {
                            //人数増やす処理入れて
                            Logger.Info($"The role \"{roles}\" is alrady included");
                        }
                        else
                        {

                            var ROteams = RoleOptionTeamsHolder.TeamsHolder.First(x => x.teams == (Teams)SelectedTeams);

                            RoleOptionTeamRoles.RoleOptionsInTeam.Add(new RoleOptionTeamRoles(ROteams, roles));

                        }
                    }



                    DragMode = false;
                    MouseHolding = false;
                    GameObject.Destroy(HoldinggameObject);
                    HoldinggameObject = null;
                    _ = Time.deltaTime;

                    timer = 0;
                    SelectedTeams = null;
                }
            }
        }
    }
    [HarmonyPatch(typeof(PassiveButton), nameof(PassiveButton.Update))]
    public static class RoleOptionsManager
    {
        public static void Postfix(PassiveButton __instance)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (__instance.name == role.ToString())
                {
                    foreach (var item_ in RoleOptionsHolder.roleOptions)
                    {
                        if (__instance.name == item_.roles.ToString())
                        {
                            item_.MouseOverUpdate();

                        }
                    }
                }
            }

            RoleOptionTeams.Drag();
        }
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        private static class HudManagerUpdate
        {
            private static void Postfix()
            {
                SetNums();

            }
        }
        public static void SetNums()
        {

            int i = 0;
            int team = 0;
            foreach (var roleop in RoleOptionTeamsHolder.TeamsHolder)
            {
                roleop.SetPos(i + team * 1.2f); 
                List<RoleOptionTeamRoles> items = RoleOptionTeamRoles.RoleOptionsInTeam.Where(x => x.team == roleop.teams).ToList();
                roleop.Title_TMP.text = roleop.teams.ToString() + $"({items.Count})";
                if (roleop.isEnable)
                {

                    
                    foreach (var item in items)
                    {
                        item.@object.active = true;
                        i++;
                        item.SetPos(i + team * 1.2f);
                    }
                }
                else
                {
                    foreach (var item in items)
                    {
                        item.@object.active = false;
                    }
                }

                team++;
            }
        }
    }
}
