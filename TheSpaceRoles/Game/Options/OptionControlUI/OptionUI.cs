using TSR.Module.SmartUIBuilder;
using TSR.Module.Translation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSR.Game.Options.OptionControlUI
{
    public static class OptionUI
    {
        public static GameObject? Option;
        public static RectTransform? CanvasRect;
        
        /// <summary>
        /// 上にあるタブ｡
        /// どの設定をいじるかを設定する｡
        /// </summary>
        private static Image SettingTabInner;
        /// <summary>
        /// メインのタブ｡
        /// 具体的な設定の変更はここで｡
        /// </summary>
        private static Image MainTabInner;
        /// <summary>
        /// メインタブ左のタブ｡
        /// どれをメインタブに表示するかを変更する｡
        /// </summary>
        private static Image SubTabInner;
        public static void Create()
        {
            //UIのcanvasを作成
            CreateOptionParents();
            
            Color32 bgColor = new Color32(73, 212, 243, 0xff);
            Color32 tabInnerColor = new Color32(32, 180, 200, 0xff);
            Color32 SettingTabInnerButtonColor = new Color32(16, 90, 100, 0xff);
            
            Logger.Info("Create Option Menu");
            //タブ作成
            CreateTabs(bgColor,tabInnerColor);
            CreateSettingTabs(SettingTabInner.rectTransform,SettingTabInnerButtonColor,Helper.ColorPalette.White);
            CreateSubTabs(SubTabInner.rectTransform);
            Hide();
        }

        public static void ShowSwitch()
        {
            if (Option == null) return;
            
            if (Option.active)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private static void Show()
        {
            Logger.Info("Show Option Menu");
            Option?.gameObject.SetActive(true);
        }

        private static void Hide()
        {
            Logger.Info("Hide Option Menu");
            Option?.gameObject.SetActive(false);
        }
        private static void CreateOptionParents()
        {
            (Option, CanvasRect) = UI.CreateRoot("UI", new Vector2(Screen.width, Screen.height));
        }

        private static void CreateTabs(Color bgColor,Color tabInnerColor)
        {
            
            //オプション内部を作る
            var bg = UI.Panel(CanvasRect,new Vector2(Screen.width,Screen.height)*0.85f,bgColor);
            
            SettingTabInner = UI.Panel(bg.transform,new Vector2(0,0),tabInnerColor);
            
            //SettingTabInner.rectTransform.anchoredPosition = new Vector3(0,0,0f);
            SettingTabInner.rectTransform.anchorMin = new Vector2(0.0f,0.8f);
            SettingTabInner.rectTransform.anchorMax = new Vector2(1.0f,1.0f);
            SettingTabInner.rectTransform.offsetMin = new Vector2(50, 0);
            SettingTabInner.rectTransform.offsetMax = new Vector2(-50,-50);
            
            SubTabInner = UI.Panel(bg.transform,new Vector2(0,0),tabInnerColor);
            SubTabInner.rectTransform.anchorMin = new Vector2(0.0f,0.0f);
            SubTabInner.rectTransform.anchorMax = new Vector2(0.3f,0.8f);
            SubTabInner.rectTransform.offsetMin = new Vector2(50, 50);
            SubTabInner.rectTransform.offsetMax = new Vector2(-25,-50);
            
            MainTabInner = UI.Panel(bg.transform,new Vector2(0,0),tabInnerColor);
            MainTabInner.rectTransform.anchorMin = new Vector2(0.3f,0.0f);
            MainTabInner.rectTransform.anchorMax = new Vector2(1.0f,0.8f);
            MainTabInner.rectTransform.offsetMin = new Vector2(25,50);
            MainTabInner.rectTransform.offsetMax = new Vector2(-50,-50);
        }

        private static void CreateSettingTabs(RectTransform SettingTab,Color ButtonColor,Color CharacterColor)
        {
            /*
             * 役職設定 <- 出現に関する設定も行う｡
             * ゲームプレイ設定
             */
            var horizontalGroup = UI.HorizontalGroup(SettingTab,childControlWidth:true);
            horizontalGroup.anchorMin = new Vector2(0.0f,0.0f);
            horizontalGroup.anchorMax = new Vector2(1.0f,1.0f);
            horizontalGroup.offsetMin = new Vector2(10,10);
            horizontalGroup.offsetMax = new Vector2(-10,-10);
            
            var gameSetting = UI.Panel(horizontalGroup,new Vector2(0,0),ButtonColor);
            gameSetting.rectTransform.anchorMin = new Vector2(0.0f,0.0f);
            gameSetting.rectTransform.anchorMax = new Vector2(1.0f,1.0f);
            gameSetting.rectTransform.offsetMin = new Vector2(2,2);
            gameSetting.rectTransform.offsetMax = new Vector2(-2,-2);
            
            var gameSettingText = UI.Text(gameSetting.rectTransform,Translation.Get("option.type.game"),color:CharacterColor);
            gameSettingText.rectTransform.anchorMin = new Vector2(0.0f,0.0f);
            gameSettingText.rectTransform.anchorMax = new Vector2(1.0f,1.0f);
            gameSettingText.rectTransform.offsetMin = new Vector2(2,2);
            gameSettingText.rectTransform.offsetMax = new Vector2(-2,-2);
            gameSettingText.fontSizeMin = 12;
            gameSettingText.fontSizeMax = 180;
            gameSettingText.enableAutoSizing = true;
            
            var roleSetting = UI.Panel(horizontalGroup,new Vector2(0,0),ButtonColor);
            roleSetting.rectTransform.anchorMin = new Vector2(0.0f,0.0f);
            roleSetting.rectTransform.anchorMax = new Vector2(1.0f,1.0f);
            roleSetting.rectTransform.offsetMin = new Vector2(2,2);
            roleSetting.rectTransform.offsetMax = new Vector2(-2,-2);
            
            var roleSettingText = UI.Text(roleSetting.rectTransform,Translation.Get("option.type.role"),color:CharacterColor);
            roleSettingText.rectTransform.anchorMin = new Vector2(0.0f,0.0f);
            roleSettingText.rectTransform.anchorMax = new Vector2(1.0f,1.0f);
            roleSettingText.rectTransform.offsetMin = new Vector2(2,2);
            roleSettingText.rectTransform.offsetMax = new Vector2(-2,-2);
            roleSettingText.fontSizeMin = 12;
            roleSettingText.fontSizeMax = 180;
            roleSettingText.enableAutoSizing = true;
        }
        private static RectTransform GameSubTab;
        private static RectTransform RoleSubTab; 
        private static void CreateSubTabs(RectTransform SubTab)
        {
            GameSubTab = UI.ContentArea(SubTab);
            CreateGameSubTab(GameSubTab);
            RoleSubTab = UI.ContentArea(SubTab);
        }

        private static void CreateGameSubTab(RectTransform GameSubTab)
        {
            (RectTransform viewport, RectTransform content, UnityEngine.UI.Scrollbar? scrollbar) = UI.ScrollView(GameSubTab, new Vector2(0, 0));
            //scrollbar.handleRect.anchorMin = new Vector2(0.9f, 0);
            //scrollbar.handleRect.anchorMax = new Vector2(1, 1);
            //scrollbar.handleRect.offsetMin = new Vector2(5, 5);
            //scrollbar.handleRect.offsetMax = new Vector2(5, 5);
            for (int i = 0; i < 15; i++)
            {
                var cont = UI.Panel(content, new Vector2(150, 150),Color.black);
                cont.rectTransform.anchorMin = new Vector2(0.0f,0.0f);
                cont.rectTransform.anchorMax = new Vector2(1.0f,1.0f);
            }
        }
    }
}