//using HarmonyLib;
//using System.Collections.Generic;
//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Events;

//namespace TheSpaceRoles
//{


//    public class CustomOptionSelector
//    {
//        public static CustomOptionSelectorSetting Select = CustomOptionSelectorSetting.General;
//        public static List<CustomOptionSelector> selectors = [];
//        public CustomOptionSelectorSetting Setting;
//        public GameObject @object;
//        public PassiveButton Button;
//        public CustomOptionSelector(CustomOptionSelectorSetting setting)
//        {
//            this.Setting = setting;
//            @object = new(setting.ToString());
//            @object.transform.SetParent(HudManager.Instance.transform.FindChild("CustomSettings").FindChild("TSRSettings").FindChild("Selector"));
//            @object.transform.localPosition = new Vector3(-3f, 1.5f - (float)setting * 1f, -1);
//            @object.layer = HudManager.Instance.gameObject.layer;
//            var renderer = @object.AddComponent<SpriteRenderer>();
//            renderer.color = Helper.ColorFromColorcode("#333333");
//            var component = @object.AddComponent<BoxCollider2D>();
//            renderer.sprite = Sprites.GetSpriteFromResources("ui.selector_banner.png", 400);
//            component.size = renderer.bounds.size;
//            Logger.Info(renderer.bounds.size.ToString());
//            Button = @object.AddComponent<PassiveButton>();
//            Button.OnClick = new();
//            Button.OnMouseOut = new UnityEvent();
//            Button.OnMouseOver = new UnityEvent();
//            Button._CachedZ_k__BackingField = 0.1f;
//            Button.CachedZ = 0.1f;
//            Button.Colliders = new[] { @object.GetComponent<BoxCollider2D>() };
//            Button.OnClick.AddListener((System.Action)(() =>
//            {
//                selectors.First(x => x.Setting == Select).Check();
//                CustomOptionsHolder.TSROptions.Do(x => x.@object.gameObject.active = false);
//                CustomOptionsHolder.TSROptions.Where(x => x.obj_parent == setting).Do(x => x.@object.gameObject.active = true);

//                Select = Setting;

//                CustomOptionsHolder.AllCheck();
//            }));


//            TextMeshPro text = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
//            text.text = Translation.GetString("optionselector." + setting.ToString());
//            text.transform.SetParent(@object.transform);
//            text.fontStyle = FontStyles.Bold;
//            text.fontSizeMax = 3f;
//            text.fontSize = text.fontSizeMin = 1f;
//            text.alignment = TextAlignmentOptions.Center;
//            text.enableWordWrapping = true;
//            text.outlineWidth = 0.8f;
//            text.autoSizeTextContainer = false;
//            text.enableAutoSizing = true;
//            text.transform.localPosition = new Vector3(0f, 0, -1);
//            text.transform.localScale = Vector3.one;
//            text.gameObject.layer = HudManager.Instance.gameObject.layer;
//            text.m_sharedMaterial = Data.textMaterial;
//            text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
//            text.rectTransform.sizeDelta = new Vector2(2f, 1.6f);

//            Button.OnMouseOver.AddListener((System.Action)(() =>
//            {
//                renderer.color = Select == Setting ? Helper.ColorFromColorcode("#cccccc") : Helper.ColorFromColorcode("#555555");
//            }));
//            Button.OnMouseOut.AddListener((System.Action)(() =>
//            {
//                renderer.color = Select == Setting ? Helper.ColorFromColorcode("#cccccc") : Helper.ColorFromColorcode("#333333");
//            }));
//            Button.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
//            Button.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;







//            selectors.Add(this);
//        }
//        public void Check()
//        {
//            @object.GetComponent<SpriteRenderer>().color = Helper.ColorFromColorcode("#333333");

//        }

//    }

//    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.enabled))]
//    public static class CustomOptionSelect
//    {

//    }
//}
