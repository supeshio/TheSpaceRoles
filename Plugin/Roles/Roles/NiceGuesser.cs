using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.CustomOptionsHolder;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class NiceGuesser : CustomRole
    {
        public List<Target> targets = [];
        public static NiceGuesser instance;
        public NiceGuesser()
        {
            team = Teams.Crewmate;
            Role = Roles.NiceGuesser;
            Color = Helper.ColorFromColorcode("#FFCC00");
        }
        public static CustomOption GuessCount;
        public static CustomOption GuessCountOfMeeting;
        public static CustomOption CanGuessCrewmate;
        public override void OptionCreate()
        {
            if (GuessCount != null) return;

            GuessCount = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.niceguesser.guesscount", GetCounts(include_0:false), 2);
            GuessCountOfMeeting = Create(CustomOption.OptionType.Crewmate, "role.niceguesser.guesscountofmeeting", GetCounts(include_0: false), 14);
            CanGuessCrewmate = Create(CustomOption.OptionType.Crewmate, "role.niceguesser.canguesscrewmate", true);

            Options = [GuessCount,GuessCountOfMeeting,CanGuessCrewmate];
        }
        public static int remainBullet;
        public static int remainBulletOfMeeting;
        public override void HudManagerStart(HudManager hudManager)
        {
            remainBullet = GuessCountOfMeeting.GetInts(include_0: false);
            instance = this;
            targets = [];
        }
        public override void MeetingStart(MeetingHud meeting)
        {
            remainBulletOfMeeting = GuessCount.GetInts(include_0:false);
            TargetReset(meeting);
        }
        public void TargetReset(MeetingHud meeting,int[] untargetingplayerids = null)
        {
            if (targets != null | targets.Count > 0)
            {
                foreach (var target in targets) 
                {
                    GameObject.Destroy(target.renderer.gameObject);
                }
            }
            targets = [];
            if (!PlayerControl.LocalPlayer.Data.IsDead)
            {

                foreach (var player in MeetingHud.Instance.playerStates)
                {
                    if (player != null && !player.AmDead && player.TargetPlayerId != PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (untargetingplayerids != null)
                        {
                            if (!untargetingplayerids.Contains(player.TargetPlayerId))
                            {

                                targets.Add(new Target(player, meeting));
                            }
                        }
                        else
                        {

                            targets.Add(new Target(player, meeting));
                        }
                    }
                }
            }
            foreach (var player in MeetingHud.Instance.playerStates)
            {
                Logger.Message($"player:{player.NameText.text},Dead:{player.AmDead},null{player == null}");
            }
        }
        public static SpriteRenderer crewmateRend;
        public static SpriteRenderer impostorRend;
        public static SpriteRenderer neutralRend;
        public static int selected = 0;
        public static void ChoiceRole(MeetingHud meeting)
        {
            crewmateRend = null;
            impostorRend = null;
            neutralRend = null;
            selected = 0;
            SpriteRenderer background = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("Background").GetComponent<SpriteRenderer>();
            SpriteRenderer basecolor = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("baseColor").GetComponent<SpriteRenderer>();
            Logger.Info(background.tag);
            Transform parent = meeting.meetingContents.transform.FindChild("PhoneUI");

            var crewteam = new GameObject("CrewTeamButtons");
            crewteam.transform.localPosition = Vector3.zero;
            crewteam.transform.SetParent(parent);
            var impteam = new GameObject("ImpostorTeamButtons");
            impteam.transform.localPosition = Vector3.zero;
            impteam.transform.SetParent(parent);
            var neuteam = new GameObject("NeutralTeamButtons");
            neuteam.transform.localPosition = Vector3.zero;
            neuteam.transform.SetParent(parent);

            // PhoneUI\
            //BackGround 0 0 7
            //baseColor 0.012 0 8
            meeting.ButtonParent.gameObject.SetActive(false);

            void reset()
            {

                crewteam.gameObject.SetActive(false);
                impteam.gameObject.SetActive(false);
                neuteam.gameObject.SetActive(false);

                crewmateRend.color = Color.white;
                impostorRend.color = Color.white;
                neutralRend.color = Color.white;
                switch (selected)
                {
                    case 0:
                        crewteam.gameObject.SetActive(true);
                        crewmateRend.color = Palette.AcceptedGreen;
                        break;
                    case 1:

                        impteam.gameObject.SetActive(true);
                        impostorRend.color = Palette.AcceptedGreen;
                        break;

                    case 2:

                        neuteam.gameObject.SetActive(true);
                        neutralRend.color = Palette.AcceptedGreen;
                        break;
                }
            }


            crewmateRend = ButtonCreate(parent, Teams.Crewmate);
            crewmateRend.transform.localPosition = new Vector3(-2f, 2.2f, -10);
            crewmateRend.transform.localScale = new(1.2f, 1.2f, 1.2f);
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOut = new();
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOver = new();
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnClick = new();
            crewmateRend.gameObject.GetComponent<PassiveButton>().ClickSound = HudManager.Instance.Chat.chatButton.ClickSound;
            crewmateRend.color = Palette.AcceptedGreen;
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                selected = 0;
                reset();
                crewteam.gameObject.SetActive(true);
                crewmateRend.color = Palette.AcceptedGreen;

            }));
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                if (selected == 0)
                {

                    reset();
                }
                else
                {
                    crewmateRend.color = Helper.ColorFromColorcode("#dbdbdb");

                }
            }));
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                reset();
            }));

            impostorRend = ButtonCreate(parent, Teams.Impostor);
            impostorRend.transform.localPosition = new Vector3(0f, 2.2f, -10);
            impostorRend.transform.localScale = new(1.2f, 1.2f, 1.2f);

            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOut = new();
            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOver = new();
            impostorRend.gameObject.GetComponent<PassiveButton>().OnClick = new();
            impostorRend.gameObject.GetComponent<PassiveButton>().ClickSound = HudManager.Instance.Chat.chatButton.ClickSound;
            impostorRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                selected = 1;
                reset();
                impteam.gameObject.SetActive(true);
                impostorRend.color = Palette.AcceptedGreen;

            }));
            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                if (selected == 1)
                {

                    reset();
                }
                else
                {
                    impostorRend.color = Helper.ColorFromColorcode("#dbdbdb");

                }
            }));
            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                reset();
            }));
            neutralRend = ButtonCreate(parent, Teams.None);

            neutralRend.transform.localPosition = new Vector3(2f, 2.2f, -10);
            neutralRend.transform.localScale = new(1.2f, 1.2f, 1.2f);
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOut = new();
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOver = new();
            neutralRend.gameObject.GetComponent<PassiveButton>().OnClick = new();
            neutralRend.gameObject.GetComponent<PassiveButton>().ClickSound = HudManager.Instance.Chat.chatButton.ClickSound;
            neutralRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                selected = 2;
                reset();
                neuteam.gameObject.SetActive(true);
                neutralRend.color = Palette.AcceptedGreen;

            }));
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                if (selected == 2)
                {
                    reset();
                }
                else
                {
                    neutralRend.color = Helper.ColorFromColorcode("#dbdbdb");

                }
            }));
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                reset();
            }));



            reset();
            crewteam.gameObject.SetActive(true);



            void roleaction(Roles role)
            {

                meeting.ButtonParent.gameObject.SetActive(true);

                GameObject.Destroy(crewteam.gameObject);
                GameObject.Destroy(impteam.gameObject);
                GameObject.Destroy(neuteam.gameObject);
                GameObject.Destroy(crewmateRend.gameObject);
                GameObject.Destroy(impostorRend.gameObject);
                GameObject.Destroy(neutralRend.gameObject);
                crewmateRend = null;
                impostorRend = null;
                neutralRend = null;
                int pc = -1;
                if (targetplayer.IsRole(role))
                {
                    UnCheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, targetplayer, DeathReason.ShotByNiceGuesser);
                    pc = targetplayer.PlayerId;
                }
                else
                {

                    UnCheckedMurderPlayer.RpcMurder(targetplayer, PlayerControl.LocalPlayer, DeathReason.ShotByNiceGuesser);
                    pc = PlayerControl.LocalPlayer.PlayerId;
                }
                NiceGuesser.instance.TargetReset(meeting, [pc]);
            }


            int c = 0;
            int i = 0;
            int n = 0;
            if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {

                foreach (var role in RoleData.GetCustomRoles.ToArray().Select(x => x.Role))
                {
                    if (CanGuessCrewmate.GetBool()&&role==Roles.Crewmate)
                    {
                        continue;
                    }
                    SpriteRenderer rend;

                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Crewmate)
                    {
                        rend = ButtonCreate(crewteam.transform, Teams.Crewmate);
                        rend.transform.localPosition = new Vector3(-3.15f + 1.8f * (c % 4), 3.5f - 0.4f * Mathf.Floor(c++ / 4f), -10);
                    }
                    else
                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Impostor)
                    {
                        rend = ButtonCreate(impteam.transform, Teams.Impostor);
                        rend.transform.localPosition = new Vector3(-3.15f + 1.8f * (i % 4), 3.5f - 0.4f * Mathf.Floor(i++ / 4f), -10);
                    }
                    else
                    {

                        rend = ButtonCreate(neuteam.transform, Teams.None);
                        rend.transform.localPosition = new Vector3(-3.15f + 1.8f * (n % 4), 3.5f - 0.4f * Mathf.Floor(n++ / 4f), -10);
                    }
                    var p = rend.gameObject.GetComponent<PassiveButton>();
                    p.OnMouseOut = new();
                    p.OnMouseOver = new();
                    p.OnClick = new();
                    p.ClickSound=HudManager.Instance.Chat.chatButton.ClickSound;
                    p.OnClick.AddListener((System.Action)(() =>
                    {
                        rend.color = Palette.AcceptedGreen;
                        roleaction(role);

                    }));
                    p.OnMouseOver.AddListener((System.Action)(() =>
                    {
                        rend.color = Color.gray;
                    }));
                    p.OnMouseOut.AddListener((System.Action)(() =>
                    {
                        rend.color = Color.white;
                    }));
                    rend.gameObject.name = role.ToString();
                    rend.GetComponentInChildren<TextMeshPro>().text = RoleData.GetColoredRoleNameFromRole(role);
                    Logger.Info(role.ToString(), "Passive");
                }
            }
            else
            {

                foreach (var role in DataBase.AssignedRoles)
                {

                    SpriteRenderer rend;
                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Crewmate)
                    {
                        rend = ButtonCreate(crewteam.transform, Teams.Crewmate);
                        rend.transform.localPosition = new Vector3(-3.15f + 1.8f * (c % 4), 3.5f - 0.4f * Mathf.Floor(c++ / 4f), -10);

                    }
                    else
                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Impostor)
                    {
                        rend = ButtonCreate(impteam.transform, Teams.Impostor);
                        rend.transform.localPosition = new Vector3(-3.15f + 1.8f * (i % 4), 3.5f - 0.4f * Mathf.Floor(i++ / 4f), -10);
                    }
                    else
                    {

                        rend = ButtonCreate(neuteam.transform, Teams.None);
                        rend.transform.localPosition = new Vector3(-3.15f + 1.8f * (n % 4), 3.5f - 0.4f * Mathf.Floor(n++ / 4f), -10);
                    }
                    var p = rend.gameObject.GetComponent<PassiveButton>();
                    p.OnMouseOut = new();
                    p.OnMouseOver = new();
                    p.OnClick = new();
                    p.OnClick.AddListener((System.Action)(() =>
                    {
                        rend.color = Palette.AcceptedGreen;
                        roleaction(role);

                    }));
                    p.OnMouseOver.AddListener((System.Action)(() =>
                    {
                        rend.color = Color.gray;
                    }));
                    p.OnMouseOut.AddListener((System.Action)(() =>
                    {
                        rend.color = Color.white;
                    }));
                    rend.gameObject.name = role.ToString();
                    rend.transform.GetComponentInChildren<TextMeshPro>().text = RoleData.GetColoredRoleNameFromRole(role);
                    Logger.Info(role.ToString(),"Passive");
                }
            }
        }
        public static SpriteRenderer ButtonCreate(Transform parent, Teams teams)
        {
            if (teams != Teams.None)
            {
                SpriteRenderer
                spriteRenderer = new GameObject(teams.ToString()).AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(parent);
                spriteRenderer.sprite = Sprites.GetSpriteFromResources("ui.option.png", 350f);
                spriteRenderer.transform.localPosition = new Vector3(-2f, 2.2f, -10);
                spriteRenderer.transform.localScale = Vector3.one;
                spriteRenderer.enabled = true;
                spriteRenderer.gameObject.layer = Data.UILayer;
                spriteRenderer.gameObject.SetActive(true);
                TextMeshPro textMeshPro = new GameObject($"Guesser_{teams}_Button").AddComponent<TextMeshPro>();
                textMeshPro.transform.SetParent(spriteRenderer.transform);
                textMeshPro.gameObject.layer = Data.UILayer;
                textMeshPro.transform.localScale = Vector3.one;
                textMeshPro.transform.localPosition = new Vector3(0f, 0.02f, -1f);
                textMeshPro.text = RoleData.GetColoredTeamNameFromTeam(teams);
                textMeshPro.fontSize = textMeshPro.fontSizeMax = textMeshPro.fontSizeMin = 2f;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.gameObject.layer = 5;
                textMeshPro.color = Color.black;
                textMeshPro.autoSizeTextContainer = true;
                textMeshPro.fontStyle = TMPro.FontStyles.Bold;
                textMeshPro.m_sharedMaterial = PlayerControl.LocalPlayer.cosmetics.nameText.m_sharedMaterial;
                PassiveButton passiveButton = spriteRenderer.gameObject.AddComponent<PassiveButton>();
                var box2d = passiveButton.gameObject.AddComponent<BoxCollider2D>();
                box2d.size = spriteRenderer.bounds.size;
                passiveButton.Colliders = new[] { box2d };
                passiveButton.OnClick = new();
                passiveButton.OnMouseOut = new();
                passiveButton.OnMouseOver = new();
                passiveButton._CachedZ_k__BackingField = 0.1f;
                passiveButton.CachedZ = 0.1f;
                return spriteRenderer;
            }
            else
            {
                SpriteRenderer
                spriteRenderer = new GameObject(teams.ToString()).AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(parent);
                spriteRenderer.sprite = Sprites.GetSpriteFromResources("ui.option.png", 350f);
                spriteRenderer.transform.localPosition = new Vector3(-2f, 2.2f, -10);
                spriteRenderer.transform.localScale = Vector3.one;
                spriteRenderer.enabled = true;
                spriteRenderer.gameObject.layer = Data.UILayer;
                spriteRenderer.gameObject.SetActive(true);
                TextMeshPro textMeshPro = new GameObject($"Guesser_Other_Button").AddComponent<TextMeshPro>();
                textMeshPro.transform.SetParent(spriteRenderer.transform);
                textMeshPro.gameObject.layer = Data.UILayer;
                textMeshPro.transform.localScale = Vector3.one;
                textMeshPro.transform.localPosition = new Vector3(0f, 0.02f, -1f);
                textMeshPro.text = RoleData.GetOtherColoredName;
                textMeshPro.fontSize = textMeshPro.fontSizeMax = textMeshPro.fontSizeMin = 2f;
                textMeshPro.alignment = TextAlignmentOptions.Center;
                textMeshPro.gameObject.layer = 5;
                textMeshPro.color = Color.black;
                textMeshPro.autoSizeTextContainer = true;
                textMeshPro.fontStyle = TMPro.FontStyles.Bold;
                textMeshPro.m_sharedMaterial = PlayerControl.LocalPlayer.cosmetics.nameText.m_sharedMaterial;
                PassiveButton passiveButton = spriteRenderer.gameObject.AddComponent<PassiveButton>();
                passiveButton.HeldButtonSprite = spriteRenderer;
                var box2d = passiveButton.gameObject.AddComponent<BoxCollider2D>();
                box2d.size = spriteRenderer.bounds.size;
                passiveButton.Colliders = new[] { box2d };
                passiveButton.OnClick = new();
                passiveButton.OnMouseOut = new();
                passiveButton.OnMouseOver = new();
                passiveButton._CachedZ_k__BackingField = 0.1f;
                passiveButton.CachedZ = 0.1f;
                return spriteRenderer;
            }

        }
        public static PlayerControl targetplayer;

        public class Target
        {
            public GameObject gameObject;
            public PlayerVoteArea voteArea;
            public int playerId;
            public PassiveButton passiveButton;
            public SpriteRenderer renderer;
            public Target(PlayerVoteArea playerVoteArea, MeetingHud meeting)
            {
                this.voteArea = playerVoteArea;
                this.playerId = playerVoteArea.TargetPlayerId;
                this.passiveButton = new PassiveButton();
                this.gameObject = new();
                gameObject.name = "TargetButton";
                gameObject.transform.SetParent(playerVoteArea.transform);
                gameObject.transform.localPosition = new(-0.9f, 0, -1);
                this.renderer = gameObject.AddComponent<SpriteRenderer>();
                renderer.sprite = Sprites.GetSpriteFromResources("ui.target.png", 800f);
                renderer.gameObject.layer = HudManager.Instance.gameObject.layer;
                //renderer.material = MeetingHud.Instance.SkipVoteButton.GetComponent<SpriteRenderer>().material;

                renderer.color = Helper.ColorEditHSV(Color.white, a: 0.6f);
                var box = gameObject.AddComponent<BoxCollider2D>();
                box.size = renderer.bounds.size;
                passiveButton = gameObject.AddComponent<PassiveButton>();
                passiveButton.OnClick = new();
                passiveButton.OnMouseOut = new();
                passiveButton.OnMouseOver = new();
                passiveButton._CachedZ_k__BackingField = 0.1f;
                passiveButton.CachedZ = 0.1f;
                passiveButton.Colliders = new[] { box };
                passiveButton.OnClick.AddListener((System.Action)(() =>
                {
                    Logger.Info( $"{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).Data.PlayerName} is targeting");
                    targetplayer = DataBase.AllPlayerControls().First(x=>x.PlayerId==playerVoteArea.TargetPlayerId);
                    ChoiceRole(meeting);

                }));
                passiveButton.OnMouseOver.AddListener((System.Action)(() =>
                {
                    renderer.color = Color.red;
                }));
                passiveButton.OnMouseOut.AddListener((System.Action)(() =>
                {

                    renderer.color = Helper.ColorEditHSV(Color.white, a: 0.6f);
                }));
            }
        }
    }
}
