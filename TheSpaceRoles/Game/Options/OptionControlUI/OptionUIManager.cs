using Il2CppInterop.Runtime;
using TSR.Game.Options.OptionControlUI.OptionTab.Option;
using TSR.Game.Options.OptionControlUI.Scroll;
using TSR.Patch;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static PlayerMaterial;

namespace TSR.Game.Options.OptionControlUI;

/** BG /CustomOptionBG  /ScrollBar
 *                      /Inner  /
 *
 *     /CustomOptionTabBG
 *     /ShowRoleBG
 *     /SelectButtonBG
 *
 *
 *
 *
 *
 *
 *
 */
[SmartPatch]
public static class OptionUIManager
{
    public const int MaskLayerL = 63;
    public static GameObject OptionUI;
    public static GameObject BackGround;
    public static GameObject TabInners;
    public static GameObject[] Tabs;
    public static GameObject Header;
    public static GameObject Tint;
    private static SpriteMask _scrollSpriteMask;
    private static RectMask2D _rectMask2D;
    private static MeetingHud meetinghud => Resources.FindObjectsOfTypeAll(Il2CppType.Of<MeetingHud>())[0].Cast<MeetingHud>();

    public static void Create()
    {
        //大本のOptionUI作成 これがすべてのparent
        OptionUI = new GameObject("OptionUI");
        OptionUI.layer = Helper.UILayer;//UI LAYER
        OptionUI.transform.SetParent(HudManager.Instance.transform.parent, false);
        OptionUI.transform.localPosition = new Vector3(0, 0, -50);
        
        //maskをつくる
        //p = 1024
        // SelectorMask => p
        // MainMask => p+1
        // Optionの数だけnを増やしp+2+nの演算 <- *これはやらなくてもいいかもしれないので一旦やらない感じで*
        
        _scrollSpriteMask = new GameObject("ScrollMask").AddComponent<SpriteMask>();
        _scrollSpriteMask.transform.SetParent(OptionUI.transform);
        _scrollSpriteMask.sprite = Assets.AssetLoader.Sprites["square_topleft"];
        _scrollSpriteMask.gameObject.layer = Helper.UILayer;
        _scrollSpriteMask.transform.localPosition = new Vector3(-12f, 1.4f, -1.1f);
        _scrollSpriteMask.transform.localScale = new Vector3(24f, 4.5f + AUScroller.Space*2, 1f);
        _scrollSpriteMask.frontSortingOrder = -999;
        _scrollSpriteMask.backSortingOrder = -1001;
        _scrollSpriteMask.sortingOrder = -1000;
        _scrollSpriteMask.isCustomRangeActive = true;
        _scrollSpriteMask.alphaCutoff = 0;
        //_scrollSpriteMask.drawMode = SpriteDrawMode.Sliced;
        // _scrollSpriteMask.material.renderQueue = 1900;
        // _scrollSpriteMask.material.SetInt(PlayerMaterial.MaskLayer, MaskLayerL);
        
        //背景処理をパクる

        // BackGround = GameObject.Instantiate(Helper.ClassLib<PlayerCustomizationMenu>().transform.FindChild("Background").FindChild("BgBlocker").gameObject);
        // BackGround.transform.SetParent(OptionUI.transform);
        // BackGround.transform.localPosition = new Vector3(0, 0, 5);
            

        //パクらず 素材を使ってみる;
        var bg = new GameObject("Background").AddComponent<SpriteRenderer>();
        bg.sprite = Assets.AssetLoader.Sprites["starry_sky"];
        bg.transform.SetParent(OptionUI.transform, false);
        bg.transform.localPosition = new Vector3(0,0,-5f);
        bg.gameObject.layer = Helper.UILayer;
        bg.transform.localScale = new Vector2(1,1);
        bg.color = Helper.ColorFromColorcode("#aaa");
            
        Tint = GameObject.Instantiate(Helper.ClassLib<PlayerCustomizationMenu>().transform.FindChild("Tint")
            .gameObject);
        Tint.transform.SetParent(OptionUI.transform);
        Tint.transform.localPosition = new Vector3(0, 0, 0);
            
        //Header
        Header = GameObject.Instantiate(Helper.ClassLib<PlayerCustomizationMenu>().transform.FindChild("Header").gameObject);
        Header.transform.SetParent(OptionUI.transform);
        Header.transform.localPosition = new Vector3(0, 2.6408f, -50);
        for (int i = 0; i < Header.transform.FindChild("Tabs").GetChildCount(); i++)
        {
            var tab = Header.transform.FindChild("Tabs").GetChild(i);
            if (tab.gameObject.name.Contains("Container"))
            {
            }
            else
            {
                GameObject.Destroy(tab.gameObject);
            }
        }
            
        //Header.transform.FindChild("Tabs").gameObject.SetActive(true);
        //閉じるボタンの設定
        PassiveButton CloseButton = Header.transform.FindChild("CloseButton").gameObject.GetComponent<PassiveButton>();
        CloseButton.OnClick = new();
        CloseButton.OnClick.AddListener((UnityAction)(() => { ShowHide(false); }));
        //GameObject.Destroy(BackGround.transform.FindChild("RightPanel").gameObject);

        TabInners = new GameObject("TabInners");
        TabInners.transform.SetParent(OptionUI.transform);
        TabInners.transform.localPosition = Vector3.zero;

        /* 作る順番について
         *  1. 上の[設定タブ] - OptionTab
         *
         *  役職の場合/普通の場合
         *
         *  2-1. 役職タブスクロール
         *  2-2. 左の[役職タブ] - RoleTab
         *  2-3. 役職タブフィルタ
         *  (2-4. 役職早見表)
         *
         *  3-1. 役職名,役職Inro
         *  3-2. 役職設定タブスクロール
         *  3-3. [役職設定タブ](役職desc) - RoleSettingTab
         *
         *  /ふつうの設定の場合
         *
         *  2-1. クラシックタブスクロール
         *  2-2. [クラシックタブ] ClassicTab
         *  ~~2-3. クラシックタブフィルタ?いらんな消そう~~
         *
         *  3-1. クラシック設定名,(設定イントロ?)
         *  3-2. クラシック設定スクロール
         *  3-3. [クラシック設定] ClassicSettingTab
         */

        OptionTab.OptionTab.CreateOptionTabs();

        OptionLevelManager.PrepareOptions();
               
               
               
        //TODO:
        // option側にVisible in side をつける
        // 1.4f ~ -3.1f
        // この範囲(y)でクロップ
               
        //var settingmenu = Helper.ClassLib<GameSettingMenu>().transform.FindChild("Background");
        //for (int i = 0; i < settingmenu.childCount; i++)
        //{
        //    var g = GameObject.Instantiate(settingmenu.GetChild(i).gameObject);
        //    g.transform.SetParent(BackGround.transform, false);
        //}
        //var k = GameObject.Instantiate(Helper.ClassLib<PlayerCustomizationMenu>().transform.FindChild("Background").gameObject);

        //return;Transform TabParent = OptionUI.transform;
        /*

        var CustomOptionTabManager = new GameObject("CustomOptionTabManager");
        CustomOptionTabManager.transform.SetParent(OptionUI.transform);
        CustomOptionTabManager.transform.localPosition = Vector3.zero;
        CustomOptionTabInner = new GameObject("Inner");
        CustomOptionTabInner.transform.SetParent(CustomOptionTabManager.transform);
        CustomOptionTabInner.transform.localPosition = Vector3.zero;
        var TabMenu = new OptionAsset.CustomOptionBG("TabMenu", CustomOptionTabManager.transform, new(-2.52f, -0.77f, 0.5f), new(2.7f, 3.5f), new Color32(255, 195, 195, 255));

        var CustomOptionManager = new GameObject("CustomOptionManager");
        CustomOptionManager.transform.SetParent(OptionUI.transform);
        CustomOptionManager.transform.localPosition = Vector3.zero;
        var OptionMenu = new OptionAsset.CustomOptionBG("OptionMenu", CustomOptionManager.transform, new(1.4f, -0.77f, 0.5f), new(5f, 3.5f), new Color32(195, 215, 255, 255));

        var RolePreviewManager = new GameObject("RolePreviewManager");
        RolePreviewManager.transform.SetParent(OptionUI.transform);
        RolePreviewManager.transform.localPosition = Vector3.zero;
        var RolePrviewMenu = new OptionAsset.CustomOptionBG("RolePreviewMenu", TabParent, new(0.01f, 1.215f, 0.5f), new(7.755f, 0.4f), new Color32(255, 255, 195, 255));

        var CustomOptionControlTabManager = new GameObject("CustomOptionControlTabManager");
        CustomOptionControlTabManager.transform.SetParent(OptionUI.transform);
        CustomOptionControlTabManager.transform.localPosition = Vector3.zero;
        var CustomOptionControlTabInner = new GameObject("Inner");
        CustomOptionControlTabInner.transform.SetParent(CustomOptionTabManager.transform);
        CustomOptionControlTabInner.transform.localPosition = Vector3.zero;

        //各ManagerにMaskをつける処理
        List<OptionAsset.CustomOptionBG> Menus = [TabMenu, OptionMenu, RolePrviewMenu];
        for (int i = 0; i < Menus.Count; i++)
        {
            var mask = new GameObject("Mask").AddComponent<SpriteMask>();
            mask.gameObject.layer = 5;
            mask.transform.SetParent(Menus[i].SpRenderer.transform);
            //mask.isCustomRangeActive = true;
            mask.transform.localPosition = new(0, 0, 0);
            switch (i)
            {
                case 0:
                    mask.transform.localScale = new Vector3(1.94f * 4, 2.72f, 1);
                    break;
            }

            mask.sprite = AssetLoader.Sprites["option_bg3"];
            mask.isCustomRangeActive = true;
            mask.frontSortingOrder = (i + 8) * 2;
            mask.backSortingOrder = (i + 7) * 2;
        }

        //optioncontroltab
        OptionControlTab.OptionControlTabs.Clear();
        Scroll.Scrollbar.bar.Clear();
        foreach (OptionControlTab.ControlTabId k in Enum.GetValues(typeof(OptionControlTab.ControlTabId)))
        {
            _ = new OptionControlTab(k, k.ToString(), CustomOptionControlTabInner.transform);
        }

        var CustomOptionTabScroll = new Scroll.Scrollbar(CustomOptionTabManager.transform, new Vector2(-2.52f + 1.15f, -0.77f), 3.2f, 18f, new Vector2(-2.52f, -0.77f), new(5f, 3.5f), CustomOptionTabInner.transform, 0);

        //Scroll.Scrollbar optionscrollbar = new(OptionMenu.SpRenderer.transform, new Vector2(4.5f, 0), 1.6f, 18f, new Vector2(0, 0), new(5f, 3.5f),Transform);
        OptionControlTab.OptionControlTabs[OptionControlTab.ControlTabId.ClassicSetting].Menu.SetActive(true);

        //ClassicTabs
        CustomOptionTab.Tab[] classictabs =
            {
            CustomOptionTab.Tab.InformationEquipment,
            CustomOptionTab.Tab.Map,
            CustomOptionTab.Tab.Task,
            CustomOptionTab.Tab.Meeting,
            CustomOptionTab.Tab.Game,
        };
        for (int i = 0; i < classictabs.Length; i++)
        {
            CustomOptionTab tab = new(classictabs[i], OptionControlTab.ControlTabId.ClassicSetting, i);
        }
        //roleassignTabs
        for (int i = 0; i < Enum.GetValues<TeamId>().Length; i++)
        {
            CustomOptionTab tab = new((CustomOptionTab.Tab)Enum.GetValues<TeamId>()[i] + 10000, OptionControlTab.ControlTabId.RoleAssigner, i);
        }
        //rolesetting
        for (int i = 0; i < Enum.GetValues<RoleId>().Length; i++)
        {
            CustomOptionTab tab = new((CustomOptionTab.Tab)Enum.GetValues<RoleId>()[i] + 10000, OptionControlTab.ControlTabId.RoleAssigner, i);
        }
        //Scroll.Scrollbar optionscrollbar = new(OptionMenu.SpRenderer.transform, new Vector2(4.5f, 0), 1.6f, 18f, new Vector2(0, 0), new(5f, 3.5f),Transform);

        for (int i = 0; i < CustomOptionHolder.Options.Count; i++)
        {
            CustomOptionHolder.Options[i].CreateOptionUI();
        }

        */
        show = !false;
        ShowHide();
    }

    public static bool show = true;

    public static void ShowHide(bool? showb = null)
    {
        if (showb != null)
        {
            show = showb ?? true;
        }
        else
        {
            show = !show;
        }

        OptionUI.gameObject.SetActive(FGameManager.AmHost && show);
    }


    [SmartPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Close)), SmartPostfix]
    public static void Close()
    {
        Logger.Info("GameSettingMenu:Close");
        ShowHide(false);
    }

    [SmartPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start)), SmartPostfix]
    public static void Start(GameSettingMenu __instance)
    {
        Logger.Info("GameSettingMenu:Start");
        ShowHide(true);
        foreach (var child in __instance.transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name != "Background")
            {
                //child.localPosition += new Vector3(10000, 0, 0);
            }
        }
    }
}