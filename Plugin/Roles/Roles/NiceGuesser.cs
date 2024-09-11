using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace TheSpaceRoles
{
    public class NiceGuesser : CustomRole
    {
        public List<Target> targets = [];
        public NiceGuesser()
        {
            team = Teams.Crewmate;
            Role = Roles.NiceGuesser;
            Color = Helper.ColorFromColorcode("#FFCC00");
        }
        public override void HudManagerStart(HudManager hudManager)
        {
            targets = [];
        }
        public override void MeetingStart(MeetingHud meeting)
        {
            targets = [];
            foreach (var player in MeetingHud.Instance.playerStates)
            {
                if (player != null && !player.AmDead)
                {
                    targets.Add(new Target(player, meeting));
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
        public static void P(MeetingHud meeting)
        {

            SpriteRenderer background = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("Background").GetComponent<SpriteRenderer>();
            SpriteRenderer basecolor = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("baseColor").GetComponent<SpriteRenderer>();
            Logger.Info(background.tag);
            Transform parent = meeting.meetingContents.transform.FindChild("PhoneUI");
            // PhoneUI\
            //BackGround 0 0 7
            //baseColor 0.012 0 8
            meeting.ButtonParent.gameObject.SetActive(false);

            crewmateRend = ButtonCreate( parent, Teams.Crewmate);
            crewmateRend.transform.localPosition = new Vector3(-2f, 2.2f, -10);
            crewmateRend.transform.localScale = new(1.2f, 1.2f, 1.2f);
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOut = new();
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOver = new();
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnClick = new();
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                crewmateRend.color = Palette.AcceptedGreen;

            }));
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                crewmateRend.color = Color.gray;
            }));
            crewmateRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                crewmateRend.color = Color.white;
            }));

            impostorRend = ButtonCreate( parent, Teams.Impostor);
            impostorRend.transform.localPosition = new Vector3(0f, 2.2f, -10);
            impostorRend.transform.localScale = new(1.2f, 1.2f, 1.2f);

            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOut = new();
            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOver = new();
            impostorRend.gameObject.GetComponent<PassiveButton>().OnClick = new();
            impostorRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                impostorRend.color = Palette.AcceptedGreen;

            }));
            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                impostorRend.color = Color.gray;
            }));
            impostorRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                impostorRend.color = Color.white;
            }));
            neutralRend = ButtonCreate (parent, Teams.None);

            neutralRend.transform.localPosition = new Vector3(2f, 2.2f, -10);
            neutralRend.transform.localScale = new(1.2f, 1.2f, 1.2f);
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOut = new();
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOver = new();
            neutralRend.gameObject.GetComponent<PassiveButton>().OnClick = new();
            neutralRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                neutralRend.color = Palette.AcceptedGreen;

            }));
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                neutralRend.color = Color.gray;
            }));
            neutralRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                neutralRend.color = Color.white;
            }));










            var crewteam = new GameObject("CrewTeamButtons");
            crewteam.transform.localPosition = Vector3.zero;
            crewteam.transform.SetParent(parent);
            var impteam = new GameObject("ImpostorTeamButtons");
            impteam.transform.localPosition = Vector3.zero;
            impteam.transform.SetParent(parent);
            var neuteam = new GameObject("NeutralTeamButtons");
            neuteam.transform.localPosition = Vector3.zero;
            neuteam.transform.SetParent(parent);

            int c = 0;
            int i = 0;
            int n = 0;
            if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {

                foreach (var role in RoleData.GetCustomRoles.ToArray().Select(x=>x.Role))
                {
                    SpriteRenderer rend;

                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Crewmate)
                    {
                        rend = ButtonCreate(crewteam.transform, Teams.Crewmate);
                        rend.transform.localPosition = new Vector3(-2.2f + 1.375f * (c % 4), 1.6f - 0.3f * Mathf.Floor(c++ / 4f), -10);
                    }
                    else
                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Impostor)
                    {
                        rend = ButtonCreate(impteam.transform, Teams.Impostor);
                        rend.transform.localPosition = new Vector3(-2.2f + 1.375f * (i % 4), 1.6f - 0.3f * Mathf.Floor(i++ / 4f), -10);
                    }
                    else
                    {

                        rend = ButtonCreate(neuteam.transform, Teams.None);
                        rend.transform.localPosition = new Vector3(-2.2f + 1.375f * (n % 4), 1.6f - 0.3f * Mathf.Floor(n++ / 4f), -10);
                    }
                    var p = rend.gameObject.GetComponent<PassiveButton>();
                    p.OnMouseOut = new();
                    p.OnMouseOver = new();
                    p.OnClick = new();
                    p.OnClick.AddListener((System.Action)(() =>
                    {
                        rend.color = Palette.AcceptedGreen;

                    }));
                    p.OnMouseOver.AddListener((System.Action)(() =>
                    {
                        rend.color = Color.gray;
                    }));
                    p.OnMouseOut.AddListener((System.Action)(() =>
                    {
                        rend.color = Color.white;
                    }));
                    rend.GetComponentInChildren<TextMeshPro>().text = RoleData.GetColoredRoleNameFromRole(role);
                }
            }
            else
            {

                foreach (var role in DataBase.AssignedRoles)
                {

                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Crewmate)
                    {
                        var rend = ButtonCreate(crewteam.transform, Teams.Crewmate);
                        rend.transform.position = new Vector3(-2.2f + 1.375f * (c % 4), 1.6f - (0.3f * Mathf.Floor(c++ / 4f)), -30);
                        rend.transform.GetComponentInChildren<TextMeshPro>().text=RoleData.GetColoredRoleNameFromRole(role);

                    }
                    else
                    if (RoleData.GetCustomRoleFromRole(role).team == Teams.Impostor)
                    {
                        var rend = ButtonCreate(impteam.transform, Teams.Impostor);
                        rend.transform.position = new Vector3(-2.2f + 1.375f * (i % 4), 1.6f - (0.3f * Mathf.Floor(i++ / 4f)), -30);
                        rend.transform.GetComponentInChildren<TextMeshPro>().text = RoleData.GetColoredRoleNameFromRole(role);
                    }
                    else
                    {

                        var rend = ButtonCreate(neuteam.transform, Teams.None);
                        rend.transform.position = new Vector3(-1.8f + 1.2f * (n % 4), 1.6f - (0.3f * Mathf.Floor(n++ / 4f)), -30);
                        rend.transform.GetComponentInChildren<TextMeshPro>().text = RoleData.GetColoredRoleNameFromRole(role);
                    }

                }
            }
        }
        public static SpriteRenderer ButtonCreate(Transform parent,Teams teams)
        {
            if (teams != Teams.None)
            {
                SpriteRenderer
                spriteRenderer = new GameObject(teams.ToString()).AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(parent);
                spriteRenderer.sprite = Sprites.GetSpriteFromResources("ui.option.png", 380f);
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
                passiveButton.Colliders =new[] { box2d };
                passiveButton = spriteRenderer.gameObject.AddComponent<PassiveButton>();
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
                spriteRenderer.sprite = Sprites.GetSpriteFromResources("ui.option.png", 400f);
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
                passiveButton = spriteRenderer.gameObject.AddComponent<PassiveButton>();
                passiveButton.OnClick = new();
                passiveButton.OnMouseOut = new();
                passiveButton.OnMouseOver = new();
                passiveButton._CachedZ_k__BackingField = 0.1f;
                passiveButton.CachedZ = 0.1f;
                return spriteRenderer;
            }

        }

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

                var box = gameObject.AddComponent<BoxCollider2D>();
                box.size = renderer.bounds.size;
                passiveButton = gameObject.AddComponent<PassiveButton>();
                passiveButton.OnClick = new();
                passiveButton.OnMouseOut = new();
                passiveButton.OnMouseOver = new();
                passiveButton._CachedZ_k__BackingField = 0.1f;
                passiveButton.CachedZ = 0.1f;
                passiveButton.Colliders = new[] { gameObject.GetComponent<BoxCollider2D>() };
                passiveButton.OnClick.AddListener((System.Action)(() =>
                {
                    FastDestroyableSingleton<ChatController>.Instance.AddChat(PlayerControl.LocalPlayer, $"{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).Data.PlayerName}を狙撃対象にしたよ");
                    P(meeting);
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
