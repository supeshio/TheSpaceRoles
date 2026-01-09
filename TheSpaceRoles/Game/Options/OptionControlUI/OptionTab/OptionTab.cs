using HarmonyLib;
using System.Collections.Generic;
using TSR.Game.Options.OptionControlUI.Scroll;
using TSR.Patch;
using UnityEngine;

namespace TSR.Game.Options.OptionControlUI.OptionTab
{
    public static class OptionTab
    {
        public static OptionTabBase ClassicTab;
        public static OptionTabBase RoleTab;

        public static void CreateOptionTabs()
        {
            OptionTabBase.TabBases = new();
            ClassicTab = new OptionTabBase(OptionTabType.Classic, -8f);
            RoleTab = new OptionTabBase(OptionTabType.Role, -5f);
            ClassicTab.TabButton.OnClick.Invoke();
        }

        public class OptionTabBase
        {
            public static List<OptionTabBase> TabBases = new();

            public string TranslatedName;
            public GameObject TabInner;
            public PassiveButton TabButton;
            public SpriteRenderer TabBGSp;
            public AUScroller SelectorTabScroller;
            public AUScroller MainTabScroller;
            public SpriteMask Mask;
            public static OptionTabType Selection = OptionTabType.Classic;
            public static Color32 EnabledColor = Helper.ColorEditHSV(Helper.ColorFromColorcode("#77ffff"), a: 0.5f);
            public static Color32 DisabledColor = Helper.ColorEditHSV(Helper.ColorFromColorcode("#ffffff"), a: 0f);
            public static Color32 HoveredColor = Helper.ColorEditHSV(Helper.ColorFromColorcode("#dddddd"), a: 0.5f);
            public const float SelectorGroupSizeX = 3f;
            public const float MainGroupSizeX = 6f;
            public OptionTabBase(OptionTabType TabType, float x)
            {
                TranslatedName = Translation.Get($"option.type.{TabType}");
                TabButton = new GameObject(TabType.ToString() + "TabButton").AddComponent<PassiveButton>();

                TabInner = new GameObject(TabType.ToString() + "TabInner");
                TabInner.transform.SetParent(OptionUIManager.TabInners.transform, false);
                TabInner.transform.localPosition = Vector3.zero;

                //左側の方:selectortab
                SelectorTabScroller = AUScroller.CreateScroller(OptionUIManager.MaskLayerL,TabInner.transform, "SelectorGroup", new Vector3(-4.3f,1.4f, -1.1f),4.5f,new Vector2(SelectorGroupSizeX+AUScroller.Space*2,4f));

                //右側の方:maintab
                MainTabScroller = AUScroller.CreateScroller(OptionUIManager.MaskLayerL,TabInner.transform, "MainGroup",new Vector3(-1f,1.4f,-1.1f ), 4.5f, new Vector2(MainGroupSizeX+AUScroller.Space*2, 4f));
                //TabButton.transform.SetParent(OptionUIManager.Header);
                TabButton.transform.SetParent(OptionUIManager.Header.transform.FindChild("Tabs"), false);
                TabButton.transform.localPosition = new Vector3(x, 0, -20);
                Logger.Info(TabButton.transform.localPosition.ToString());
                var TabSp = TabButton.gameObject.AddComponent<SpriteRenderer>();
                TabSp.sprite = Assets.AssetLoader.Sprites["engineer_repair"];

                TabSp.size *= 0.5f;

                var col = TabButton.gameObject.AddComponent<CircleCollider2D>();
                col.radius = 0.5f;

                TabBGSp = new GameObject("BG").gameObject.AddComponent<SpriteRenderer>();
                TabBGSp.sprite = Assets.AssetLoader.Sprites["circle"];
                TabBGSp.transform.SetParent(TabSp.transform);
                TabBGSp.transform.localPosition = new Vector3(0, 0, 10);
                TabBGSp.size = Vector2.one;
                TabBGSp.gameObject.layer = 5;
                TabBGSp.transform.localScale = Vector2.one * 0.7f;
                if (Selection == TabType) { TabBGSp.color = EnabledColor; }
                else { TabBGSp.color = DisabledColor; }
                TabButton.Set(OnClick: () => { Selection = TabType; TabBases.Do(x => x.TabBGSp.color = DisabledColor); TabBGSp.color = EnabledColor; TabBases.Do(x => x.TabInner.gameObject.SetActive(false)); TabInner.gameObject.SetActive(true); }, OnHoverIn: () =>
                {
                    if (Selection == TabType) { TabBGSp.color = EnabledColor; }
                    else { TabBGSp.color = HoveredColor; }
                }, OnHoverOut: () =>
                {
                    if (Selection == TabType) { TabBGSp.color = EnabledColor; }
                    else { TabBGSp.color = DisabledColor; }
                });
                TabButton.Colliders = new[] { col };
                TabBases.Add(this);
                
            }
        }

        public enum OptionTabType
        {
            Classic = 0,
            Role,
        }
    }
}