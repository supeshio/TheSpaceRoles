using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TSR.Module.SmartUIBuilder;

[SmartPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
public static class SmartCanvas
{
    public static GameObject? TSRUI;
    private static Canvas? _uiCanvas;
    private static EventSystem? _eventSystem;

    public static void Postfix()
    {
        if (TSRUI != null) return;

        TSRUI = new GameObject("TSRUI");
        Object.DontDestroyOnLoad(TSRUI);

        _eventSystem = new GameObject("EventSystem").AddComponent<EventSystem>();
        _eventSystem.transform.SetParent(TSRUI.transform);
        _eventSystem.OnEnable();

        _uiCanvas = new GameObject("UICanvas").AddComponent<Canvas>();
        _uiCanvas.transform.SetParent(TSRUI.transform);
        _uiCanvas.worldCamera = Camera.main;
        _uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = _uiCanvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        _uiCanvas.gameObject.AddComponent<GraphicRaycaster>();
        Logger.Info("TSRUI created");
    }
}