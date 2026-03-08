using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace TSR.Module.SmartUIBuilder
{
    [SmartPatch(typeof(MainMenuManager),nameof(MainMenuManager.Start))]
    public static class SmartCanvas
    {
        public static GameObject? TSRUI = null;
        private static UnityEngine.Canvas? _uiCanvas = null;
        private static EventSystem? _eventSystem = null;
        
        public static void Postfix()
        {
            if (TSRUI != null)
            {
                return;
            }

            TSRUI = new GameObject("TSRUI");
            
            Object.DontDestroyOnLoad(TSRUI);
            
            _eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            _eventSystem.transform.SetParent(TSRUI.transform);
            _eventSystem.OnEnable();
            _uiCanvas = new GameObject("UICanvas").AddComponent<Canvas>();
            _uiCanvas.transform.SetParent(TSRUI.transform);
            
            var canvasScaler = _uiCanvas.gameObject.AddComponent<CanvasScaler>();
            
            //var canvasRect =UICanvas.GetComponent<RectTransform>();
            //canvasRect.position = new Vector3(0f, 0f, 0f);
            
            _uiCanvas.worldCamera = Camera.main;
            _uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            var raycaster = _uiCanvas.gameObject.AddComponent<GraphicRaycaster>();
            //EventSystem eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
            //eventSystem.transform.SetParent(TSRUI.transform);
            
            
            
            /*
            var f = UsefulUI.UsefulUI.DefaultScrollbar(_uiCanvas.transform,new Color32(0xe9,0xa6,0xc2,0xff),new Vector3(0,0,0),new Vector3(600,0),new Vector2(1080,960),new Vector2(30,960));
            var k = f.Item2.ScrollRect.content.transform;
            ImageUI Panel = ImageUI.CreateSlicedImageUI(k,
                new ImageUI.UIBuilderImage(Assets.AssetLoader.Sprites["option_bg"], new Color32(0x85, 0xdf, 0x73, 0xff),
                    new Vector2(1000, 1000)));
            TextUI textUI = TextUI.CreateAutoSizeTextContainer(
                _uiCanvas.transform,
                new TextUI.UIBuilderText("helper",Color.white,new TextUI.FontSize(20f),new TextUI.Outline(0.15f),anchoredPosition3D: new Vector3(0,0,-3f)));
            ImageUI imageUI = ImageUI.CreateSlicedImageUI(k,
                new ImageUI.UIBuilderImage(Assets.AssetLoader.Sprites["option_bg"],
                    Helper.ColorFromColorcode("#08085f"),new Vector2(150f,20f),anchoredPosition3D: new Vector3(0,0,-2f)));
            var image = ImageUI.CreateSlicedImageUI(k,
                new ImageUI.UIBuilderImage(Assets.AssetLoader.Sprites["option_bg"],Helper.ColorPalette.White, new Vector2(300f, 60f),
                    anchoredPosition3D: new Vector3(0, -40, -3f)));
            ButtonUI buttonUI = ButtonUI.CreateButtonUI(new ButtonUI.UIBuilderButton(image));
            buttonUI.SetOnClick((UnityAction)(()=> { Logger.Info("ddddd"); }));
            */
            
            
            
            
            
            
            
            Logger.Info("TSRUI created");
            //親にrectMask2dをつけると子がcropされるぞ

        }
    }
}