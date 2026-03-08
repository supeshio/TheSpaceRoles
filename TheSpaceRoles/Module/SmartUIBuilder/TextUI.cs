using TMPro;
using UnityEngine;

namespace TSR.Module.SmartUIBuilder
{
    public class TextUI
    {
        public TextMeshProUGUI Text;
        
        public const float PlayerNameOutlineThickness = 0.1745f;
        
        public static TextUI CreateAutoSizeTextContainer(Transform parent,UIBuilderText uiBuilderText)
        {
            var textUI = new TextUI();
            textUI.Text = new GameObject("Text").AddComponent<TextMeshProUGUI>();
            textUI.Text.transform.SetParent(parent);
            textUI.Text.text = uiBuilderText.TextValue;
            textUI.Text.color = uiBuilderText.Color;
            textUI.Text.fontSize = uiBuilderText.FontSize.Size;
            textUI.Text.fontSizeMax = uiBuilderText.FontSize.Max;
            textUI.Text.fontSizeMin = uiBuilderText.FontSize.Min;
            textUI.Text.alignment = uiBuilderText.Alignment;
            //textUI.Text.rectTransform.sizeDelta = new Vector2(textUI.Text.rectTransform.sizeDelta.x, fontSize);
            textUI.Text.autoSizeTextContainer = uiBuilderText.AutoSizing;
            textUI.Text.rectTransform.anchorMin = new  Vector2(0.5f, 0.5f);
            textUI.Text.rectTransform.anchorMax = new  Vector2(0.5f, 0.5f);
            textUI.Text.SetOutlineColor(uiBuilderText.Outline.Color ??  Color.black);
            textUI.Text.SetOutlineThickness(uiBuilderText.Outline.Thickness);
            textUI.Text.rectTransform.anchoredPosition3D = uiBuilderText.AnchoredPosition;
            textUI.Text.rectTransform.sizeDelta = uiBuilderText.Size;
            //Text.fontMaterial = 
            
            return textUI;
        }
        
        
        public struct UIBuilderText(
            string textValue,
            Color color,
            FontSize fontSize,
            Outline outline,
            TextAlignmentOptions alignment = TextAlignmentOptions.Center,
            Vector3? anchoredPosition3D = null,
            bool autoSizing = true,
            Vector2? size = null)
        {
            public string TextValue = textValue;
            public Color Color = color;
            public FontSize FontSize = fontSize;
            public Outline Outline = outline;
            public TextAlignmentOptions Alignment = alignment;
            public Vector3 AnchoredPosition = anchoredPosition3D ?? Vector3.zero;
            public bool AutoSizing = autoSizing;
            public Vector2 Size = size ?? Vector2.zero;
        }

        public struct Outline
        {
            public float Thickness;
            public Color? Color;

            public Outline(float ? thickness = null, Color? color = null)
            {
                Thickness = thickness ?? 0;
                Color = color ?? Helper.ColorPalette.White;
            }
        }
        public struct FontSize
        {
            public float Size;
            public float Max;
            public float Min;

            public FontSize(float size, float? max = null, float? min =null)
            {
                this.Size = size;
                this.Max = max ?? size;
                this.Min = min ?? size;
            }
        }
    }
    
}