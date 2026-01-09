using TMPro;
using TSR.Patch;
using UnityEngine;

namespace TSR.Game.Options.OptionControlUI
{
    public class OptionAsset
    {
        public class CustomOptionBG
        {
            public SpriteRenderer SpRenderer;

            public CustomOptionBG(string Name, Transform Parent, Vector3 LocalPosition, Vector2 Size, Color32? color = null)
            {
                SpRenderer = new GameObject(Name).AddComponent<SpriteRenderer>();
                SpRenderer.transform.SetParent(Parent);
                SpRenderer.sprite = Assets.AssetLoader.Sprites["option_bg"];
                SpRenderer.drawMode = SpriteDrawMode.Sliced;
                SpRenderer.gameObject.layer = 5;
                SpRenderer.transform.localPosition = LocalPosition;
                SpRenderer.color = color ?? Color.white;
                SpRenderer.size = Size * 2f;
                SpRenderer.transform.localScale = Vector3.one * 0.5f;
            }
        }

        public class ScrollBG
        {
            public SpriteRenderer SpRenderer;

            public ScrollBG(string Name, Transform Parent, Vector3 LocalPosition, Vector2 Size, Color32? color = null)
            {
                SpRenderer = new GameObject(Name).AddComponent<SpriteRenderer>();
                SpRenderer.transform.SetParent(Parent);
                SpRenderer.sprite = Assets.AssetLoader.Sprites["square"];
                SpRenderer.drawMode = SpriteDrawMode.Sliced;
                SpRenderer.gameObject.layer = 5;
                SpRenderer.transform.localPosition = LocalPosition;
                SpRenderer.color = color ?? Color.white;
                SpRenderer.size = Size * 2f;
                SpRenderer.transform.localScale = Vector3.one * 0.5f;
            }
        }

        public class CustomOptionText
        {
            public TextMeshPro Text;

            public CustomOptionText(string Name, Transform Parent, Color32 TextColor, Color32 OutlineColor, float FontSizeMin, float FontSizeMax, Vector3 LocalPosition, Vector2 Size, float Scale, TextAlignmentOptions Align = TextAlignmentOptions.Left)
            {
                Text = new GameObject("Text").AddComponent<TextMeshPro>();
                Text.transform.SetParent(Parent);
                Text.gameObject.layer = 5;
                Text.text = Name;
                Text.color = TextColor;
                Text.fontSizeMin = FontSizeMin;
                Text.fontSize = Text.fontSizeMax = FontSizeMax;
                Text.font = Helper.TextFont_Ping();
                Text.material = Helper.TextMaterial_Ping();
                Text.outlineColor = OutlineColor;
                Text.outlineWidth = 0.2f;
                Text.alignment = Align;
                Text.enableWordWrapping = false;
                Text.enableAutoSizing = true;
                Text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                Text.rectTransform.sizeDelta = Size;
                Text.transform.localPosition = LocalPosition;
            }
        }
    }
}