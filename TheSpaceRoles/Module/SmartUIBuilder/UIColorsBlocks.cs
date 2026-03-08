using UnityEngine;
using UnityEngine.UI;

namespace TSR.Module.SmartUIBuilder
{
    
    public struct UIColorBlocks(
        Color normalColor,
        Color highlightColor,
        Color pressedColor,
        Color selectedColor,
        Color disabledColor)
    {
            
        public Color DisabledColor = disabledColor;
        public Color HighlightColor = highlightColor;
        public Color NormalColor = normalColor;
        public Color PressedColor = pressedColor;
        public Color SelectedColor = selectedColor;
        public const float FadeDuration = 0.1f;
        public const float ColorMultiplier = 1;
        public ColorBlock Get()
        {
            var colors = new ColorBlock();
            colors.normalColor = NormalColor;
            colors.highlightedColor = HighlightColor;
            colors.disabledColor = DisabledColor;
            colors.pressedColor = PressedColor;
            colors.selectedColor = SelectedColor;
            colors.fadeDuration = FadeDuration;
            colors.colorMultiplier = ColorMultiplier;
            return  colors;
        }
    }
}