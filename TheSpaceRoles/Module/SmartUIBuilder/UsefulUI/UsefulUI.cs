using System;
using TSR.Assets;
using UnityEngine;
using UnityEngine.UI;

namespace TSR.Module.SmartUIBuilder.UsefulUI
{
    public static class UsefulUI
    {
        public static ImageUI DefaultRectangleImg(Transform Parent,Color32 color,Vector3 offset,Vector2 size,bool hasRectMask = true)
        {
            return ImageUI.CreateSlicedImageUI(Parent,new ImageUI.UIBuilderImage(AssetLoader.Sprites["option_bg"],color,size,false,offset,hasRectMask));
        }

        public static Tuple<ScrollbarUI,ScrollPanelUI> DefaultScrollbar(Transform Parent, Color32 backGround, Vector3 offset, Vector3 scrollOffset, Vector2 size,Vector2 scrollerSize)
        {
            //handle
            var handle = DefaultRectangleImg(Parent,Helper.ColorFromColorcode("#D3FFFF"),scrollOffset,scrollerSize);
            //scrollbar
            var scrollimg = DefaultRectangleImg(Parent, new Color32(0x99,0x99,0x99,0x99), scrollOffset, scrollerSize);
            var color = new UIColorBlocks(
                Helper.ColorFromColorcode("#FFDF96"),//normal
                Helper.ColorFromColorcode("#F4D690"),//highlight
                Helper.ColorFromColorcode("#C0A871"),//pressed
                Helper.ColorFromColorcode("#F4D690"),//selected
                Helper.ColorFromColorcode("#C8C8C877")//disabled
                );
            var scrollbar = ScrollbarUI.Create(new ScrollbarUI.UIBuilderScrollbar(Navigation.Mode.Vertical | Navigation.Mode.Explicit,scrollimg,handle,color)) ;
            //inner
            var inner = DefaultRectangleImg(Parent,backGround,offset,size + new Vector2(0,400));
            inner.Image.rectTransform.anchorMax = inner.Image.rectTransform.anchorMin = new Vector2(0.5f, 1);
            //panel
            var panel = DefaultRectangleImg(Parent,new Color32(0xff,0xff,0xff,0x99),offset,size);
            var scrollPanelUI = ScrollPanelUI.Create(new ScrollPanelUI.UIBuilderScrollRect(panel,inner,ScrollPanelUI.ScrollDirection.Vertical,null,verticalScrollbarUI:scrollbar));
            return (scrollbar,scrollPanelUI).ToTuple();
        }
    }
}