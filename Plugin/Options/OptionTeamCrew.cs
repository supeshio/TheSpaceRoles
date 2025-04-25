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

namespace TheSpaceRoles
{
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
        public static int PlayerCount;
        public static TextMeshPro Text;
        public static void Create()
        {
            if (TeamCrewParent != null) return;
            
            CrewSprite= Sprites.GetSpriteFromResources("ui.crewmate.png",400f);
            BackCrewSprite = Sprites.GetSpriteFromResources("ui.crewmate.png", 370f);
            DR = Sprites.GetSpriteFromResources("ui.double_right.png");
            DL = Sprites.GetSpriteFromResources("ui.double_left.png");
            TeamCrewParent = new GameObject("TeamCrewParent");
            TeamCrewParent.transform.SetParent(HudManager.Instance.transform);
            TeamCrewParent.transform.localPosition = new Vector3(0,0,-100f);
            CrewList = new List<GameObject>();
            for (int i = 0; i < 15; i++)
            {
                GameObject a = new GameObject($"Crew{i}");
                a.transform.SetParent(TeamCrewParent.transform);
                a.layer = Data.UILayer;
                var sp = a.AddComponent<SpriteRenderer>();
                sp.sprite = CrewSprite;
                sp.material = Data.NormalMaterial;
                GameObject b = GameObject.Instantiate(a);
                b.transform.SetParent(a.transform);
                b.transform.localPosition = new Vector3(-0.005f, 0.005f, 1);
                b.GetComponent<SpriteRenderer>().sprite = BackCrewSprite;
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
                colider.size = sp.bounds.size;
                action.OnClick = new();
                action.OnMouseOut = new UnityEvent();
                action.OnMouseOver = new UnityEvent();
                action._CachedZ_k__BackingField = 0.1f;
                action.CachedZ = 0.1f;
                action.Colliders = new[] { colider };
                action.OnClick.AddListener((System.Action)(() => { onClick.Invoke(); }));
                action.OnMouseOver.AddListener((System.Action)(() => { sp.color = Palette.AcceptedGreen; }));
                action.OnMouseOut.AddListener((System.Action)(() => { sp.color = Color.white; }));
                action.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
                action.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;

                DoubleLeft.transform.localPosition = new Vector3(x, 2.2f, 0);
                DoubleRight.sprite = sprite;
            };
            Double(DoubleLeft, -5.0f, DL, () => { PlayerCount++; SetPlayers(PlayerCount); });
            Double(DoubleRight,-4.8f,DR, () => { PlayerCount--; SetPlayers(PlayerCount); });
            
        }
        public static void SetPlayers(int players)
        {
            if(PlayerCount < 0) PlayerCount = 15;
            if (PlayerCount > 15) PlayerCount = 0;
            DoubleLeft.gameObject.SetActive(true);
            DoubleRight.gameObject.SetActive(true);
            CrewList.Do(x => x.SetActive(false));
            for (int i = 0; i < players; i++)
            {
                var crew = CrewList[i];
                crew.SetActive(true);
                crew.transform.localPosition = new Vector3(i*0.48f -4.4f, 2.5f,0);
                var sp = crew.GetComponent<SpriteRenderer>();
                sp.color = new CrewmateTeam().Color;
            }
            var opt = CustomOptionsHolder.TeamOptions_Count;
            var opr = CustomOptionsHolder.RoleOptions_Count;
            Dictionary<Teams, System.Collections.Generic.List<Tuple<Roles,CustomOption>>> CRoles = [];
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
            DoubleLeft.gameObject.SetActive(false);
            DoubleRight.gameObject.SetActive(false);
        }

    }
}
