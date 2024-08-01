using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    public class Guesser : CustomRole
    {
        public List<Target> targets = [];
        public Guesser()
        {
            teamsSupported = GetLink.GetAllTeams();
            Role = Roles.Guesser;
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
        public static SpriteRenderer bg;
        public static SpriteRenderer bc;
        public static void P(MeetingHud meeting)
        {
            if (bg != null)
            {
                try
                {
                    GameObject.Destroy(bg.gameObject);
                    GameObject.Destroy(bc.gameObject);
                }
                finally
                {

                    bg = null;
                    bc = null;
                }

            }

            SpriteRenderer background = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("Background").GetComponent<SpriteRenderer>();
            SpriteRenderer basecolor = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("baseColor").GetComponent<SpriteRenderer>();
            Logger.Info(background.tag);
            Transform parent = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI");
            // PhoneUI\
            //BackGround 0 0 7
            //baseColor 0.012 0 8
            float times = 1.5f;
            var BG = new GameObject("GuesserSelecterBackGround");
            BG.layer = Data.UILayer;
            BG.transform.parent = parent;
            BG.transform.localScale = background.transform.localScale * times;
            BG.transform.localPosition = new Vector3(0f, 0, -16f);
            bg = BG.AddComponent<SpriteRenderer>();
            BG.transform.localScale = new(bg.transform.localScale.x, bg.transform.localScale.y, bg.transform.localScale.z);
            bg.sprite = background.sprite;
            bg.material = background.material;
            bg.size = background.size;

            var BC = new GameObject("GuesserSelecterBaseColor");
            BC.layer = Data.UILayer;
            BC.transform.parent = parent;
            BC.transform.localScale = basecolor.transform.localScale * times * 0.4f;
            BC.transform.localPosition = new Vector3(0, 0, -17f);
            bc = BC.AddComponent<SpriteRenderer>();
            BC.transform.localScale = new(bc.transform.localScale.x * 0.9f, bc.transform.localScale.y * 1.2f, bc.transform.localScale.z);
            bc.sprite = basecolor.sprite;
            bc.material = basecolor.material;
            bc.color = basecolor.color;
            bc.size = basecolor.size;
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
