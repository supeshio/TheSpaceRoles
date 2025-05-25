using System.Linq;
using TMPro;
using UnityEngine;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    /*https://discord.com/channels/1213452258847490048/1273959213961445487/1273959228511490080*/
    /***[役職名/Name]**

    - トリビア/Trivia

    **[イントロ/Introduction]**

    -  知識こそが村を導く

    **[陣営/Team]**

    - クルーメイト陣営

    **[役職の色]**

    - #00B5FF

    **[説明/Description]**

    - 選択した役職の生存人数を確認できる役職です
    会議中で確認したい役職を選択すると、チャットにその役職の人が何人生存してるかが見れます

    **[ボタン/Buttons]**

    - **[知識]**
    - 発動条件:残りの発動可能回数が1以上のときに会議に入っていること
    - 効果: 会議中に発動できます
         ゲッサーの画面のように役職一覧が出せてそこで役職を選択すると、
         チャットに選択した役職を持ってる人の残りの生存人数を見ることができます
         発動回数には制限があります
         陣営の人数が確認できるように設定すれば、候補がインポスター、ニュートラル、クルーメイト
         のみになり、細かな役ではなく陣営の大まかな人数が把握できます
         設定次第でクルー役、クルーメイトのみ確認できなくなります
         クルー役が確認できないかは、陣営で確認するかがオフにしているときにのみ設定できます
         陣営で確認するか、クルーメイトが確認できないかの両方をオンにしていれば、
         クルー陣営(クルー役とクルーメイト)が確認できなくなります
         全役職が一度に確認できる設定だと、選択画面を開かずに生きている役のみを見れます
         陣営で確認するか、全役職を一度に確認できるかの両方を
         オンにしていると、どの陣営が何人生きてるかを一度に見れます

    **[役職設定/Settings]**

    - 発動可能回数
    - 初期値 : 1 , 最小値 1,最大値 5

    - クルーメイトが確認できないか
    - 初期値 : off

    - 陣営で確認するか
    - 初期値 : off

    - クルー役職が確認できないか
    - 初期値 : off

    - 全役職の生存人数を一度に確認できるか
    - 初期値 : off

    **[その他/others]**
    - 主な使い方は、盤面把握です
    例:ローラーしたときに能力を使い、キル役の候補役職が減っていたらロラストなど
    またクルー役が見れる設定のとき
    役騙りをしているかなども確認することができるため、協力な役職だと言えます*/
    public class Trivia : CustomRole
    {
        public Trivia()
        {
            Team = Teams.Crewmate;
            Role = Roles.Trivia;
            Color = Helper.ColorFromColorcode("#005BFF");
        }
        public static CustomOption IdeaCount;
        public static CustomOption IdeaOfMeeting;
        //public static CustomOption CanIdeaCrewmateTeam;
        public override void OptionCreate()
        {
            if (IdeaCount != null) return;

            IdeaCount = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.trivia.ideacount", new CustomIntRange(1, 15, 1), 1);
            IdeaOfMeeting = Create(CustomOption.OptionType.Crewmate, "role.trivia.ideacountofmeeting", new CustomIntRange(1, 15, 1), 1);
            //CanIdeaCrewmateTeam = Create(CustomOption.OptionType.Crewmate, "role.trivia.canideacrewmate", true);

            Options = [IdeaCount, IdeaOfMeeting];
        }
        public static int remainIdea;
        public static int remainIdeaOfMeeting;
        public override void HudManagerStart(HudManager hudManager)
        {
            remainIdea = (int)IdeaOfMeeting.GetIntValue();
        }
        public override void MeetingStart(MeetingHud meeting)
        {
            remainIdeaOfMeeting = (int)IdeaCount.GetIntValue();
            Button(meeting);
        }
        public void UsedIdea()
        {
            remainIdea--;
            remainIdeaOfMeeting--;
            Button(MeetingHud.Instance);
        }
        public GameObject gameObject;
        public PlayerVoteArea voteArea;
        public PassiveButton passiveButton;
        public SpriteRenderer renderer;
        public void Button(MeetingHud meeting)
        {
            if (meeting?.meetingContents == null) return;
            if (remainIdea <= 0 || remainIdeaOfMeeting <= 0) return;
            this.gameObject = new();
            gameObject.name = "TargetButton";
            gameObject.transform.SetParent(meeting.meetingContents.transform);
            gameObject.transform.localPosition = new(0f, -1.8f, -10);
            this.renderer = gameObject.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("ui.target.png", 800f);
            renderer.gameObject.layer = HudManager.Instance.gameObject.layer;

            renderer.color = Helper.ColorEditHSV(Color.gray);
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
                ShowRoles(meeting);
                GameObject.Destroy(gameObject);
                try
                {

                    passiveButton.OnClick = new();
                    UnityEngine.Object.Destroy(passiveButton);
                }
                catch { }

            }));
            passiveButton.OnMouseOver.AddListener((System.Action)(() =>
            {
                renderer.color = Color.white;
            }));
            passiveButton.OnMouseOut.AddListener((System.Action)(() =>
            {

                renderer.color = Helper.ColorEditHSV(Color.gray);
            }));
        }
        public static SpriteRenderer crewmateRend;
        public static SpriteRenderer impostorRend;
        public static SpriteRenderer neutralRend;
        public int selected = 0;
        public void ShowRoles(MeetingHud meeting)
        {
            GameObject.Destroy(gameObject);
            crewmateRend = null;
            impostorRend = null;
            neutralRend = null;
            selected = 0;
            SpriteRenderer background = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("Background").GetComponent<SpriteRenderer>();
            SpriteRenderer basecolor = meeting.transform.FindChild("MeetingContents").FindChild("PhoneUI").FindChild("baseColor").GetComponent<SpriteRenderer>();
            Logger.Info(background.tag);
            Transform parent = meeting.meetingContents.transform.FindChild("PhoneUI");

            var crewteam = new GameObject("CrewTeamButtons");
            crewteam.transform.SetParent(parent);
            crewteam.transform.localPosition = Vector3.zero;
            var impteam = new GameObject("ImpostorTeamButtons");
            impteam.transform.SetParent(parent);
            impteam.transform.localPosition = Vector3.zero;
            var neuteam = new GameObject("NeutralTeamButtons");
            neuteam.transform.SetParent(parent);
            neuteam.transform.localPosition = Vector3.zero;

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
            //back button
            SpriteRenderer
            BackRend = new GameObject("BackButton").AddComponent<SpriteRenderer>();
            BackRend.transform.SetParent(parent);
            BackRend.sprite = Sprites.GetSpriteFromResources("ui.Cancel.png", 560f);
            BackRend.transform.localPosition = new Vector3(-4f, 0f, -10);
            BackRend.transform.localScale = Vector3.one;
            BackRend.enabled = true;
            BackRend.gameObject.layer = Data.UILayer;
            BackRend.gameObject.SetActive(true);
            PassiveButton BackButton = BackRend.gameObject.AddComponent<PassiveButton>();
            var box2d = BackButton.gameObject.AddComponent<BoxCollider2D>();
            box2d.size = BackRend.bounds.size;
            BackButton.Colliders = new[] { box2d };
            BackButton.OnClick = new();
            BackButton.OnMouseOut = new();
            BackButton.OnMouseOver = new();
            BackButton._CachedZ_k__BackingField = 0.1f;
            BackButton.CachedZ = 0.1f;
            BackRend.gameObject.GetComponent<PassiveButton>().ClickSound = HudManager.Instance.Chat.chatButton.ClickSound;
            BackRend.gameObject.GetComponent<PassiveButton>().OnClick.AddListener((System.Action)(() =>
            {
                BackRend.color = Palette.EnabledColor;
                reset();
                BackRend.gameObject.SetActive(true);
                roleaction(Roles.None);
                Button(MeetingHud.Instance);

            }));
            BackRend.gameObject.GetComponent<PassiveButton>().OnMouseOver.AddListener((System.Action)(() =>
            {
                BackRend.color = Color.white;
                reset();

            }));
            BackRend.gameObject.GetComponent<PassiveButton>().OnMouseOut.AddListener((System.Action)(() =>
            {
                BackRend.color = Color.gray;
                reset();
            }));




            //crew
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
            //if (!CanIdeaCrewmateTeam.GetBoolValue())
            //{

            //    selected = 1;
            //    reset();
            //    impteam.gameObject.SetActive(true);
            //    impostorRend.color = Palette.AcceptedGreen;
            //    crewmateRend.gameObject.SetActive(false);
            //}


            reset();
            crewteam.gameObject.SetActive(true);


            void roleaction(Roles role)
            {

                meeting.ButtonParent.gameObject.SetActive(true);
                try
                {
                    GameObject.Destroy(crewteam.gameObject);
                    GameObject.Destroy(impteam.gameObject);
                    GameObject.Destroy(neuteam.gameObject);
                    GameObject.Destroy(crewmateRend.gameObject);
                    GameObject.Destroy(impostorRend.gameObject);
                    GameObject.Destroy(neutralRend.gameObject);
                    GameObject.Destroy(BackRend.gameObject);
                    crewmateRend = null;
                    impostorRend = null;
                    neutralRend = null;
                }
                catch
                {

                }
                if (role == Roles.None)
                {
                    return;
                }
                int rolec = 0;
                foreach (var pr in DataBase.AllPlayerData)
                {
                    if (pr.Value.CustomRole.Role == role)
                    {
                        int p = pr.Key;
                        if (!Helper.GetPlayerById(p).Data.IsDead)
                        {
                            rolec++;
                        }

                    }
                }
                Helper.AddChat($"Role:{Helper.GetCustomRole(role).ColoredRoleName}の生存数 : {rolec}");
                UsedIdea();
                //Button(meeting);
            }


            int c = 0;
            int i = 0;
            int n = 0;
            Logger.Message(DataBase.AssignedRoles().Select(x => x.ToString()).ToArray().Joinsep("\n"));
            foreach (var role in DataBase.AssignedRoles())
            {

                SpriteRenderer rend;

                if (RoleData.GetCustomRoleFromRole(role).Team == Teams.Crewmate)
                {
                    rend = ButtonCreate(crewteam.transform, Teams.Crewmate);
                    rend.transform.localPosition = new Vector3(-2.7f + 1.8f * (c % 4), 1.6f - 0.4f * Mathf.Floor(c++ / 4f), -10);
                    //-2.7f, 1.6f, -10
                }
                else
                if (RoleData.GetCustomRoleFromRole(role).Team == Teams.Impostor)
                {
                    rend = ButtonCreate(impteam.transform, Teams.Impostor);
                    rend.transform.localPosition = new Vector3(-2.7f + 1.8f * (i % 4), 1.6f - 0.4f * Mathf.Floor(i++ / 4f), -10);
                }
                else
                {

                    rend = ButtonCreate(neuteam.transform, Teams.None);
                    rend.transform.localPosition = new Vector3(-2.7f + 1.8f * (n % 4), 1.6f - 0.4f * Mathf.Floor(n++ / 4f), -10);
                }
                var p = rend.gameObject.GetComponent<PassiveButton>();
                p.OnMouseOut = new();
                p.OnMouseOver = new();
                p.OnClick = new();
                p.ClickSound = HudManager.Instance.Chat.chatButton.ClickSound;
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
        public static SpriteRenderer ButtonCreate(Transform parent, Teams teams)
        {
            if (teams != Teams.None)
            {
                SpriteRenderer spriteRenderer = new GameObject(teams.ToString()).AddComponent<SpriteRenderer>();
                spriteRenderer.transform.SetParent(parent);
                spriteRenderer.sprite = Sprites.GetSpriteFromResources("ui.option.png", 350f);
                spriteRenderer.transform.localPosition = new Vector3(-2.7f, 1.6f, -10);
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
                spriteRenderer.transform.localPosition = new Vector3(-2.7f, 1.6f, -10);
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
    }
}
