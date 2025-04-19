using AmongUs.GameOptions;
using BepInEx.Configuration;
using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class CustomOptions
    {
        public static GameOptionsMenu TSRTab;
        public static PassiveButton TSRButton;
        public static GameOptionsMenu RoleTab;
        public static PassiveButton RoleButton;
        public static GameOptionsMenu CrewmateTab;
        public static PassiveButton CrewmateButton;
        public static GameOptionsMenu NeutralTab;
        public static PassiveButton NeutralButton;
        public static GameOptionsMenu ImpostorTab;
        public static PassiveButton ImpostorButton;

        public static Sprite CrewSprite;
        [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start)), HarmonyPostfix]
        public static void ContinueCoStart(GameSettingMenu __instance)
        {
            var crew = DestroyableSingleton<CrewmateTrackerEntry>.Instance.;
            CrewSprite = crew.sprite;
            LegacyGameOptions.KillDistances = new(new float[] { 0.5f, 1f, 1.8f, 2.5f });
            LegacyGameOptions.KillDistanceStrings = new(new string[] { "Very Short", "Short", "Medium", "Long" });






            Logger.Info("Start");
            __instance.PresetsTab.gameObject.SetActive(false);
            __instance.GamePresetsButton.gameObject.SetActive(false);
            __instance.RoleSettingsButton.gameObject.SetActive(false);
            __instance.transform.FindChild("What Is This?").gameObject.SetActive(false);
            TSRButton = UnityEngine.Object.Instantiate(__instance.GameSettingsButton.gameObject).GetComponent<PassiveButton>();
            TSRTab = UnityEngine.Object.Instantiate(__instance.GameSettingsTab.gameObject).GetComponent<GameOptionsMenu>();

            RoleButton = UnityEngine.Object.Instantiate(__instance.GameSettingsButton.gameObject).GetComponent<PassiveButton>();
            RoleTab = UnityEngine.Object.Instantiate(__instance.GameSettingsTab.gameObject).GetComponent<GameOptionsMenu>();

            ImpostorButton = UnityEngine.Object.Instantiate(__instance.GameSettingsButton.gameObject).GetComponent<PassiveButton>();
            ImpostorTab = UnityEngine.Object.Instantiate(__instance.GameSettingsTab.gameObject).GetComponent<GameOptionsMenu>();
            CrewmateButton = UnityEngine.Object.Instantiate(__instance.GameSettingsButton.gameObject).GetComponent<PassiveButton>();
            CrewmateTab = UnityEngine.Object.Instantiate(__instance.GameSettingsTab.gameObject).GetComponent<GameOptionsMenu>();
            NeutralButton = UnityEngine.Object.Instantiate(__instance.GameSettingsButton.gameObject).GetComponent<PassiveButton>();
            NeutralTab = UnityEngine.Object.Instantiate(__instance.GameSettingsTab.gameObject).GetComponent<GameOptionsMenu>();


            GameOptionsMenu[] tabs = [__instance.GameSettingsTab, TSRTab, RoleTab, CrewmateTab, ImpostorTab, NeutralTab];

            PassiveButton[] buttons = [__instance.GameSettingsButton, TSRButton, RoleButton, CrewmateButton, ImpostorButton, NeutralButton];
            foreach (var tab in tabs)
            {
                if (tab == __instance.GameSettingsTab) continue;
                tab.transform.SetParent(__instance.GameSettingsTab.transform.parent);
                //tab.transform.localPosition = __instance.GameSettingsTab.transform.localPosition;
                tab.scrollBar.allowX = false;

                tab.scrollBar.Inner.DestroyChildren();
                tab.RefreshChildren();
                tab.transform.localPosition = __instance.GameSettingsTab.transform.localPosition;

            }
            foreach (var button in buttons)
            {

                button.gameObject.SetActive(true);
                button.transform.SetParent(__instance.GamePresetsButton.transform.parent);
                var t = button.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>();
            }
            __instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
            {

                TSRButton.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>().text = Translation.GetString("option.setting_mod");
                RoleButton.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>().text = Translation.GetString("option.setting_role");
                CrewmateButton.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>().text = Translation.GetString("option.setting_crewmate");
                ImpostorButton.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>().text = Translation.GetString("option.setting_impostor");
                NeutralButton.transform.FindChild("FontPlacer").FindChild("Text_TMP").GetComponent<TextMeshPro>().text = Translation.GetString("option.setting_neutral");

            })));


            //Logger.Info(TSRButton.transform.parent.name);
            //-2.96 -0.42 -2

            int i = 0;
            foreach (var item in buttons)
            {
                item.transform.localScale = Vector3.one * 0.75f;
                item.transform.localPosition = new Vector3(-2.96f, 0.9f - i * 0.55f, -2f);
                i++;
            }
            for (int j = 0; j < buttons.Count(); j++)
            {
                int copyindex = j;
                var button = buttons[j];
                button.OnClick = new();
                button.OnClick.AddListener((UnityAction)(() =>
                {
                    __instance.GameSettingsTab.gameObject.SetActive(false);
                    __instance.RoleSettingsTab.gameObject.SetActive(false);
                    __instance.PresetsTab.gameObject.SetActive(false);
                    buttons.Do(x => { x.transform.FindChild("Selected").gameObject.SetActive(false); });

                    tabs.Do(x => x.gameObject.SetActive(false));
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
                        RoleTab.gameObject.SetActive(true);
                    }
                    else if (copyindex == 3)
                    {
                        CrewmateTab.gameObject.SetActive(true);
                    }
                    else if (copyindex == 4)
                    {

                        ImpostorTab.gameObject.SetActive(true);
                    }
                    else if (copyindex == 5)
                    {

                        NeutralTab.gameObject.SetActive(true);
                    }
                }));
            }
            if (CustomOption.options.Count == 0)
            {
                CustomOptionsHolder.CreateCustomOptions();
            }
            optionTypeCounter.Clear();
            foreach (OptionType item in Enum.GetValues(typeof(OptionType)))
            {

                CustomOption.optionTypeCounter.Add(item, 1.5f);
            }
            foreach (var option in CustomOption.options)
            {
                option.OptionCloneSet();
            }

            foreach (var optiontype in optionTypeCounter)
            {
                Scroller sc = null;
                switch (optiontype.Key)
                {

                    case OptionType.General:
                        sc = CustomOptions.TSRTab.scrollBar;
                        break;
                    case OptionType.Roles:
                        sc = CustomOptions.RoleTab.scrollBar;
                        break;
                    case OptionType.Crewmate:
                        sc = CustomOptions.CrewmateTab.scrollBar;
                        break;
                    case OptionType.Impostor:
                        sc = CustomOptions.ImpostorTab.scrollBar;
                        break;
                    case OptionType.Neutral:
                        sc = CustomOptions.NeutralTab.scrollBar;
                        break;
                }
                if (sc == null) continue;
                sc.ContentYBounds = new FloatRange(0, -1.5f - optiontype.Value);

            }
        }

    }
    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.OnPlayerJoined))]
    public static class JoinGamePatch
    {
        public static void Postfix()
        {
            if (AmongUsClient.Instance.AmHost)
            {
                ShareAllOptions();

            }
        }
    }
    [HarmonyPatch]
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
            Crewmate,
            Impostor,

            Neutral,
        }
        public Color color = Color.white;
        public string nameId;
        public CustomRange Range;
        public ModOption ModOption;
        public int Selection() => entry.Value;
        public int defaultSelection;
        public ConfigEntry<int> entry;
        public Action onChange;
        public OptionType optionType;
        public static List<CustomOption> options = new();
        Func<bool> Show;
        CategoryHeaderMasked categoryHeaderMasked;
        public StringNames[] GetStringNames(string[] str)
        {
            List<StringNames> k = [];
            foreach (var item in str)
            {
                k.AddItem(StringNames.ImpostorsCategory);
            }
            return k.ToArray();
        }
        public string ValueText()
        {

            return /*"<b>"*/  Range.GetRange(Selection());
        }
        public float GetValue()
        {

            return /*"<b>"*/  Range.GetValue(Selection());
        }

        public int GetIntValue() => (int)GetValue();

        public float GetFloatValue() => GetValue();
        public bool GetBoolValue() => GetValue() == OSAS.on.GetValueFromSelector();




        public string Title()
        {

            if (nameId.StartsWith("header.team_"))
            {
                var ids = nameId.Split('_');
                return /*"<b>"*/  RoleData.GetCustomTeams.First(x => x.Team.ToString().ToLower() == ids[1].ToLower()).ColoredTeamName;
            }
            if (nameId.StartsWith("header.role_"))
            {
                var ids = nameId.Split('_');
                return /*"<b>"*/  RoleData.GetCustomRoles.First(x => x.Role.ToString().ToLower() == ids[1].ToLower()).ColoredRoleName;
            }
            if (nameId.StartsWith("team_"))
            {
                var ids = nameId.Split('_');
                return /*"<b>"*/  RoleData.GetCustomTeams.First(x => x.Team.ToString().ToLower() == ids[1].ToLower()).ColoredTeamName;
            }
            if (nameId.StartsWith("role_"))
            {
                var ids = nameId.Split('_');
                if (ids[2] == "count")
                {
                    return /*"<b>"*/  RoleData.GetCustomRoles.First(x => x.Role.ToString().ToLower() == ids[1].ToLower()).ColoredRoleName;
                }
            }
            return /*"<b>"*/  Translation.GetString($"option.{nameId}");
        }
        public static CustomOption CreateOption(OptionType optionType, string nameId, CustomRange range, int defaultSelection, Func<bool> Show = null, Action onChange = null, bool isHeader = false, string colorcode = "#ffffff")
        {
            if (options.Any(x => x.nameId == nameId))
            {
                return options.First(x => x.nameId == nameId);
            }
            return new CustomOption(optionType, nameId, range, defaultSelection, Show, onChange, isHeader, colorcode);
        }
        public CustomOption(OptionType optionType, string nameId, CustomRange range, int defaultSelection, Func<bool> Show = null, Action onChange = null, bool isHeader = false, string colorcode = "#ffffff")
        {
            this.color = Helper.ColorFromColorcode(colorcode);
            this.nameId = nameId;
            this.defaultSelection = defaultSelection;
            this.onChange = onChange;
            this.Range = range;
            this.optionType = optionType;
            this.Show = Show ?? (() => true);
            ModOption = new ModOption();
            ModOption.isHeader = isHeader;
            if (isHeader)
            {

                this.nameId = "header." + nameId;
            }
            else
            {

                entry = TSR.Instance.Config.Bind($"CustomOption", nameId, defaultSelection);
            }
            options.Add(this);
        }
        public void OptionCloneSet()
        {
            if (ModOption.isHeader)
            {
                categoryHeaderMasked = null;

                switch (optionType)
                {
                    case OptionType.Default:
                        categoryHeaderMasked = GameObject.Instantiate<CategoryHeaderMasked>(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, GameSettingMenu.Instance.GameSettingsTab.settingsContainer);
                        break;
                    case OptionType.General:
                        categoryHeaderMasked = GameObject.Instantiate<CategoryHeaderMasked>(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, CustomOptions.TSRTab.settingsContainer);
                        break;
                    case OptionType.Roles:
                        categoryHeaderMasked = GameObject.Instantiate<CategoryHeaderMasked>(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, CustomOptions.RoleTab.settingsContainer);
                        break;
                    case OptionType.Crewmate:
                        categoryHeaderMasked = GameObject.Instantiate<CategoryHeaderMasked>(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, CustomOptions.CrewmateTab.settingsContainer);
                        break;
                    case OptionType.Impostor:
                        categoryHeaderMasked = GameObject.Instantiate<CategoryHeaderMasked>(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, CustomOptions.ImpostorTab.settingsContainer);
                        break;
                    case OptionType.Neutral:
                        categoryHeaderMasked = GameObject.Instantiate<CategoryHeaderMasked>(GameSettingMenu.Instance.GameSettingsTab.categoryHeaderOrigin, CustomOptions.NeutralTab.settingsContainer);
                        break;
                }
                if (categoryHeaderMasked == null) Logger.Error("CategroyHeaderMasked is not found");
                categoryHeaderMasked.SetHeader(StringNames.ImpostorsCategory, 20);
                categoryHeaderMasked.Title.text = Title();
                categoryHeaderMasked.Title.outlineColor = Color.black;
                categoryHeaderMasked.Title.outlineWidth = 0.1f;
                categoryHeaderMasked.Title.fontSize = categoryHeaderMasked.Title.fontSizeMax  = 3f;
                categoryHeaderMasked.Title.fontSizeMin = 1f;
                categoryHeaderMasked.Title.fontStyle = TMPro.FontStyles.Bold;
                categoryHeaderMasked.transform.localPosition = new Vector3(-0.903f, optionTypeCounter[optionType], -2f);
                categoryHeaderMasked.transform.localScale = Vector3.one * 0.63f;
                categoryHeaderMasked.Title.enableAutoSizing = true;
                categoryHeaderMasked.Title.enableWordWrapping = false;
                //categoryHeaderMasked.Background.color = color;
                OptionTypeCounterCountup(optionType, -0.63f);

                //GameSettingMenu.Instance.StartCoroutine(Effects.Lerp(1f, new Action<float>((p) =>
                //{
                //    categoryHeaderMasked.Title.text = Title();
                //})));
                categoryHeaderMasked.gameObject.name = nameId;
            }
            else
            {

                StringOption stringOption = null;
                switch (optionType)
                {
                    case OptionType.Default:
                        stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, GameSettingMenu.Instance.GameSettingsTab.settingsContainer);

                        stringOption.SetClickMask(GameSettingMenu.Instance.GameSettingsTab.ButtonClickMask);
                        break;
                    case OptionType.General:
                        //Scroller/SliderInner
                        stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, CustomOptions.TSRTab.settingsContainer);

                        stringOption.SetClickMask(CustomOptions.TSRTab.ButtonClickMask);
                        break;
                    case OptionType.Roles:
                        //Scroller/SliderInner
                        stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, CustomOptions.RoleTab.settingsContainer);

                        stringOption.SetClickMask(CustomOptions.RoleTab.ButtonClickMask);
                        break;
                    case OptionType.Crewmate:
                        //Scroller/SliderInner
                        stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, CustomOptions.CrewmateTab.settingsContainer);

                        stringOption.SetClickMask(CustomOptions.CrewmateTab.ButtonClickMask);
                        break;
                    case OptionType.Impostor:
                        //Scroller/SliderInner
                        stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, CustomOptions.ImpostorTab.settingsContainer);

                        stringOption.SetClickMask(CustomOptions.ImpostorTab.ButtonClickMask);
                        break;
                    case OptionType.Neutral:
                        //Scroller/SliderInner
                        stringOption = GameObject.Instantiate(GameSettingMenu.Instance.GameSettingsTab.stringOptionOrigin, CustomOptions.NeutralTab.settingsContainer);

                        stringOption.SetClickMask(CustomOptions.NeutralTab.ButtonClickMask);
                        break;
                }
                stringOption.gameObject.name = nameId;
                Logger.Info(stringOption.name, nameId);
                ModOption.StringOption = stringOption;
                ModOption.StringOption.Values = new Il2CppInterop.Runtime.InteropTypes.Arrays.Il2CppStructArray<StringNames>(100);
                ModOption.StringOption.Value = Selection();
                //ModOption.StringOption.LabelBackground.sprite = Sprites.GetSpriteFromResources("ui.option_background.png");
                ModOption.StringOption.LabelBackground.color = color;
                ModOption.TitleText = ModOption.StringOption.TitleText;
                ModOption.ValueText = ModOption.StringOption.ValueText;
                ModOption.StringOption.TitleText.outlineColor = Color.black;
                ModOption.StringOption.TitleText.outlineWidth = 0.1f;
                ModOption.StringOption.TitleText.enableAutoSizing = false;
                ModOption.StringOption.TitleText.enableWordWrapping = false;
                ModOption.StringOption.TitleText.alignment = TextAlignmentOptions.Left;
                ModOption.StringOption.TitleText.fontSize = ModOption.StringOption.TitleText.fontSizeMax = 5f;
                ModOption.StringOption.TitleText.fontSizeMin = 1f;
                ModOption.StringOption.TitleText.fontStyle = TMPro.FontStyles.Bold;
                ModOption.StringOption.TitleText.enableAutoSizing = true;
                ModOption.StringOption.TitleText.enableWordWrapping = true;
                ModOption.CustomOption = this;
                Logger.Info(nameId);

                ModOption.StringOption.ValueText.transform.localPosition = new(3.5f, -0.046f, -1);

                //ModOption.StringOption..transform.localPosition = new Vector3(3.5f, -0.05f, 0);
                ModOption.StringOption.LabelBackground.transform.localPosition = new(-1.5f, -0.06f, 0);
                ModOption.StringOption.LabelBackground.GetComponent<SpriteRenderer>().size = new(6, 0.7f);
                ModOption.StringOption.TitleText.rectTransform.sizeDelta = new Vector2(5.6f, 0.5f);
                ModOption.StringOption.TitleText.transform.localPosition = new Vector3(-1.5f, -0.06f, -3);
                ModOption.StringOption.transform.FindChild("ValueBox").localPosition = new(3.5f, -0.05f, 0);
                ModOption.StringOption.transform.FindChild("MinusButton").localPosition = new(2.1f, -0.05f, 0);
                ModOption.StringOption.transform.FindChild("PlusButton").localPosition = new(4.9f, -0.05f, 0);
                //ValueBox 3.5 -0.05 0
                //valuetext 3.5 -0.046 -1
                //plus 0.7 minus -1.4

                //labelback size (6,0.7)  -1.5,-0.062,0
                //title -3.6 -0.06 -3
                //TORから
                SpriteRenderer[] componentsInChildren = stringOption.GetComponentsInChildren<SpriteRenderer>(true);
                for (int i = 0; i < componentsInChildren.Length; i++)
                {
                    try
                    {

                        componentsInChildren[i].material.SetInt(PlayerMaterial.MaskLayer, 20);
                    }
                    catch { }
                }

                foreach (TextMeshPro textMeshPro in stringOption.GetComponentsInChildren<TextMeshPro>(true))
                {
                    try
                    {
                        textMeshPro.fontMaterial.SetFloat("_StencilComp", 3f);
                        textMeshPro.fontMaterial.SetFloat("_Stencil", (float)20);
                    }
                    catch
                    { }
                }


                stringOption.OnValueChanged = new Action<OptionBehaviour>((o) => { });
                stringOption.transform.localPosition = new Vector3(0.952f, optionTypeCounter[optionType], -2);
                UpdateSelection(Selection());
                GameSettingMenu.Instance.StartCoroutine(Effects.Lerp(0f, new Action<float>((p) =>
                            {
                                ModOption.StringOption.TitleText.text = Title();
                                ModOption.StringOption.ValueText.text = ValueText();
                            })));
                ModOption.StringOption.Value = ModOption.StringOption.oldValue = Selection();
                OptionTypeCounterCountup(optionType, -0.45f);

            }

        }
        public static CustomOption GetOption(string nameId)
        {
            return options.First(x => x.nameId == nameId);
        }

        public static CustomOption Create(OptionType optionType, string name, CustomRange range, float DefaultValue = 0, Func<bool> Show = null, Action onChange = null, string colorcode = "#4f4f4f")
        {
            return CreateOption(optionType, name, range, range.GetSelectors().ToList().IndexOf(DefaultValue.ToString()), Show, onChange, colorcode: colorcode);
        }
        public static CustomOption Create(OptionType optionType, string name, CustomRange range, OSAS selector, Func<bool> Show = null, Action onChange = null, string colorcode = "#4f4f4f")
        {
            return CreateOption(optionType, name, range, range.GetSelectors().ToList().IndexOf(selector.GetStringFromSelector()), Show, onChange, colorcode: colorcode);
        }
        public static CustomOption Create(OptionType optionType, string name, CustomRange range, int selector, Func<bool> Show = null, Action onChange = null, string colorcode = "#4f4f4f")
        {
            return CreateOption(optionType, name, range, selector, Show, onChange, colorcode: colorcode);
        }

        public static CustomOption Create(OptionType optionType, string name, bool selector, Func<bool> Show = null, Action onChange = null, string colorcode = "#4f4f4f")
        {
            return CreateOption(optionType, name, new CustomBoolRange(), selector switch { true => 0, false => 1 }, Show, onChange, colorcode: colorcode);
        }
        public static CustomOption HeaderCreate(OptionType optionType, string nameId, string colorcode = "#4f4f4f")
        {
            return CreateOption(optionType, nameId, null, 0, isHeader: true, colorcode: colorcode);
        }
        public string[] selections => Range.GetSelectors();
        public void UpdateSelection(int selecting)
        {
            selecting = Mathf.Clamp((selecting + selections.Length) % selections.Length, 0, selections.Length - 1);
            if (!ModOption.isHeader)
            {

                Logger.Info($"{nameId}:{Selection()} -> {selecting}");
                if (selecting != Selection())
                {
                    if (onChange != null) onChange.Invoke();
                    //var ob = GameObject.Instantiate(FastDestroyableSingleton<NotificationPopper>.Instance.notificationMessageOrigin, FastDestroyableSingleton<NotificationPopper>.Instance.transform).GetComponent<LobbyNotificationMessage>();
                    //ob.SetUp($"<b>{Title()}</b> を <b>{Value()}</b> に設定する", FastDestroyableSingleton<NotificationPopper>.Instance.notificationMessageOrigin.Icon.sprite, Color.white, (Action)(() => { FastDestroyableSingleton<NotificationPopper>.Instance.activeMessages.Remove(ob); }));
                    //FastDestroyableSingleton<NotificationPopper>.Instance.activeMessages.Add(ob);
                }
                if (AmongUsClient.Instance.AmHost)
                {

                    ShareOption();
                    ModOption.StringOption.Value = ModOption.StringOption.oldValue = selecting;
                    entry.Value = selecting;
                    ModOption.StringOption.ValueText.text = ValueText();
                }

                entry.Value = selecting;
            }
        }
        public static void ShareAllOptions()
        {
            var rpc = CustomRPC.SendRpc(Rpcs.ShareOptions);
            rpc.Write((uint)options.Where(x => !x.ModOption.isHeader).Count());
            foreach (var option in options.Where(x => !x.ModOption.isHeader))
            {

                rpc.Write(option.nameId);
                rpc.Write((uint)option.entry.Value);
            }
            AmongUsClient.Instance.FinishRpcImmediately(rpc);
        }
        public void ShareOption()
        {
            if (!ModOption.isHeader)
            {
                var rpc = CustomRPC.SendRpc(Rpcs.ShareOptions);
                rpc.Write((uint)1);
                rpc.Write(nameId);
                rpc.Write((uint)Selection());
                AmongUsClient.Instance.FinishRpcImmediately(rpc);
            }
        }
        public static void RecieveOption(MessageReader reader)
        {
            uint count = reader.ReadUInt32();
            for (int i = 0; i < count; i++)
            {
                string str = reader.ReadString();
                uint value = reader.ReadUInt32();
                Logger.Message(str + ":" + options.FirstOrDefault(x => x.nameId == str).Selection().ToString() + "->" + value, "RecieveOption");
                options.FirstOrDefault(x => x.nameId == str).UpdateSelection((int)value);
            }
        }
        [HarmonyPatch(typeof(StringOption))]
        private static class CustomStringOption
        {
            [HarmonyPatch(nameof(StringOption.Increase)), HarmonyPrefix]
            private static bool Increase(StringOption __instance)
            {
                CustomOption option = CustomOption.options.First(x => x.ModOption.StringOption == __instance);
                if (option == null) return true;
                option.UpdateSelection(option.Selection() + 1);
                //option.ModOption.Increase();
                return false;
            }
            [HarmonyPatch(nameof(StringOption.Decrease)), HarmonyPrefix]
            private static bool Decrease(StringOption __instance)
            {
                CustomOption option = CustomOption.options.First(x => x.ModOption.StringOption == __instance);
                if (option == null) return true;
                option.UpdateSelection(option.Selection() - 1);
                //option.ModOption.Decrease();
                return false;
            }

            //[HarmonyPatch(nameof(StringOption.Increase)), HarmonyPostfix]
            //private static void Increase(StringOption __instance)
            //{
            //    CustomOption option = CustomOption.options.FirstOrDefault(x => x.ModOption.StringOption == __instance);
            //    if (option == null) return;
            //    option.ModOption.Increase();
            //}
            //[HarmonyPatch(nameof(StringOption.Decrease)), HarmonyPostfix]
            //private static void Decrease(StringOption __instance)
            //{
            //    CustomOption option = CustomOption.options.FirstOrDefault(x => x.ModOption.StringOption == __instance);
            //    if (option == null) return;
            //    option.ModOption.Decrease();
            //}
        }
    }



}
