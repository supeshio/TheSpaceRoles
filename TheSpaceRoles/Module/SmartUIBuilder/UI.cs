using System;
using System.Collections.Generic;
using TSR.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TSR.Module.SmartUIBuilder;

/// <summary>
/// 画像・テキスト・レイアウト・スクロールを統一した API で提供する UI ビルダー。
/// RectMask2D を必要時のみ使用。SpriteMask は使わない。
/// </summary>
public static class UI
{
    private static Sprite? DefaultPanelSprite =>
        AssetLoader.Sprites.GetValueOrDefault("option_bg");

    #region Image

    /// <summary>アイコン用（アスペクト保持）</summary>
    public static Image Icon(Transform parent, Sprite sprite, Vector2 size, Color? color = null)
    {
        var go = new GameObject("Image");
        var img = go.AddComponent<Image>();
        img.rectTransform.SetParent(parent, false);
        img.sprite = sprite;
        img.color = color ?? Color.white;
        img.type = Image.Type.Simple;
        img.preserveAspect = true;
        img.raycastTarget = true;
        img.maskable = true;
        SetRectDefault(img.rectTransform, size);
        return img;
    }

    /// <summary>背景パネル用（スライス可能）</summary>
    public static Image ImageSliced(Transform parent, Sprite sprite, Vector2 size, Color? color = null)
    {
        var go = new GameObject("Image");
        var img = go.AddComponent<Image>();
        img.rectTransform.SetParent(parent, false);
        img.sprite = sprite;
        img.color = color ?? Color.white;
        img.type = Image.Type.Sliced;
        img.fillCenter = true;
        img.raycastTarget = true;
        img.maskable = true;
        SetRectDefault(img.rectTransform, size);
        return img;
    }

    /// <summary>option_bg を使ったパネル（スライス）</summary>
    public static Image Panel(Transform parent, Vector2 size, Color? color = null)
    {
        var sprite = DefaultPanelSprite;
        return ImageSliced(parent, sprite, size, color ?? new Color32(0x20, 0x22, 0x28, 0xff));
    }

    #endregion

    #region Text

    /// <summary>テキスト表示</summary>
    public static TextMeshProUGUI Text(Transform parent, string content,
        float fontSize = 16,
        TextAlignmentOptions alignment = TextAlignmentOptions.Center,
        Color? color = null)
    {
        var go = new GameObject("Text");
        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.transform.SetParent(parent, false);
        tmp.text = content;
        tmp.fontSize = fontSize;
        tmp.alignment = alignment;
        tmp.color = color ?? Color.white;
        tmp.raycastTarget = false;
        SetRectDefault(tmp.rectTransform, Vector2.zero);
        return tmp;
    }

    /// <summary>テキスト（アウトライン付き）</summary>
    public static TextMeshProUGUI TextWithOutline(Transform parent, string content,
        float fontSize = 16,
        TextAlignmentOptions alignment = TextAlignmentOptions.Center,
        Color? color = null,
        float outlineThickness = 0.15f,
        Color? outlineColor = null)
    {
        var tmp = Text(parent, content, fontSize, alignment, color);
        tmp.outlineWidth = outlineThickness;
        tmp.outlineColor = outlineColor ?? Color.black;
        return tmp;
    }

    #endregion

    #region Button

    /// <summary>画像ベースのボタン</summary>
    public static Button Button(Transform parent, Image image, UnityAction onClick = null)
    {
        var btn = image.gameObject.AddComponent<Button>();
        btn.targetGraphic = image;
        btn.transition = Selectable.Transition.ColorTint;
        btn.colors = DefaultButtonColors();
        if (onClick != null)
            btn.onClick.AddListener(onClick);
        return btn;
    }

    /// <summary>タブボタン。背景 Image を out で返す（選択状態の色変更用）</summary>
    public static Button TabButton(Transform parent, Vector2 size, string label,
        out Image bgImage,
        Color? bgColor = null,
        UnityAction onClick = null)
    {
        var panel = Panel(parent, size, bgColor ?? new Color32(0x30, 0x30, 0x40, 0xff));
        bgImage = panel;
        var le = panel.gameObject.AddComponent<LayoutElement>();
        le.preferredWidth = size.x;
        le.preferredHeight = size.y;
        var txt = Text(panel.transform, label, 18, TextAlignmentOptions.Center);
        txt.rectTransform.anchorMin = Vector2.zero;
        txt.rectTransform.anchorMax = Vector2.one;
        txt.rectTransform.offsetMin = Vector2.zero;
        txt.rectTransform.offsetMax = Vector2.zero;
        return Button(parent, panel, onClick);
    }

    /// <summary>パネル＋ラベルでボタン作成</summary>
    public static Button Button(Transform parent, Vector2 size, string label,
        Color? bgColor = null,
        UnityAction onClick = null)
    {
        var panel = Panel(parent, size, bgColor ?? new Color32(0x40, 0x45, 0x55, 0xff));
        var txt = Text(panel.transform, label, 18);
        txt.rectTransform.anchorMin = Vector2.zero;
        txt.rectTransform.anchorMax = Vector2.one;
        txt.rectTransform.offsetMin = Vector2.zero;
        txt.rectTransform.offsetMax = Vector2.zero;
        var btn = Button(parent, panel, onClick);
        return btn;
    }

    #endregion

    #region Layout

    /// <summary>縦並びレイアウト</summary>
    /// <param name="childControlWidth">子要素の横幅をレイアウトで自動調節するか</param>
    public static RectTransform VerticalGroup(Transform parent,
        float spacing = 8,
        RectOffset padding = null,
        TextAnchor alignment = TextAnchor.UpperCenter,
        bool childControlWidth = true)
    {
        var go = new GameObject("VerticalGroup");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        var vlg = go.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = spacing;
        vlg.padding = padding ?? new RectOffset();
        vlg.childAlignment = alignment;
        vlg.childControlWidth = childControlWidth;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = childControlWidth;
        vlg.childForceExpandHeight = false;
        return rt;
    }

    /// <summary>横並びレイアウト</summary>
    /// <param name="childControlWidth">子要素の横幅をレイアウトで自動調節するか</param>
    public static RectTransform HorizontalGroup(Transform parent,
        float spacing = 8,
        RectOffset padding = null,
        TextAnchor alignment = TextAnchor.MiddleCenter,
        bool childControlWidth = true)
    {
        var go = new GameObject("HorizontalGroup");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing = spacing;
        hlg.padding = padding ?? new RectOffset();
        hlg.childAlignment = alignment;
        hlg.childControlWidth = childControlWidth;
        hlg.childControlHeight = true;
        hlg.childForceExpandWidth = childControlWidth;
        hlg.childForceExpandHeight = true;
        return rt;
    }

    /// <summary>ContentSizeFitter（縦方向に自動伸縮）を付与</summary>
    public static void AutoSizeVertical(RectTransform rt)
    {
        var csf = rt.gameObject.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
    }

    #endregion

    #region SliderWithInput

    /// <summary>スライダーとインプットフィールドを連動させた数値入力UI。戻り値: (Root, Slider, InputField)</summary>
    public static (RectTransform Root, Slider Slider, TMP_InputField InputField) SliderWithInput(Transform parent,
        float min, float max, float value,
        bool wholeNumbers = false,
        float inputWidth = 60,
        float height = 28,
        UnityAction<float>? onValueChanged = null)
    {
        value = Mathf.Clamp(value, min, max);
        var row = HorizontalGroup(parent, 8, null, TextAnchor.MiddleCenter, childControlWidth: false);
        row.sizeDelta = new Vector2(0, height);

        // スライダー領域
        var sliderArea = new GameObject("SliderArea").AddComponent<RectTransform>();
        sliderArea.SetParent(row, false);
        var sliderLe = sliderArea.gameObject.AddComponent<LayoutElement>();
        sliderLe.flexibleWidth = 1;
        sliderLe.preferredHeight = height;

        var sliderBg = ImageSliced(sliderArea, DefaultPanelSprite, Vector2.zero, new Color32(0x30, 0x32, 0x3a, 0xff));
        sliderBg.rectTransform.anchorMin = new Vector2(0, 0.25f);
        sliderBg.rectTransform.anchorMax = new Vector2(1, 0.75f);
        sliderBg.rectTransform.offsetMin = Vector2.zero;
        sliderBg.rectTransform.offsetMax = Vector2.zero;

        var fillArea = new GameObject("Fill Area").AddComponent<RectTransform>();
        fillArea.SetParent(sliderArea, false);
        fillArea.anchorMin = Vector2.zero;
        fillArea.anchorMax = Vector2.one;
        fillArea.offsetMin = new Vector2(4, 4);
        fillArea.offsetMax = new Vector2(-4, -4);

        var fill = ImageSliced(fillArea, DefaultPanelSprite, Vector2.zero, new Color32(0x50, 0x70, 0x90, 0xff));
        fill.rectTransform.anchorMin = Vector2.zero;
        fill.rectTransform.anchorMax = Vector2.one;
        fill.rectTransform.offsetMin = Vector2.zero;
        fill.rectTransform.offsetMax = Vector2.zero;

        var handleArea = new GameObject("Handle Slide Area").AddComponent<RectTransform>();
        handleArea.SetParent(sliderArea, false);
        handleArea.anchorMin = Vector2.zero;
        handleArea.anchorMax = Vector2.one;
        handleArea.offsetMin = Vector2.zero;
        handleArea.offsetMax = Vector2.zero;

        var handle = new GameObject("Handle").AddComponent<RectTransform>();
        handle.SetParent(handleArea, false);
        handle.sizeDelta = new Vector2(16, 0);
        var handleImg = handle.gameObject.AddComponent<Image>();
        handleImg.color = new Color32(0x80, 0x90, 0xb0, 0xff);

        var slider = sliderArea.gameObject.AddComponent<Slider>();
        slider.fillRect = fill.rectTransform;
        slider.handleRect = handle;
        slider.minValue = min;
        slider.maxValue = max;
        slider.wholeNumbers = wholeNumbers;
        slider.value = value;
        slider.targetGraphic = handleImg;
        slider.colors = DefaultButtonColors();

        // インプットフィールド
        var inputGo = new GameObject("InputField");
        var inputRt = inputGo.AddComponent<RectTransform>();
        inputRt.SetParent(row, false);
        var inputLe = inputGo.AddComponent<LayoutElement>();
        inputLe.preferredWidth = inputWidth;
        inputLe.preferredHeight = height;

        var inputBg = ImageSliced(inputGo.transform, DefaultPanelSprite, Vector2.zero, new Color32(0x28, 0x2a, 0x32, 0xff));
        inputBg.rectTransform.anchorMin = Vector2.zero;
        inputBg.rectTransform.anchorMax = Vector2.one;
        inputBg.rectTransform.offsetMin = Vector2.zero;
        inputBg.rectTransform.offsetMax = Vector2.zero;

        var textArea = new GameObject("Text Area");
        var textAreaRt = textArea.AddComponent<RectTransform>();
        textAreaRt.SetParent(inputRt, false);
        textAreaRt.anchorMin = Vector2.zero;
        textAreaRt.anchorMax = Vector2.one;
        textAreaRt.offsetMin = new Vector2(4, 2);
        textAreaRt.offsetMax = new Vector2(-4, -2);
        textArea.AddComponent<RectMask2D>();

        var textGo = new GameObject("Text");
        var textRt = textGo.AddComponent<RectTransform>();
        textRt.SetParent(textAreaRt, false);
        textRt.anchorMin = Vector2.zero;
        textRt.anchorMax = Vector2.one;
        textRt.offsetMin = Vector2.zero;
        textRt.offsetMax = Vector2.zero;
        var textTmp = textGo.AddComponent<TextMeshProUGUI>();
        textTmp.text = FormatValue(value, wholeNumbers);
        textTmp.fontSize = 14;
        textTmp.alignment = TextAlignmentOptions.Center;
        textTmp.color = Color.white;
        textTmp.raycastTarget = false;

        var placeholderGo = new GameObject("Placeholder");
        var placeholderRt = placeholderGo.AddComponent<RectTransform>();
        placeholderRt.SetParent(textAreaRt, false);
        placeholderRt.anchorMin = Vector2.zero;
        placeholderRt.anchorMax = Vector2.one;
        placeholderRt.offsetMin = Vector2.zero;
        placeholderRt.offsetMax = Vector2.zero;
        var placeholderTmp = placeholderGo.AddComponent<TextMeshProUGUI>();
        placeholderTmp.text = FormatValue(value, wholeNumbers);
        placeholderTmp.fontSize = 14;
        placeholderTmp.alignment = TextAlignmentOptions.Center;
        placeholderTmp.color = new Color(1, 1, 1, 0.5f);
        placeholderTmp.raycastTarget = false;

        var inputField = inputGo.AddComponent<TMP_InputField>();
        inputField.textViewport = textAreaRt;
        inputField.textComponent = textTmp;
        inputField.placeholder = placeholderTmp;
        inputField.contentType = wholeNumbers ? TMP_InputField.ContentType.IntegerNumber : TMP_InputField.ContentType.DecimalNumber;
        inputField.text = FormatValue(value, wholeNumbers);

        void SyncSliderToInput(float val)
        {
            inputField.SetTextWithoutNotify(FormatValue(val, wholeNumbers));
            placeholderTmp.text = FormatValue(val, wholeNumbers);
            onValueChanged?.Invoke(val);
        }

        void SyncInputToSlider()
        {
            var s = inputField.text;
            if (float.TryParse(s, out var parsed))
            {
                var clamped = Mathf.Clamp(parsed, min, max);
                if (wholeNumbers) clamped = Mathf.Round(clamped);
                slider.SetValueWithoutNotify(clamped);
                inputField.SetTextWithoutNotify(FormatValue(clamped, wholeNumbers));
                placeholderTmp.text = FormatValue(clamped, wholeNumbers);
                onValueChanged?.Invoke(clamped);
            }
            else
            {
                inputField.SetTextWithoutNotify(FormatValue(slider.value, wholeNumbers));
            }
        }

        slider.onValueChanged.AddListener((UnityAction<float>)SyncSliderToInput);
        inputField.onEndEdit.AddListener((UnityAction<string>)(_ => SyncInputToSlider()));

        return (row, slider, inputField);
    }

    private static string FormatValue(float v, bool wholeNumbers) =>
        wholeNumbers ? ((int)Mathf.Round(v)).ToString() : v.ToString("F1");

    #endregion

    #region ScrollView

    public enum ScrollDirection { Vertical, Horizontal }

    /// <summary>スクロールビューを作成。戻り値: (Viewport, Content, Scrollbar)。showScrollbar=false のとき Scrollbar は null</summary>
    public static (RectTransform Viewport, RectTransform Content, UnityEngine.UI.Scrollbar? Scrollbar) ScrollView(Transform parent,
        Vector2 viewportSize,
        ScrollDirection direction = ScrollDirection.Vertical,
        bool showScrollbar = true,
        Color? contentBgColor = null)
    {
        var scrollGo = new GameObject("ScrollView");
        var scrollRt = scrollGo.AddComponent<RectTransform>();
        scrollRt.SetParent(parent, false);
        if (viewportSize.x > 0 && viewportSize.y > 0)
        {
            scrollRt.anchorMin = new Vector2(0.5f, 0.5f);
            scrollRt.anchorMax = new Vector2(0.5f, 0.5f);
            scrollRt.sizeDelta = viewportSize;
        }
        else
        {
            scrollRt.anchorMin = Vector2.zero;
            scrollRt.anchorMax = Vector2.one;
            scrollRt.offsetMin = Vector2.zero;
            scrollRt.offsetMax = Vector2.zero;
        }

        scrollGo.AddComponent<RectMask2D>();

        var viewportGo = new GameObject("Viewport");
        var viewport = viewportGo.AddComponent<RectTransform>();
        viewport.SetParent(scrollRt, false);
        viewport.anchorMin = Vector2.zero;
        viewport.anchorMax = Vector2.one;
        viewport.offsetMin = showScrollbar ? new Vector2(0, 0) : Vector2.zero;
        viewport.offsetMax = showScrollbar ? new Vector2(-28, 0) : Vector2.zero;
        viewportGo.AddComponent<RectMask2D>();

        var contentGo = new GameObject("Content");
        var content = contentGo.AddComponent<RectTransform>();
        content.SetParent(viewport, false);
        content.pivot = new Vector2(0.5f, 1);
        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(1, 1);
        content.offsetMin = new Vector2(0, 0);
        content.offsetMax = new Vector2(0, 0);
        content.sizeDelta = new Vector2(0, viewportSize.y > 0 ? viewportSize.y : 400);

        var vlg = contentGo.AddComponent<VerticalLayoutGroup>();
        vlg.spacing = 8;
        vlg.childAlignment = TextAnchor.UpperCenter;
        vlg.childControlWidth = true;
        vlg.childControlHeight = false;
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.padding = new RectOffset { left = 10, right = 10, top = 10, bottom = 10 };

        var csf = contentGo.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;

        if (contentBgColor.HasValue)
        {
            var bg = contentGo.AddComponent<Image>();
            bg.color = contentBgColor.Value;
            bg.raycastTarget = false;
        }

        var scrollRect = scrollGo.AddComponent<ScrollRect>();
        scrollRect.content = content;
        scrollRect.viewport = viewport;
        scrollRect.vertical = direction == ScrollDirection.Vertical;
        scrollRect.horizontal = direction == ScrollDirection.Horizontal;
        scrollRect.scrollSensitivity = 30;
        scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;

        UnityEngine.UI.Scrollbar? scrollbar = null;
        if (showScrollbar)
        {
            var (sb, _) = CreateScrollbar(scrollRt, viewportSize.y);
            scrollbar = sb;
            scrollRect.verticalScrollbar = sb;
        }

        return (viewport, content, scrollbar);
    }

    private static (UnityEngine.UI.Scrollbar scrollbar, RectTransform handle) CreateScrollbar(RectTransform parent, float height)
    {
        var sprite = DefaultPanelSprite;
        var trackGo = new GameObject("Scrollbar");
        var trackRt = trackGo.AddComponent<RectTransform>();
        trackRt.SetParent(parent, false);
        trackRt.anchorMin = new Vector2(1, 0);
        trackRt.anchorMax = new Vector2(1, 1);
        trackRt.pivot = new Vector2(1, 0.5f);
        trackRt.anchoredPosition = Vector2.zero;
        trackRt.sizeDelta = new Vector2(20, 0);
        var trackImg = trackGo.AddComponent<Image>();
        trackImg.sprite = sprite;
        trackImg.type = Image.Type.Simple;
        trackImg.color = new Color32(0x40, 0x40, 0x50, 0xff);
        trackImg.raycastTarget = true;

        var slidingArea = new GameObject("SlidingArea").AddComponent<RectTransform>();
        slidingArea.SetParent(trackRt, false);
        slidingArea.anchorMin = Vector2.zero;
        slidingArea.anchorMax = Vector2.one;
        slidingArea.offsetMin = new Vector2(4, 4);
        slidingArea.offsetMax = new Vector2(-4, -4);

        var handleGo = new GameObject("Handle");
        var handleRt = handleGo.AddComponent<RectTransform>();
        handleRt.SetParent(slidingArea, false);
        handleRt.anchorMin = new Vector2(0, 0);
        handleRt.anchorMax = new Vector2(1, 0);
        handleRt.pivot = new Vector2(0.5f, 0);
        handleRt.anchoredPosition = Vector2.zero;
        handleRt.sizeDelta = new Vector2(0, 40);
        var handleImg = handleGo.AddComponent<Image>();
        handleImg.sprite = sprite;
        handleImg.type = Image.Type.Simple;
        handleImg.color = new Color32(0x80, 0x85, 0xa0, 0xff);
        handleImg.raycastTarget = true;

        var scrollbar = trackGo.AddComponent<UnityEngine.UI.Scrollbar>();
        scrollbar.handleRect = handleRt;
        scrollbar.targetGraphic = handleImg;
        scrollbar.direction = UnityEngine.UI.Scrollbar.Direction.BottomToTop;
        scrollbar.numberOfSteps = 0;
        scrollbar.Set(1f);
        return (scrollbar, handleRt);
    }

    #endregion

    #region Root

    /// <summary>Canvas 付き UI ルートを作成。戻り値: (root, canvasRectTransform)</summary>
    public static (GameObject Root, RectTransform CanvasRect) CreateRoot(string name,
        Vector2 referenceResolution,
        int layer = 5)
    {
        var root = new GameObject(name);
        root.layer = layer;
        //UnityEngine.Object.DontDestroyOnLoad(root);

        var es = new GameObject("EventSystem").AddComponent<UnityEngine.EventSystems.EventSystem>();
        es.transform.SetParent(root.transform);
        es.OnEnable();

        var canvasGo = new GameObject("UICanvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.transform.SetParent(root.transform);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = Camera.main;

        var scaler = canvasGo.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = referenceResolution;
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;

        canvasGo.AddComponent<GraphicRaycaster>();
        var canvasRt = canvas.GetComponent<RectTransform>();
        return (root, canvasRt!);
    }

    /// <summary>RectMask2D 付きコンテナ領域</summary>
    public static RectTransform ContentArea(RectTransform parent,
        float marginLeft = 0, float marginRight = 0, float marginTop = 0, float marginBottom = 0)
    {
        var go = new GameObject("ContentArea");
        var rt = go.AddComponent<RectTransform>();
        rt.SetParent(parent, false);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(marginLeft, marginBottom);
        rt.offsetMax = new Vector2(-marginRight, -marginTop);
        go.AddComponent<RectMask2D>();
        return rt;
    }

    #endregion

    #region Helpers

    private static void SetRectDefault(RectTransform rt, Vector2 size)
    {
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        if (size != Vector2.zero)
            rt.sizeDelta = size;
    }

    private static ColorBlock DefaultButtonColors()
    {
        return new ColorBlock
        {
            normalColor = Color.white,
            highlightedColor = new Color(0.9f, 0.95f, 1f),
            pressedColor = new Color(0.75f, 0.8f, 0.9f),
            disabledColor = new Color(0.5f, 0.5f, 0.5f),
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };
    }

    #endregion
}
