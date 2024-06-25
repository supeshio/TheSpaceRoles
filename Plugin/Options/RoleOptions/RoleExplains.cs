//using System;
//using TMPro;
//using UnityEngine;

//namespace TheSpaceRoles
//{
//    public enum SelectingType
//    {
//        None,
//        Team,
//        Role,
//        AddedRole,

//    }
//    public static class RoleOptionsDescription
//    {
//        public static TextMeshPro Title;//役職名/陣営名
//        public static TextMeshPro Intro;//intro
//        public static TextMeshPro Description;
//        public static void StartExplain()
//        {
//            /*Added:
//             * 陣営 /TA
//             * 役職 /RA
//             * 勝利条件
//             * 占い結果()
//             * 霊媒結果()
//             * 能力
//             */

//            if (Description != null) return;

//            GameObject g = new GameObject("Description");
//            g.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").FindChild("Description"));
//            g.transform.localPosition = Vector3.zero;


//            Title = new GameObject("Title").AddComponent<TextMeshPro>();
//            Title.gameObject.layer = Data.UILayer;
//            Title.text = "";
//            Title.fontStyle = FontStyles.Bold;
//            Title.transform.SetParent(g.transform);
//            Title.transform.localPosition = new Vector3(2.3f, 1.9f, 0);
//            Title.m_sharedMaterial = Data.textMaterial;
//            Title.rectTransform.pivot = new Vector2(0.5f, 0.5f);
//            Title.rectTransform.sizeDelta = new Vector2(3.0f, 2.0f);
//            Title.fontSize = 4f;
//            Title.fontSizeMax = 4f;
//            Title.fontSizeMin = 1.0f;
//            Title.alignment = TextAlignmentOptions.Center;
//            Title.enableWordWrapping = false;
//            Title.autoSizeTextContainer = false;
//            Title.enableAutoSizing = true;

//            Intro = new GameObject("Intro").AddComponent<TextMeshPro>();
//            Intro.gameObject.layer = Data.UILayer;
//            Intro.text = "";
//            Intro.fontStyle = FontStyles.Bold;
//            Intro.transform.SetParent(g.transform);
//            Intro.transform.localPosition = new Vector3(2.3f, 1.5f, 0);
//            Intro.m_sharedMaterial = Data.textMaterial;
//            Intro.rectTransform.pivot = new Vector2(0.5f, 0.5f);
//            Intro.rectTransform.sizeDelta = new Vector2(2.5f, 0.3f);
//            Intro.fontSize = 2.8f;
//            Intro.fontSizeMax = 3f;
//            Intro.fontSizeMin = 2f;
//            Intro.alignment = TextAlignmentOptions.Center;
//            Intro.enableWordWrapping = false;
//            Intro.autoSizeTextContainer = false;
//            Intro.enableAutoSizing = true;

//            string r = "";
//            foreach (Teams item in Enum.GetValues(typeof(Teams)))
//            {
//                r += Helper.ColoredText(GetLink.ColorFromTeams(item), Translation.GetString("role." + item.ToString() + ".sname")) + ",";
//            }

//            Description = new GameObject("Description").AddComponent<TextMeshPro>();
//            Description.gameObject.layer = Data.UILayer;
//            Description.text = Translation.GetString("canvisibleteam", [r]);
//            //Description.fontStyle = FontStyles.Bold;
//            Description.transform.SetParent(g.transform);
//            Description.transform.localPosition = new Vector3(2.3f, 1.2f, 0);
//            Description.m_sharedMaterial = Data.textMaterial;
//            Description.rectTransform.pivot = new Vector2(0.5f, 1f);
//            Description.rectTransform.sizeDelta = new Vector2(5f, 5f);
//            Description.fontSize = 1.2f;
//            Description.fontSizeMax = 2f;
//            Description.fontSizeMin = 1f;
//            Description.alignment = TextAlignmentOptions.Top;
//            Description.enableWordWrapping = true;
//            Description.autoSizeTextContainer = false;
//            Description.enableAutoSizing = false;
//        }

//        public static void Set(Roles roles)
//        {
//            var c = GetLink.GetCustomRole(roles);
//            SetDescription(c.ColoredRoleName, c.ColoredIntro, c.Description());

//        }
//        public static void Set(Teams teams)
//        {
//            if ((int)teams == -1)
//            {
//                SetDescription(Helper.ColoredText(Color.white, Translation.GetString("team.additional.name")), Helper.ColoredText(Color.white, Translation.GetString("team.additional.intro")), Helper.ColoredText(Color.white, Translation.GetString("team.additional.description")));
//            }
//            else
//            {

//                var t = GetLink.GetCustomTeam(teams);
//                if (t == null) Logger.Info("t is null");
//                SetDescription(t.ColoredTeamName, t.ColoredIntro, t.Description);
//            }
//        }
//        public static void Set(Teams teams, Roles roles)
//        {
//            if ((int)teams == -1)
//            {

//                var r = GetLink.GetCustomRole(roles);
//                SetDescription(r.ColoredRoleName + Translation.GetString("team.additional.sname"), r.ColoredIntro, "\n" + r.Description()); ;
//            }
//            else
//            {

//                var r = GetLink.GetCustomRole(roles);
//                var t = GetLink.GetCustomTeam(teams);
//                SetDescription(r.ColoredRoleName + t.ColoredShortTeamName, r.ColoredIntro, t.WinConditionTeam + "\n" + r.Description());
//            }

//        }
//        public static void SetDescription(string Title_, string Intro_, string Description_)
//        {
//            if (Title != null)
//            {

//                Title.text = Title_;
//                Intro.text = Intro_;
//                Description.text = Description_;
//            }
//        }
//    }
//}
