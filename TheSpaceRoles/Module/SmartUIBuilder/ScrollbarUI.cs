using System;
using UnityEngine;
using UnityEngine.UI;

namespace TSR.Module.SmartUIBuilder
{
    public class ScrollbarUI
    {
        //Scroller + img
        //sliding area(rect) stretch +10
        //handle img
        public UnityEngine.UI.Scrollbar Scrollbar;
        public Image HandleImage;

        public static ScrollbarUI Create(UIBuilderScrollbar scrollbar)
        {
            ScrollbarUI scr = new ScrollbarUI()
            {
                Scrollbar = scrollbar.ScrollbarImage.Image.gameObject
                    .AddComponent<UnityEngine.UI.Scrollbar>(),
                HandleImage = scrollbar.HandleImage.Image
                
            };
            scr.Scrollbar.gameObject.name = "Scrollbar";
            scr.Scrollbar.transition = Selectable.Transition.ColorTint;
            scr.Scrollbar.interactable = true;
            scr.Scrollbar.colors = scrollbar.Colors.Get();
            scr.Scrollbar.image = scrollbar.ScrollbarImage.Image;
            scr.Scrollbar.image.raycastTarget = true;
            scr.Scrollbar.handleRect = scrollbar.HandleImage.Image.rectTransform;
            scr.Scrollbar.targetGraphic = scrollbar.HandleImage.Image;
            scr.Scrollbar.direction = UnityEngine.UI.Scrollbar.Direction.BottomToTop;
            scr.Scrollbar.navigation.mode = scrollbar.Mode;// Navigation.Mode.Explicit | Navigation.Mode.Horizontal;
            scr.Scrollbar.Set(1);
            
            var slidingArea = new GameObject("SlidingArea").AddComponent<RectTransform>();
            slidingArea.transform.SetParent(scr.Scrollbar.transform);
            slidingArea.anchorMin = Vector2.zero;
            slidingArea.anchorMax = Vector2.one;
            slidingArea.pivot = new Vector2(0.5f, 0.5f);
            slidingArea.offsetMax = new Vector2(10,10);
            slidingArea.offsetMin = new Vector2(10, 10);

            scrollbar.HandleImage.Image.transform.SetParent(slidingArea.transform);
            scrollbar.HandleImage.Image.raycastTarget = true;
            scrollbar.HandleImage.Image.rectTransform.offsetMin = new  Vector2(-10, -10);
            scrollbar.HandleImage.Image.rectTransform.offsetMax = new  Vector2(-10, -10);
            scrollbar.HandleImage.Image.name = "Handle";
            return scr;
        }
        public struct UIBuilderScrollbar
        {
            //public Transform Parent;
            public Navigation.Mode Mode = Navigation.Mode.Explicit | Navigation.Mode.Horizontal;
            public ImageUI ScrollbarImage;
            public ImageUI HandleImage;
            public UIColorBlocks Colors;

            public UIBuilderScrollbar(Navigation.Mode Mode, ImageUI ScrollbarImage,ImageUI HandleImage, UIColorBlocks Colors)
            {
                this.Mode = Mode;
                this.ScrollbarImage = ScrollbarImage;
                this.HandleImage = HandleImage;
                this.Colors = Colors;
            }
        }
    }
    public class ScrollPanelUI
    {
        public ScrollRect ScrollRect;

        public static ScrollPanelUI Create(UIBuilderScrollRect scrollRect)
        {
            var t = new ScrollPanelUI { ScrollRect = scrollRect.PanelUI.Image.gameObject.AddComponent<ScrollRect>() };
            t.ScrollRect.gameObject.name = "Panel";
            t.ScrollRect.content = scrollRect.ContentUI.Image.rectTransform;
            scrollRect.ContentUI.Image.transform.SetParent(scrollRect.PanelUI.Image.transform);
            t.ScrollRect.horizontal = (scrollRect.Direction & ScrollDirection.Horizontal) == ScrollDirection.Horizontal;
            t.ScrollRect.vertical = (scrollRect.Direction & ScrollDirection.Vertical) == ScrollDirection.Vertical;
            t.ScrollRect.viewport = scrollRect.PanelUI.Image.rectTransform;
            t.ScrollRect.horizontalScrollbar = scrollRect.HorizontalScrollbarUI?.Scrollbar;
            t.ScrollRect.verticalScrollbar = scrollRect.VerticalScrollbarUI?.Scrollbar;
            t.ScrollRect.scrollSensitivity = 50;
            return t;
        }

        public struct UIBuilderScrollRect(
            ImageUI panelUI,
            ImageUI contentUI,
            ScrollDirection direction,
            ScrollbarUI? horizontalScrollbarUI,
            ScrollbarUI? verticalScrollbarUI)
        {
            public readonly ImageUI PanelUI = panelUI;
            public readonly ScrollbarUI? HorizontalScrollbarUI = horizontalScrollbarUI;
            public readonly ScrollbarUI? VerticalScrollbarUI = verticalScrollbarUI;
            public readonly ImageUI ContentUI = contentUI;
            public readonly ScrollDirection Direction = direction;
        }

        [Flags]
        public enum ScrollDirection
        {
            None = 0,
            Vertical = 1 << 0,
            Horizontal = 1 << 1
        }
    }
}