using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TheSpaceRoles.CustomOption;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class CustomOptions
    {
        public static GameOptionsMenu TSRTab;
        public static PassiveButton TSRButton;
        [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start)), HarmonyPostfix]
        public static void ContinueCoStart(GameSettingMenu __instance)
        {
            Logger.Info("Start");
            __instance.PresetsTab.gameObject.SetActive(false);
            __instance.GamePresetsButton.gameObject.SetActive(false);
            TSRButton = UnityEngine.Object.Instantiate(__instance.GameSettingsButton.gameObject).GetComponent<PassiveButton>();
            TSRTab = UnityEngine.Object.Instantiate(__instance.GameSettingsTab.gameObject).GetComponent<GameOptionsMenu>();

            TSRButton.gameObject.SetActive(true);
            TSRButton.transform.SetParent(__instance.GamePresetsButton.transform.parent);
            TSRTab.transform.SetParent(__instance.GameSettingsTab.transform.parent);
            TSRTab.transform.localPosition = __instance.GameSettingsTab.transform.localPosition;
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {

                TSRButton.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>().text = $"{TSR.c_name}</color> 設定";

            })));

            TSRTab.scrollBar.Inner.DestroyChildren();
            TSRTab.RefreshChildren();

            Logger.Info(TSRButton.transform.parent.name);
            //-2.96 -0.42 -2

            GameObject[] buttons = [__instance.GameSettingsButton.gameObject, TSRButton.gameObject, __instance.RoleSettingsButton.gameObject];
            int i = 0;
            foreach (var item in buttons)
            {
                item.transform.localPosition = new Vector3(-2.96f, -0.42f - i * 0.6f, -2f);
                i++;
            }

            List<PassiveButton> passiveButtons = [__instance.GameSettingsButton, TSRButton, __instance.RoleSettingsButton];
            //[__instance.GameSettingsTab,TSRTab,__instance.RoleSettingsTab];
            for (int j = 0; j < passiveButtons.Count; j++)
            {
                int copyindex = j;
                var button = passiveButtons[j];
                button.OnClick = new(); 
                button.OnClick.AddListener((UnityAction)(() =>
                {
                    __instance.GameSettingsTab.gameObject.SetActive(false);
                    __instance.RoleSettingsTab.gameObject.SetActive(false);
                    __instance.PresetsTab.gameObject.SetActive(false);
                    passiveButtons.Do(x => { x.transform.FindChild("Selected").gameObject.SetActive(false); });
                    TSRTab.gameObject.SetActive(false);
                    if (copyindex == 0)
                    {
                        __instance.GameSettingsTab.gameObject.SetActive(true);

                    }
                    else if (copyindex == 1)
                    {
                        TSRTab.gameObject.SetActive(true);
                    }
                    else if (copyindex == 2)
                    {

                        __instance.RoleSettingsTab.gameObject.SetActive(true);
                    }
                }));
            }
            optionTypeCounter.Clear();
            foreach (OptionType item in Enum.GetValues(typeof(OptionType)))
            {

                CustomOption.optionTypeCounter.Add(item, 0);
            }
            CustomOptionsHolder.CreateCustomOptions();
        }

    }
    public class CustomOption
    {


        public static Dictionary<OptionType, float> optionTypeCounter = new();

        public static void OptionTypeCounterCountup(OptionType optionType, float i = 1)
        {
            optionTypeCounter[optionType] += i;
        }

        public enum OptionType
        {
            Default,
            General,
            Roles,
        }
        public string nameId;
        public Func<string>[] selections;
        public ModOption ModOption;
        public int selection;
        public int defaultSelection;
        public ConfigEntry<int> entry;
        public Action onChange;
        public OptionType optionType;
        public static List<CustomOption> options = [];
        Func<bool> Show;
        public StringNames[] GetStringNames(string[] str){
            List<StringNames> k = [];
            foreach (var item in str)
            {
                k.AddItem(StringNames.None);
            }
            return k.ToArray();
        }
        public string Value() => Translation.GetString(selections[selection]());
        public string Title() => Translation.GetString($"option.{nameId}");
        public CustomOption(OptionType optionType, string nameId, Func<string>[] selections, int defaultSelection, Func<bool> Show = null, Action onChange = null)
        {
            this.nameId = nameId;
            this.defaultSelection = defaultSelection;
            this.onChange = onChange;
            this.selections = selections;
            this.optionType = optionType;
            this.Show = Show ?? (() => true);
            this.entry = TSR.Instance.Config.Bind($"CustomOption", nameId, defaultSelection);
            StringOption stringOption = null;
            switch (optionType)
            {
                case OptionType.Default:
                    stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, GameSettingMenu.Instance.GameSettingsTab.scrollBar.Inner);
                    break;
                case OptionType.General:
                    //Scroller/SliderInner
                    stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, CustomOptions.TSRTab.scrollBar.Inner);
                    break;
                case OptionType.Roles:
                    stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, GameSettingMenu.Instance.RoleSettingsTab.scrollBar.Inner);
                    break;
            }
            Logger.Info(stringOption.name, nameId);
                ModOption = new ModOption
                {
                    StringOption = stringOption
                };
            ModOption.StringOption.Values = GetStringNames(selections.Select(x=>x()).ToArray()) ;
            ModOption.StringOption.Value = 1;
            ModOption.TitleText = ModOption.StringOption.TitleText;
            ModOption.ValueText = ModOption.StringOption.ValueText;
            ModOption.CustomOption = this;
            Logger.Info(nameId);
            stringOption.transform.localPosition = new Vector3(0.952f, optionTypeCounter[optionType] * -0.4f + 1.8f, -2);
            OptionTypeCounterCountup(optionType);
            options.Add(this);
            GameSettingMenu.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {
                ModOption.StringOption.TitleText.text = Title();
                ModOption.StringOption.ValueText.text = Value();
            })));
        }
        public static Func<string> GetOptionSelection(string str, string[] strs = null)
        {
            return () => Translation.GetString("option.selection." + str, strs);
        }
        public bool GetBool() => selection == 0;
        public static Func<string> On() => GetOptionSelection("on");
        public static Func<string> Off() => GetOptionSelection("off");
        public static Func<string> Unlimited() => GetOptionSelection("unlimited");
        public static Func<string> Right() => GetOptionSelection("right");
        public static Func<string> Left() => GetOptionSelection("left");
        public static Func<string> Sec(float x) => GetOptionSelection("second", [x.ToString()]);
        /// <summary>
        /// 1 true ,0 false
        /// </summary>
        public static Func<int, bool> funcOn = x => x != 0;
        public static Func<int, bool> funcOff = x => x == 0;


        public static CustomOption Create(OptionType optionType, string name, bool DefaultValue = false, Func<bool> Show = null, Action onChange = null)
        {
            return new CustomOption(optionType, name, [Off(), On()], DefaultValue ? 1 : 0, Show, onChange);
        }
        public static CustomOption Create(OptionType optionType, string name, Func<string>[] selections, int selection, Func<bool> Show = null, Action onChange = null)
        {
            return new CustomOption(optionType, name, selections, selection, Show, onChange);
        }


        public void UpdateSelection(int selecting)
        {
            if (selecting != selection)
            {
                onChange.Invoke();
                ModOption.StringOption.TitleText.text = Title();
                ModOption.StringOption.ValueText.text = Value();
            }
            selection = selecting;
        }

    }
    public class HeaderMasked
    {
        public string Title() => Translation.GetString($"option.header.{nameId}");
        OptionType OptionType;
        string nameId;
        CategoryHeaderMasked categoryHeaderMasked;
        public HeaderMasked(OptionType optionType, string nameId)
        {
            this.OptionType = optionType;
            this.nameId = nameId;
            categoryHeaderMasked = null;

            switch (optionType)
            {
                case OptionType.Default:
                    categoryHeaderMasked = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, GameSettingMenu.Instance.GameSettingsTab.scrollBar.Inner);
                    break;
                case OptionType.General:
                    categoryHeaderMasked = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, CustomOptions.TSRTab.scrollBar.Inner);
                    break;
                case OptionType.Roles:
                    categoryHeaderMasked = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, GameSettingMenu.Instance.RoleSettingsTab.scrollBar.Inner);
                    break;
            }
            Logger.Info((SpriteRenderer.Instantiate(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin).Background == null).ToString());
            categoryHeaderMasked.Background = SpriteRenderer.Instantiate(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin).Background;
            categoryHeaderMasked.Divider = SpriteRenderer.Instantiate(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin).Divider;
            categoryHeaderMasked.transform.localPosition = new Vector3(0.952f, optionTypeCounter[optionType] * -0.4f + 1.8f, -2);
            OptionTypeCounterCountup(optionType,1.5f);
            GameSettingMenu.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {
                categoryHeaderMasked.Title.text = Title();
            })));
        }
        public static HeaderMasked Create(OptionType optionType, string nameId)
        {
            return new HeaderMasked(optionType, nameId);
        }
    }
}
