using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.ParticleSystem.PlaybackState;
using UnityEngine.Events;
using TMPro;
using UnityEngine.TextCore;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class OptionTeamCrew
    {
        public static GameObject TeamCrewParent;
        public static Sprite CrewSprite;
        public static Sprite BackCrewSprite;
        public static Sprite DR;
        public static Sprite DL;
        public static List<GameObject> CrewList;
        public static SpriteRenderer DoubleRight;
        public static SpriteRenderer DoubleLeft;
        public static int PlayerCount = 0;
        public static TextMeshPro Text;
        public static void Create(GameSettingMenu __instance)
        {
            if (TeamCrewParent?.gameObject != null) return;
            
            CrewSprite= Sprites.GetSpriteFromResources("ui.crewmate.png",400f);
            BackCrewSprite = Sprites.GetSpriteFromResources("ui.crewmate.png", 370f);
            DR = Sprites.GetSpriteFromResources("ui.double_right.png");
            DL = Sprites.GetSpriteFromResources("ui.double_left.png");
            TeamCrewParent = new GameObject("TeamCrewParent");
            TeamCrewParent.transform.SetParent(__instance.transform);
            TeamCrewParent.transform.localPosition= new Vector3(0,0,-100f);
            CrewList = [];
            for (int i = 0; i < 15; i++)
            {
                GameObject a = new($"Crew{i}");
                a.transform.SetParent(TeamCrewParent.transform);
                a.layer = Data.UILayer;
                var sp = a.AddComponent<SpriteRenderer>();
                sp.sprite = CrewSprite;
                sp.material = Data.NormalMaterial;
                GameObject b = GameObject.Instantiate(a);
                b.transform.SetParent(a.transform);
                b.transform.localPosition = new Vector3(-0.02f, -0.02f, 1);
                b.GetComponent<SpriteRenderer>().sprite = CrewSprite;
                CrewList.Add(a);
            }
            Hide();
            DoubleLeft = new GameObject("left").AddComponent<SpriteRenderer>();
            DoubleRight = new GameObject("right").AddComponent<SpriteRenderer>();
            var Double = (SpriteRenderer sp,float x,Sprite sprite,Action onClick) => { 
                sp.transform.SetParent (TeamCrewParent.transform);
                sp.material = Data.NormalMaterial;
                sp.gameObject.SetActive(true);
                sp.gameObject.layer = Data.UILayer;
                sp.color = Color.white;
                sp.material.color = Color.white;
                var action = sp.gameObject.AddComponent<PassiveButton>();

                var colider = sp.gameObject.AddComponent<BoxCollider2D>();
                colider.size = new Vector2(0.25f, 0.25f);
                colider.bounds.size.Set(0.2f, 0.2f, 0.2f);
                action.OnClick = new();
                action.OnMouseOut = new UnityEvent();
                action.OnMouseOver = new UnityEvent();
                action._CachedZ_k__BackingField = 0.1f;
                action.CachedZ = 0.1f;
                action.Colliders = new[] { colider };
                action.OnClick.AddListener((System.Action)(() => { onClick.Invoke(); }));
                action.OnMouseOver.AddListener((System.Action)(() => { sp.color = Palette.AcceptedGreen; }));
                action.OnMouseOut.AddListener((System.Action)(() => { sp.color = Color.white; }));
                action.HoverSound = HudManager.Instance.Chat.chatScreen.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
                action.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;

                sp.transform.localPosition = new Vector3(x, 2.4f, 0);
                sp.sprite = sprite;
            };
            Double(DoubleLeft, -5.2f, DL, () => { PlayerCount--; SetPlayers(); });
            Double(DoubleRight,-4.7f,DR, () => { PlayerCount++; SetPlayers(); });

            Text = new GameObject("ShowCrewType").AddComponent<TextMeshPro>();
            Text.transform.SetParent(TeamCrewParent.transform);
            Text.transform.localPosition = new Vector2(-4.95f, 2.7f);
            Text.m_sharedMaterial = Data.NormalMaterial;
            Text.fontSizeMax =
            Text.fontSize = 5f;
            Text.fontSizeMin = 0.1f;
            Text.enableAutoSizing = true;
            //Text.bounds.size.Set(0.6f, 0.3f, 1);
            Text.alignment = TextAlignmentOptions.Center;
            Text.autoSizeTextContainer = false;
            Text.enableAutoSizing = true;
            Text.enableWordWrapping = false;
            Text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            Text.rectTransform.sizeDelta = new Vector2(0.6f, 1f);
            Text.gameObject.layer = Data.UILayer;
            Text.outlineWidth = 0.5f;

            SetPlayers();
        }
        public static bool isShowing;
        private static int lastplayercount;
        [HarmonyPatch(typeof(GameSettingMenu),nameof(GameSettingMenu.Update)),HarmonyPostfix]
        public static void MenuUpdate()
        {
            if(lastplayercount 
                != PlayerCount)
            {
                lastplayercount = PlayerCount;

                SetPlayers();
            }
        }
        public static void SetPlayers()
        {

            if (HudManager.Instance.transform.parent.FindChild("PlayerOptionsMenu(Clone)") == null) { Logger.Info("PlayerOptionsMenu(Clone) is not found."); return; }

            if (PlayerCount < 0) PlayerCount = GameOptionsManager.Instance.GameHostOptions.MaxPlayers;
            if (PlayerCount > GameOptionsManager.Instance.GameHostOptions.MaxPlayers) PlayerCount = 0;
            Text.text = PlayerCount == 0 ? Translation.GetString("option.teamcrew.nowplayer") : PlayerCount.ToString();
            DoubleLeft.gameObject.SetActive(true);
            DoubleRight.gameObject.SetActive(true);
            CrewList.Do(x => x.SetActive(false));
            int player_count = PlayerCount==0 ? GameStartManager.Instance.LastPlayerCount : PlayerCount;
            for (int i = 0; i < player_count; i++)
            {
                var crew = CrewList[i];
                crew.SetActive(true);
                crew.transform.localPosition = new Vector3(i*0.48f -4.4f, 2.7f,0);
                var sp = crew.GetComponent<SpriteRenderer>();
                sp.color = new CrewmateTeam().Color;
            }
            var opt = CustomOptionsHolder.TeamOptions_Count;
            var opr = CustomOptionsHolder.RoleOptions_Count;
            Dictionary<Teams, List<Tuple<Roles,CustomOption>>> CRoles = [];
            foreach((var role,var cus) in opr)
            {
                var c = RoleData.GetCustomRoleFromRole(role);
                if (!CRoles.ContainsKey(c.Team)) { 
                    CRoles.Add(c.Team, []); 

                }
                CRoles[c.Team].Add((role, cus).ToTuple());
            }
            int k = 0;

            foreach ((var team, var cus) in opt)
            {
                int p = cus.GetIntValue();
                if (p > 0)
                {
                    int outline = CRoles.ContainsKey(team) ? CRoles[team].Sum(x=>x.Item2.GetIntValue()) : 0;
                    var co = RoleData.GetColorFromTeams(team);
                    Logger.Info(outline.ToString(),$"outline:{team}");
                    for (int i = 0; i < p; i++)
                    {
                        var s = CrewList[k].GetComponent<SpriteRenderer>();
                        if (i < outline) {
                            var backsp = s.transform.GetChild(0).GetComponent<SpriteRenderer>();
                            backsp.color = Helper.ColorFromColorcode("#efefbf");
                            //"_OutlineColor"_Outline
                        }
                        else
                        {
                        }
                            s.color = co;
                        k++;
                    }
                }
            }

        }
        
        public static void Hide()
        {
            CrewList.Do(x => x.SetActive(false));
            CrewList.Do(x =>
            {
                var backsp = x.transform.GetChild(0).GetComponent<SpriteRenderer>();
                backsp.gameObject.SetActive(true);
                backsp.color = Helper.ColorFromColorcode("#0f0f0f");
            });
            if (DoubleLeft != null)
            {
                DoubleLeft.gameObject.SetActive(false);
                DoubleRight.gameObject.SetActive(false);
            }
        }

    }
}
