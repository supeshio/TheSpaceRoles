using AmongUs.GameOptions;
using BepInEx.Configuration;
using HarmonyLib;
using Hazel;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Il2CppSystem.DateTimeParse;

namespace TheSpaceRoles
{
    [Serializable]
    /// <summary>
    /// めっちゃTORからもってきました
    /// </summary>
    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
    class GameOptionsMenuStartPatch
    {
        public static void Postfix(GameOptionsMenu __instance)
        {
            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();

            if (template == null) { return; }
            if (__instance?.transform?.FindChild("TSRSettings") != null) { return; }
            if (__instance?.transform?.FindChild("CustomRoleSettings") != null) { return; }
            var gameSettings = GameObject.Find("Game Settings");
            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
            var tsrSettings = HudManager.Instance.transform.FindChild("CustomSettings").FindChild("TSRSettings");
            var customroleSettings = HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings");


            var gameTab = GameObject.Find("GameTab");
            var roleTab = GameObject.Find("RoleTab");


            var tsrTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var tsrTabHighlight = tsrTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            //tsrTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Sprites.GetSprite("TSRlogo.png", 100f);

            var customroleTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
            var customroleTabHighlight = customroleTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
            //customroleTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Sprites.GetSprite("TSRlogo.png", 100f);


            gameTab.transform.position += Vector3.left * 3.5f;
            roleTab.transform.position += Vector3.left * 3.75f;
            tsrTab.transform.position += Vector3.left * 2.75f;
            customroleTab.transform.position += Vector3.left * 1.75f;


            gameSettingMenu.RolesSettings.gameObject.SetActive(false);
            tsrSettings.gameObject.SetActive(false);
            customroleSettings.gameObject.SetActive(false);
            gameSettingMenu.RolesSettingsHightlight.enabled = false;
            tsrTabHighlight.enabled = false;
            customroleTabHighlight.enabled = false;

            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek)
                gameSettingMenu.HideNSeekSettings.gameObject.SetActive(true);
            else
                gameSettingMenu.RegularGameSettings.SetActive(true);
            gameSettingMenu.GameSettingsHightlight.enabled = true;

            var tabs = new GameObject[] { gameTab, roleTab, tsrTab, customroleTab };
            for (int i = 0; i < tabs.Length; i++)
            {
                var tab = tabs[i];
                var button = tab.GetComponentInChildren<PassiveButton>();
                var copyindex = i;
                if (button == null) continue;
                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
                button.OnClick.AddListener((System.Action)(() =>
                {
                    gameSettingMenu.RegularGameSettings.SetActive(false);
                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
                    gameSettingMenu.HideNSeekSettings.gameObject.SetActive(false);
                    tsrSettings.gameObject.SetActive(false);
                    customroleSettings.gameObject.SetActive(false);
                    gameSettingMenu.GameSettingsHightlight.enabled = false;
                    gameSettingMenu.RolesSettingsHightlight.enabled = false;
                    tsrTabHighlight.enabled = false;
                    customroleTabHighlight.enabled = false;
                    if (copyindex == 0)
                    {

                        Logger.Info($"open : {copyindex},{((GameObject.Find("TSRSettings") == null) ? "null" : "TSRSettings!")}");

                        if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek)
                            gameSettingMenu.HideNSeekSettings.gameObject.SetActive(true);
                        else
                            gameSettingMenu.RegularGameSettings.SetActive(true);
                        gameSettingMenu.GameSettingsHightlight.enabled = true;
                    }
                    else if (copyindex == 1)
                    {

                        Logger.Info($"open : {copyindex}");
                        gameSettingMenu.RolesSettings.gameObject.SetActive(true);
                        gameSettingMenu.RolesSettingsHightlight.enabled = true;
                    }
                    else if (copyindex == 2)
                    {
                        Logger.Info($"open : {copyindex}");
                        tsrSettings.gameObject.SetActive(true);
                        tsrTabHighlight.enabled = true;

                    }
                    else if (copyindex == 3)
                    {
                        Logger.Info($"open : {copyindex}");
                        customroleSettings.gameObject.SetActive(true);
                        customroleTabHighlight.enabled = true;
                    }
                }));
            }






            AdaptTaskCount(__instance);
        }
        private static void AdaptTaskCount(GameOptionsMenu __instance)
        {
            // Adapt task count for main options
            var commonTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
            if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);

            var shortTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

            var longTasksOption = __instance.Children.FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
        }

    }
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start))]
    class GameSettingStart
    {
        public static void Postfix(PlayerControl __instance)
        {

            var cSettings = new GameObject("CustomSettings");
            cSettings.transform.parent = HudManager.Instance.transform;
            cSettings.active = true;
            cSettings.transform.localPosition = new(0,0,-160);
            var tsrSettings = new GameObject("TSRSettings");
            tsrSettings.transform.parent = cSettings.transform;
            tsrSettings.active = false;
            tsrSettings.transform.localPosition = Vector3.zero;
            var customroleSettings = new GameObject("CustomRoleSettings");
            customroleSettings.transform.parent = cSettings.transform ;
            customroleSettings.active = false;
            customroleSettings.transform.localPosition = Vector3.zero;
            CustomOptionsHolder.CreateCustomOptions();
            CustomOptionsHolder.AllCheck();
        }


    }

    public class CustomOption
    {
        public enum setting
        {
            TSRSettings,
            RoleSettings
        }
        public static Transform GetTransformFromSetting(setting setting)
        { var csetting = HudManager.Instance.transform.FindChild("CustomSettings");
            return setting switch
            {
                setting.TSRSettings => csetting.FindChild("TSRSettings"),
                setting.RoleSettings => csetting.FindChild("CustomRoleSettings"),
                _ => csetting.FindChild("TSRSettings"),
            }; ;
        }
        public static int preset = 0;
        public string GetName() => Translation.GetString("tsroption." + name);
        public string GetSelectionName() => Translation.GetString("tsroption.selection." + selections[selection]);

        public string name;
        public string parentId;
        public int selection => entry.Value;
        public int defaultSelection;
        public object[] selections;
        public ConfigEntry<int> entry;
        public GameObject @object;
        public Func<int, bool> func;
        public Action onChange;
        public OptionBehaviour optionBehaviour;
        public setting obj_parent;
        public TextMeshPro Title_TMP;
        public TextMeshPro Value_TMP;
        public CustomOption(setting parent,string name,
            object[] selections, object dafaultValue, string parentId = null, Func<int, bool> func = null, Action onChange = null
            )
        {
            this.obj_parent = parent;
            this.name = name;
            this.func = func;
            this.onChange = onChange;
            this.selections = selections;
            int index = Array.IndexOf(selections, dafaultValue);
            this.defaultSelection = index >= 0 ? index : 0;
            this.parentId = parentId;

            entry = TSR.Instance.Config.Bind($"Preset{preset}", name, defaultSelection);

            @object = new GameObject();
            var renderer = @object.AddComponent<SpriteRenderer>();
            renderer.sprite = Sprites.GetSpriteFromResources("banner.png",200);
            renderer.color = Helper.ColorFromColorcode("#333333");
            @object.name = name;
            @object.transform.parent = GetTransformFromSetting(parent);
            @object.active = true;
            @object.layer = HudManager.Instance.gameObject.layer;
            @object.transform.localPosition = Vector3.zero;
            @object.transform.localScale = new(0.9f,0.9f,0.9f);

            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
            Title_TMP.transform.parent = @object.transform;
            Title_TMP.fontStyle = FontStyles.Bold;
            Title_TMP.text = GetName();
            Title_TMP.fontSize = Title_TMP.fontSizeMax= 2f;
            Title_TMP.fontSizeMin = 1f;
            Title_TMP.alignment = TextAlignmentOptions.Left;
            Title_TMP.enableWordWrapping = false;
            Title_TMP.outlineWidth = 0.8f;
            Title_TMP.autoSizeTextContainer = false;
            Title_TMP.transform.localPosition = new Vector3(-2.3f, 0, -1);
            Title_TMP.transform.localScale = Vector3.one;
            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
            Title_TMP.m_sharedMaterial = HudManager.Instance.GameSettings.m_sharedMaterial;
            Title_TMP.gameObject.AddComponent<TextContainer>().rectTransform.sizeDelta = new Vector2(50, 0);

            Value_TMP = new GameObject("Value_TMP").AddComponent<TextMeshPro>();
            Value_TMP.transform.parent = @object.transform;
            Value_TMP.fontStyle = FontStyles.Bold;
            Value_TMP.text = GetSelectionName();
            Value_TMP.fontSize = Value_TMP.fontSizeMax = 2f;
            Value_TMP.fontSizeMin = 0.4f;
            Value_TMP.alignment = TextAlignmentOptions.Center;
            Value_TMP.enableWordWrapping = false;
            Value_TMP.outlineWidth = 0.8f;
            Value_TMP.enableAutoSizing = true;
            Value_TMP.transform.localPosition = new Vector3(0.7f, -0.25f, -1);
            Value_TMP.transform.localScale = Vector3.one;
            Value_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
            Value_TMP.m_sharedMaterial = HudManager.Instance.GameSettings.m_sharedMaterial;
            Value_TMP.gameObject.AddComponent<TextContainer>().rectTransform.sizeDelta = new Vector2(1f, 0.5f);


        }
        public static CustomOption Create(setting parent,string name, bool DefaultValue = false, string parentId = null, Func<int, bool> func = null, Action onChange = null)
        {
            return new CustomOption(parent,name, ["Off", "On"], DefaultValue ? "On" : "Off", parentId, func, onChange);
        }
        public static Func<int, bool> OnOff = x => x == 0;

        public static CustomOption Create(setting parent,string name, string[] selections, string selection, string parentId = null, Func<int, bool> func = null, Action onChange = null)
        {
            return new CustomOption(parent,name, selections, selection, parentId, func, onChange);
        }
        public void updateSelection(int newSelection)
        {
            Logger.Info($"{name}:{selection}");
            ShareOptionSelections();
            CustomOptionsHolder.AllCheck();
        }
        public void Check(int i)
        {
            Logger.Info(name,"Check");
            if(func == null || parentId == null)
            {

                @object.active = true;
                @object.transform.localPosition = new Vector3(3, 2f - 0.5f * i, 0);
            }
            else
            {

                if (func(selection))
                {
                    @object.active = true;
                    @object.transform.localPosition = new Vector3(3, 2f - 0.3f * i, 0);
                }
                else
                {
                    @object.transform.localPosition = new Vector3(0, 200, 0);
                    @object.active = false;
                }
            }
        }

        public static void ShareOptionSelections()
        {
            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance?.AmHost == false && PlayerControl.LocalPlayer == null) return;

            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)Rpcs.ShareOptions, Hazel.SendOption.Reliable);
            

            writer.WritePacked((uint)CustomOptionsHolder.Options.Select(x => x.Count).Sum());
            foreach(var options in CustomOptionsHolder.Options)
            {
                foreach (CustomOption option in options)
                {
                    writer.WritePacked((uint)Convert.ToUInt32(option.selection));
                }
            }
            writer.EndMessage();
        }
        public static void GetOptionSelections(MessageReader reader)
        {
            if (reader == null) return;

            uint count = reader.ReadPackedUInt32();
            foreach (var options in CustomOptionsHolder.Options)
            {
                foreach (CustomOption option in options)
                {
                    uint i = reader.ReadPackedUInt32();
                    option.entry.Value= (int)i;
                }
            }
            CustomOptionsHolder.AllCheck();
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
    public class RpcSyncSettingsPatch
    {
        public static void Postfix()
        {
            CustomOption.ShareOptionSelections();
        }
    }


}
