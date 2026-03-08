using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TSR.Module.SmartUIBuilder
{
    /// <summary>
    /// ボタンを作成する｡
    /// 必ずcanvas以下に作成すること｡
    /// OnClickは自分で設定すること｡
    /// </summary>
    public class ButtonUI
    {
        public Button Button;

        public const float PlayerNameOutlineThickness = 0.1745f;

        public static ButtonUI CreateButtonUI(UIBuilderButton uiBuilderButton)
        {
            ButtonUI panel = new ButtonUI();
            panel.Button = uiBuilderButton.ImageUI.Image.gameObject.AddComponent<UnityEngine.UI.Button>();
            panel.Button.gameObject.name = "Button";
            var colors = uiBuilderButton.UIColorBlockBlock.Get();
            panel.Button.transition = Selectable.Transition.ColorTint;
            panel.Button.colors = colors;
            panel.Button.image = uiBuilderButton.ImageUI.Image;
            panel.Button.interactable = true;
            panel.Button.image.raycastTarget = true;
            return panel;
        }
        /// <summary>
        /// 毎回リセットするので一度にすべてのactionを送るべし
        /// </summary>
        /// <param name="action"></param>
        public void SetOnClick(UnityAction action)
        {
            Button.onClick = new Button.ButtonClickedEvent();
            Button.onClick.AddListener((UnityAction)(() => { }));
            Button.colors = Button.colors;
        }


        public struct UIBuilderButton(
            ImageUI imageUI,
            TextAlignmentOptions alignment = TextAlignmentOptions.Center,
            Vector3? anchoredPosition3D = null,
            bool autoSizing = true,
            Vector2? size = null,UIColorBlocks? colors = null)
        {
            public UIColorBlocks UIColorBlockBlock = colors ??
                                       new UIColorBlocks(
                                           Palette.White,
                                           new Color(0.6f,0.9f,0.7f),
                                           new Color(0.3f,0.7f,0.8f),
                                           new Color(0.2f,0.8f,0.6f),
                                           Palette.DisabledClear
                                           );
            public ImageUI ImageUI= imageUI;
        }

        //public const DefaultColors = new 
    }
}