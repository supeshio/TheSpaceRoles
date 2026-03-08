using UnityEngine;
using UnityEngine.UI;

namespace TSR.Module.SmartUIBuilder
{
    public class ImageUI
    {
        public Image Image;

        
        public struct UIBuilderImage
        {
            public readonly Sprite Sprite;
            public readonly Color Color;
            public readonly Vector3 AnchoredPosition3D;
            public readonly Vector2 Size;
            public readonly bool SetDefaultSize;
            public readonly bool HasRectMask = true;

            public UIBuilderImage(Sprite sprite,Color color,Vector2 size,bool setDefaultSize =false,Vector3? anchoredPosition3D = null,bool hasRectMask=true)
            {
                this.Sprite = sprite;
                this.Color = color;
                this.Size = size;
                this.SetDefaultSize = setDefaultSize;
                this.AnchoredPosition3D = anchoredPosition3D ?? Vector3.zero;
                this.HasRectMask = hasRectMask;
            }
            public UIBuilderImage(Sprite sprite,Color32 color,Vector2 size,bool setDefaultSize =false,Vector3? anchoredPosition3D = null,bool hasRectMask=true)
            {
                this.Sprite = sprite;
                this.Color = color;
                this.Size = size;
                this.SetDefaultSize = setDefaultSize;
                this.AnchoredPosition3D = anchoredPosition3D ?? Vector3.zero;
                this.HasRectMask = hasRectMask;
            }
        }
        
        public static ImageUI CreateSlicedImageUI(Transform parent,UIBuilderImage uiBuilderImage,float pixelsPerUnitMultiplier = 1)
        {
            ImageUI imageUI = new();
            imageUI.Image= new GameObject("Image").AddComponent<Image>();
            imageUI.Image.rectTransform.SetParent(parent);
            imageUI.Image.rectTransform.anchorMin = new  Vector2(0.5f, 0.5f);
            imageUI.Image.rectTransform.anchorMax = new  Vector2(0.5f, 0.5f);
            imageUI.Image.rectTransform.anchoredPosition3D = uiBuilderImage.AnchoredPosition3D;
            imageUI.Image.sprite = uiBuilderImage.Sprite;
            imageUI.Image.color = uiBuilderImage.Color;
            imageUI.Image.type = Image.Type.Sliced;
            imageUI.Image.maskable = true;
            imageUI.Image.fillCenter = true;
            imageUI.Image.pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;
            imageUI.Image.rectTransform.sizeDelta = uiBuilderImage.Size;
            if(uiBuilderImage.SetDefaultSize)imageUI.Image.SetNativeSize();
            if(uiBuilderImage.HasRectMask)imageUI.Image.gameObject.AddComponent<RectMask2D>();
            return imageUI;
        }
        public static ImageUI CreateSimpleImageUI(Transform parent,UIBuilderImage uiBuilderImage,bool preserveAspect =  true)
        {
            ImageUI imageUI = new();
            imageUI.Image= new GameObject("Image").AddComponent<Image>();
            imageUI.Image.rectTransform.SetParent(parent);
            imageUI.Image.rectTransform.anchorMin = new  Vector2(0.5f, 0.5f);
            imageUI.Image.rectTransform.anchorMax = new  Vector2(0.5f, 0.5f);
            imageUI.Image.rectTransform.anchoredPosition3D = uiBuilderImage.AnchoredPosition3D;
            imageUI.Image.sprite = uiBuilderImage.Sprite;
            imageUI.Image.color = uiBuilderImage.Color;
            imageUI.Image.type = Image.Type.Simple;
            imageUI.Image.maskable = true;
            imageUI.Image.preserveAspect = preserveAspect;
            imageUI.Image.rectTransform.sizeDelta = uiBuilderImage.Size;
            if(uiBuilderImage.SetDefaultSize)imageUI.Image.SetNativeSize();
            if(uiBuilderImage.HasRectMask)imageUI.Image.gameObject.AddComponent<RectMask2D>();
            return imageUI;
        }
    }
}