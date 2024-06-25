//using AmongUs.GameOptions;
//using BepInEx.Configuration;
//using HarmonyLib;
//using Hazel;
//using System;
//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.Events;

//namespace TheSpaceRoles
//{
//    [Serializable]
//    /// <summary>
//    /// めっちゃTORからもってきました
//    /// </summary>
//    [HarmonyPatch(typeof(GameOptionsMenu), nameof(GameOptionsMenu.Start))]
//    class GameOptionsMenuStartPatch
//    {
//        public static void Postfix(GameOptionsMenu __instance)
//        {
//            var template = UnityEngine.Object.FindObjectsOfType<StringOption>().FirstOrDefault();

//            if (template == null) { return; }
//            if (__instance?.transform?.FindChild("TSRSettings") != null) { return; }
//            if (__instance?.transform?.FindChild("CustomRoleSettings") != null) { return; }
//            var gameSettings = GameObject.Find("Game Settings");
//            var gameSettingMenu = UnityEngine.Object.FindObjectsOfType<GameSettingMenu>().FirstOrDefault();
//            var tsrSettings = HudManager.Instance.transform.FindChild("CustomSettings").FindChild("TSRSettings");
//            var customroleSettings = HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings");


//            var gameTab = GameObject.Find("GameTab");
//            var roleTab = GameObject.Find("RoleTab");


//            var tsrTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
//            var tsrTabHighlight = tsrTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
//            //tsrTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Sprites.GetSprite("TSRlogo.png", 100f);

//            var customroleTab = UnityEngine.Object.Instantiate(roleTab, roleTab.transform.parent);
//            var customroleTabHighlight = customroleTab.transform.FindChild("Hat Button").FindChild("Tab Background").GetComponent<SpriteRenderer>();
//            //customroleTab.transform.FindChild("Hat Button").FindChild("Icon").GetComponent<SpriteRenderer>().sprite = Sprites.GetSprite("TSRlogo.png", 100f);


//            gameTab.transform.position += Vector3.left * 3.5f;
//            roleTab.transform.position += Vector3.left * 3.75f;
//            tsrTab.transform.position += Vector3.left * 2.75f;
//            customroleTab.transform.position += Vector3.left * 1.75f;


//            gameSettingMenu.RolesSettings.gameObject.SetActive(false);
//            tsrSettings.gameObject.SetActive(false);
//            customroleSettings.gameObject.SetActive(false);
//            gameSettingMenu.RolesSettingsHightlight.enabled = false;
//            tsrTabHighlight.enabled = false;
//            customroleTabHighlight.enabled = false;

//            if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek)
//                gameSettingMenu.HideNSeekSettings.gameObject.SetActive(true);
//            else
//                gameSettingMenu.RegularGameSettings.SetActive(true);
//            gameSettingMenu.GameSettingsHightlight.enabled = true;

//            var tabs = new GameObject[] { gameTab, roleTab, tsrTab, customroleTab };
//            for (int i = 0; i < tabs.Length; i++)
//            {
//                var tab = tabs[i];
//                var button = tab.GetComponentInChildren<PassiveButton>();
//                var copyindex = i;
//                if (button == null) continue;
//                button.OnClick = new UnityEngine.UI.Button.ButtonClickedEvent();
//                button.OnClick.AddListener((System.Action)(() =>
//                {
//                    gameSettingMenu.RegularGameSettings.SetActive(false);
//                    gameSettingMenu.RolesSettings.gameObject.SetActive(false);
//                    gameSettingMenu.HideNSeekSettings.gameObject.SetActive(false);
//                    tsrSettings.gameObject.SetActive(false);
//                    customroleSettings.gameObject.SetActive(false);
//                    gameSettingMenu.GameSettingsHightlight.enabled = false;
//                    gameSettingMenu.RolesSettingsHightlight.enabled = false;
//                    tsrTabHighlight.enabled = false;
//                    customroleTabHighlight.enabled = false;
//                    if (copyindex == 0)
//                    {

//                        Logger.Info($"open : {copyindex},{((GameObject.Find("TSRSettings") == null) ? "null" : "TSRSettings!")}");

//                        if (GameOptionsManager.Instance.currentGameMode == GameModes.HideNSeek)
//                            gameSettingMenu.HideNSeekSettings.gameObject.SetActive(true);
//                        else
//                            gameSettingMenu.RegularGameSettings.SetActive(true);
//                        gameSettingMenu.GameSettingsHightlight.enabled = true;
//                    }
//                    else if (copyindex == 1)
//                    {

//                        Logger.Info($"open : {copyindex}");
//                        gameSettingMenu.RolesSettings.gameObject.SetActive(true);
//                        gameSettingMenu.RolesSettingsHightlight.enabled = true;
//                    }
//                    else if (copyindex == 2)
//                    {
//                        Logger.Info($"open : {copyindex}");
//                        tsrSettings.gameObject.SetActive(true);
//                        tsrTabHighlight.enabled = true;

//                    }
//                    else if (copyindex == 3)
//                    {
//                        Logger.Info($"open : {copyindex}");
//                        customroleSettings.gameObject.SetActive(true);
//                        customroleTabHighlight.enabled = true;
//                    }
//                }));
//            }






//            AdaptTaskCount(__instance);
//        }
//        private static void AdaptTaskCount(GameOptionsMenu __instance)
//        {
//            // Adapt task count for main options
//            var commonTasksOption = __instance.Children.ToArray().ToList().FirstOrDefault(x => x.name == "NumCommonTasks").TryCast<NumberOption>();
//            if (commonTasksOption != null) commonTasksOption.ValidRange = new FloatRange(0f, 4f);

//            var shortTasksOption = __instance.Children.ToArray().ToList().FirstOrDefault(x => x.name == "NumShortTasks").TryCast<NumberOption>();
//            if (shortTasksOption != null) shortTasksOption.ValidRange = new FloatRange(0f, 23f);

//            var longTasksOption = __instance.Children.ToArray().ToList().FirstOrDefault(x => x.name == "NumLongTasks").TryCast<NumberOption>();
//            if (longTasksOption != null) longTasksOption.ValidRange = new FloatRange(0f, 15f);
//        }

//    }
//    [HarmonyPatch]
//    class GameSetting
//    {
//        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Start)), HarmonyPostfix]
//        public static void Start_Postfix(PlayerControl __instance)
//        {
//            if (HudManager.Instance.transform.FindChild("CustomSettings") != null)
//            {
//                return;
//            }
//            var cSettings = new GameObject("CustomSettings");
//            cSettings.transform.SetParent(HudManager.Instance.transform);
//            cSettings.active = true;
//            cSettings.transform.localPosition = new(0, 0, -70);
//            var tsrSettings = new GameObject("TSRSettings");
//            tsrSettings.transform.SetParent(cSettings.transform);
//            tsrSettings.active = false;
//            var selector = new GameObject("Selector");
//            selector.transform.SetParent(tsrSettings.transform);
//            selector.transform.localPosition = Vector3.zero;
//            selector.active = true;
//            var opt = new GameObject("Option");
//            opt.transform.SetParent(tsrSettings.transform);
//            opt.transform.localPosition = Vector3.zero;
//            opt.active = true;
//            tsrSettings.transform.localPosition = Vector3.zero;
//            var customroleSettings = new GameObject("CustomRoleSettings");
//            customroleSettings.transform.SetParent(cSettings.transform);
//            customroleSettings.active = false;
//            customroleSettings.transform.localPosition = Vector3.zero;
//            var roles = new GameObject("Roles");
//            roles.transform.SetParent(customroleSettings.transform);
//            roles.transform.localPosition = Vector3.zero;
//            roles.active = true;
//            var teams = new GameObject("Teams");
//            teams.transform.SetParent(customroleSettings.transform);
//            teams.transform.localPosition = Vector3.zero;
//            teams.active = true;
//            var added = new GameObject("AddedRoles");
//            added.transform.SetParent(customroleSettings.transform);
//            added.transform.localPosition = Vector3.zero;
//            added.active = true;
//            var desc = new GameObject("Description");
//            desc.transform.SetParent(customroleSettings.transform);
//            desc.transform.localPosition = Vector3.zero;
//            desc.active = true;
//            var n = new GameObject("E");
//            n.transform.SetParent(customroleSettings.transform);
//            n.transform.localPosition = Vector3.zero;
//            n.active = true;
//            CustomOptionSelectorHolder.CreateSelector();
//            CustomOptionsHolder.CreateCustomOptions();
//            CustomOptionsHolder.AllCheck();
//            RoleOptionsDescription.StartExplain();
//            RoleOptionsHolder.RoleOptionsCreate();
//            RoleOptionTeamsHolder.Create();

//            _ = new ScrollerP("OptSel_Scroller", ref selector, ref tsrSettings, new(-3, 5, 0), new(-5, -5, 0), new(-4.5f, -0.5f, 0), CustomOptionSelector.selectors.Count * 1f);

//            _ = new ScrollerP("Role_Scroller", ref roles, ref customroleSettings, new(-5, 5, 0), new(-7, -5, 0), new(-3.45f, -0.5f, 0), RoleOptionsHolder.roleOptions.Count * 0.36f);
//            _ = new ScrollerP("Team_Scroller", ref teams, ref customroleSettings, new(-3, 5, 0), new(-5, -5, 0), new(-0.5f, -0.5f, 0), RoleOptionTeamsHolder.TeamsHolder.Count * 0.36f);

//            _ = new ScrollerP("Description_Scroller", ref desc, ref customroleSettings, new(-3, 5, 0), new(-5, -5, 0), new(5f, -0.5f, 0), CustomOptionsHolder.RoleOptions.Where(x => x.role == RoleOptionOptions.nowRole && x.team == RoleOptionOptions.nowTeam).ToList().Count * 0.36f + 1.8f);

//            optsc = new ScrollerP("Opt_Scroller", ref opt, ref tsrSettings, new(1, 5, 0), new(5, -5, 0), new(0f, -0.5f, 0), (CustomOptionsHolder.TSROptions.Where(x => x.obj_parent == CustomOptionSelector.Select).Count()) * 0.38f + 0);

//        }
//        public static ScrollerP optsc;
//        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.ExitGame)), HarmonyPostfix]
//        public static void End_Postfix()
//        {
//            CustomOptionSelector.selectors = [];
//            CustomOptionsHolder.Options.Do(x => x = []);
//            RoleOptionsHolder.roleOptions = [];
//            RoleOptionTeamsHolder.TeamsHolder = [];
//            RoleOptionTeamRoles.RoleOptionsInTeam = [];

//        }
//        [HarmonyPatch(typeof(GameManager), nameof(GameManager.EndGame)), HarmonyPostfix]
//        public static void Endgame_Postfix()
//        {
//            CustomOptionSelector.selectors = [];
//            CustomOptionsHolder.Options.Do(x => x = []);
//            RoleOptionsHolder.roleOptions = [];
//            RoleOptionTeamsHolder.TeamsHolder = [];
//            RoleOptionTeamRoles.RoleOptionsInTeam = [];

//        }

//    }

//    public enum CustomSetting
//    {
//        TSRSettings,
//        RoleSettings
//    }
//    public class CustomOption
//    {
//        public static CustomOption GetOption(string option)
//        {
//            foreach (var item in CustomOptionsHolder.Options)
//            {
//                foreach (var item1 in item)
//                {
//                    if (item1.name == option)
//                    {
//                        return item1;
//                    }
//                }
//            }
//            return null;
//        }
//        public static void SetOption(string option, int i)
//        {
//            foreach (var item in CustomOptionsHolder.Options)
//            {
//                foreach (var item1 in item)
//                {
//                    if (item1.name == option)
//                    {
//                        item1.UpdateSelection(i);
//                        return;
//                    }
//                }
//            }
//            return;
//        }
//        public static CustomOption GetRoleOption(string option, Roles roles, Teams teams)
//        {
//            foreach (var item in CustomOptionsHolder.Options)
//            {
//                foreach (var item1 in item)
//                {
//                    if (item1.name == option && item1.team == teams && item1.role == roles)
//                    {
//                        return item1;
//                    }
//                }
//            }
//            return null;
//        }
//        public static void SetRoleOption(string option, Roles roles, Teams teams, int i)
//        {
//            foreach (var item in CustomOptionsHolder.Options)
//            {
//                foreach (var item1 in item)
//                {
//                    if (item1.name == option && item1.team == teams && item1.role == roles)
//                    {
//                        item1.UpdateSelection(i);
//                        return;
//                    }
//                }
//            }
//            return;
//        }
//        public static int preset = 0;
//        public string GetName() => this.CustomSetting == CustomSetting.TSRSettings ? Translation.GetString("option." + name) : Translation.GetString("roption." + name);
//        //public string GetSelectionName() => Translation.GetString("option.selection.sec", [selections[selection].ToString()]);
//        public string GetSelectionName()
//        {

//            return selections[selection]();

//        }
//        public static Transform GetTransformFromSetting(CustomSetting setting, CustomOptionSelectorSetting? parent)
//        {
//            var csetting = HudManager.Instance.transform.FindChild("CustomSettings");
//            return setting switch
//            {
//                CustomSetting.TSRSettings => csetting.FindChild("TSRSettings").FindChild("Option"),
//                CustomSetting.RoleSettings => csetting.FindChild("CustomRoleSettings").FindChild("E"),
//                _ => csetting.FindChild("CustomRoleSettings"),
//            }; ;
//        }
//        public string name;
//        public string parentId;
//        public int selection => entry.Value;
//        public int defaultSelection;
//        public Func<string>[] selections;
//        public ConfigEntry<int> entry;
//        public GameObject @object;
//        public Func<int, bool> func;
//        public Action onChange;
//        public OptionBehaviour optionBehaviour;
//        public CustomOptionSelectorSetting obj_parent;
//        public CustomSetting CustomSetting;
//        public TextMeshPro Title_TMP;
//        public TextMeshPro Value_TMP;
//        public SpriteRenderer right;
//        public SpriteRenderer left;
//        public Roles role;
//        public Teams team;
//        public CustomOption(CustomSetting customSetting, string name,
//            Func<string>[] selections, Func<string> dafaultValue, string parentId = null, Func<int, bool> func = null, Action onChange = null, CustomOptionSelectorSetting? parent = null, Teams team = Teams.None, Roles role = Roles.None
//            )
//        {
//            this.CustomSetting = customSetting;
//            this.role = role;
//            this.team = team;
//            if (parent != null)
//            {

//                this.obj_parent = (CustomOptionSelectorSetting)parent;
//            }
//            this.name = name;
//            this.func = func;
//            this.onChange = onChange;
//            this.selections = selections;
//            int index = Array.IndexOf(selections, dafaultValue);
//            this.defaultSelection = index >= 0 ? index : 0;
//            this.parentId = parentId;

//            if (this.CustomSetting == CustomSetting.TSRSettings)
//            {
//                entry = TSR.Instance.Config.Bind($"Preset{preset}", name, defaultSelection);
//            }
//            else
//            {
//                entry = TSR.Instance.Config.Bind($"Preset{preset}", team.ToString() + "_" + role.ToString() + "_" + name, defaultSelection);
//            }


//            @object = new GameObject();
//            var renderer = @object.AddComponent<SpriteRenderer>();
//            renderer.sprite = Sprites.GetSpriteFromResources("ui.banner.png", 200);
//            renderer.color = Helper.ColorFromColorcode("#333333");
//            if (customSetting == CustomSetting.TSRSettings)
//            {

//                @object.name = name;
//            }
//            else
//            {

//                @object.name = team + "_" + role + "_" + name;
//            }
//            @object.transform.SetParent(GetTransformFromSetting(customSetting, parent));
//            @object.active = true;
//            @object.layer = HudManager.Instance.gameObject.layer;
//            @object.transform.localPosition = Vector3.zero;
//            @object.transform.localScale = new(0.9f, 0.9f, 0.9f);
//            @object.SetActive(false);



//            Title_TMP = new GameObject("Title_TMP").AddComponent<TextMeshPro>();
//            Title_TMP.transform.SetParent(@object.transform);
//            Title_TMP.fontStyle = FontStyles.Bold;
//            Title_TMP.text = GetName();
//            Title_TMP.fontSize = Title_TMP.fontSizeMax = 2f;
//            Title_TMP.fontSizeMin = 1f;
//            Title_TMP.alignment = TextAlignmentOptions.Left;
//            Title_TMP.enableWordWrapping = false;
//            Title_TMP.outlineWidth = 0.8f;
//            Title_TMP.autoSizeTextContainer = false;
//            Title_TMP.enableAutoSizing = true;
//            Title_TMP.transform.localPosition = new Vector3(-2.3f, 0, -1);
//            Title_TMP.transform.localScale = Vector3.one;
//            Title_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
//            Title_TMP.m_sharedMaterial = Data.textMaterial;
//            Title_TMP.rectTransform.pivot = new Vector2(0, 0.5f);
//            Title_TMP.rectTransform.sizeDelta = new Vector2(2.7f, 0.5f);

//            Value_TMP = new GameObject("Value_TMP").AddComponent<TextMeshPro>();
//            Value_TMP.transform.SetParent(@object.transform);
//            Value_TMP.fontStyle = FontStyles.Bold;
//            Value_TMP.text = GetSelectionName();
//            Value_TMP.fontSize = Value_TMP.fontSizeMax = 2f;
//            Value_TMP.fontSizeMin = 0.4f;
//            Value_TMP.alignment = TextAlignmentOptions.Center;
//            Value_TMP.enableWordWrapping = false;
//            Value_TMP.outlineWidth = 0.8f;
//            Value_TMP.enableAutoSizing = true;
//            Value_TMP.transform.localPosition = new Vector3(1.3f, 0f, -1);
//            Value_TMP.transform.localScale = Vector3.one;
//            Value_TMP.gameObject.layer = HudManager.Instance.gameObject.layer;
//            Value_TMP.m_sharedMaterial = Data.textMaterial;
//            Value_TMP.rectTransform.pivot = new Vector2(0.5f, 0.5f);
//            Value_TMP.rectTransform.sizeDelta = new Vector2(1, 0.5f);


//            right = new GameObject("right").AddComponent<SpriteRenderer>();
//            right.sprite = Sprites.GetSpriteFromResources("ui.double_right.png", 80);
//            right.gameObject.layer = HudManager.Instance.gameObject.layer;
//            right.transform.SetParent(@object.transform);
//            right.transform.localScale = Vector3.one;
//            right.transform.localPosition = new Vector3(2.1f, 0, -1);
//            right.color = Color.white;
//            right.material = Data.textMaterial;
//            right.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
//            var rbutton = right.gameObject.AddComponent<PassiveButton>();
//            rbutton.OnClick = new();
//            rbutton.OnMouseOut = new UnityEvent();
//            rbutton.OnMouseOver = new UnityEvent();
//            rbutton._CachedZ_k__BackingField = 0.1f;
//            rbutton.CachedZ = 0.1f;
//            rbutton.Colliders = new[] { right.GetComponent<BoxCollider2D>() };
//            rbutton.OnClick.AddListener((System.Action)(() => { UpdateSelection(selection + 1); }));
//            rbutton.OnMouseOver.AddListener((System.Action)(() => { right.color = Palette.AcceptedGreen; }));
//            rbutton.OnMouseOut.AddListener((System.Action)(() => { right.color = Color.white; }));
//            rbutton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
//            rbutton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;


//            left = new GameObject("left").AddComponent<SpriteRenderer>();
//            left.sprite = Sprites.GetSpriteFromResources("ui.double_left.png", 80);
//            left.gameObject.layer = HudManager.Instance.gameObject.layer;
//            left.transform.SetParent(@object.transform);
//            left.transform.localScale = Vector3.one;
//            left.color = Color.white;
//            left.transform.localPosition = new Vector3(0.5f, 0, -1);
//            left.material = Data.textMaterial;
//            left.gameObject.AddComponent<BoxCollider2D>().size = new Vector2(0.5f, 0.5f);
//            var lbutton = left.gameObject.AddComponent<PassiveButton>();
//            lbutton.OnClick = new();
//            lbutton.OnMouseOut = new UnityEvent();
//            lbutton.OnMouseOver = new UnityEvent();
//            lbutton._CachedZ_k__BackingField = 0.1f;
//            lbutton.CachedZ = 0.1f;
//            lbutton.Colliders = new[] { left.GetComponent<BoxCollider2D>() };
//            lbutton.OnClick.AddListener((UnityAction)(() => { UpdateSelection(selection - 1); }));
//            lbutton.OnMouseOver.AddListener((UnityAction)(() => { left.color = Palette.AcceptedGreen; }));
//            lbutton.OnMouseOut.AddListener((UnityAction)(() => { left.color = Color.white; }));
//            lbutton.HoverSound = HudManager.Instance.Chat.GetComponentsInChildren<ButtonRolloverHandler>().FirstOrDefault().HoverSound;
//            lbutton.ClickSound = HudManager.Instance.Chat.quickChatMenu.closeButton.ClickSound;

//        }
//        public static Func<string> GetOptionSelection(string str, string[] strs = null)
//        {
//            return () => Translation.GetString("option.selection." + str, strs);
//        }

//        public static Func<string> On() => GetOptionSelection("on");
//        public static Func<string> Off() => GetOptionSelection("off");
//        public static Func<string> Unlimited() => GetOptionSelection("unlimited");
//        public static Func<string> Right() => GetOptionSelection("right");
//        public static Func<string> Left() => GetOptionSelection("left");
//        public static Func<string> Sec(float x) => GetOptionSelection("second", [x.ToString()]);

//        public static Func<int, bool> funcOn = x => x != 0;
//        public static Func<int, bool> funcOff = x => x == 0;


//        public static void TSRCreate(CustomOptionSelectorSetting parent, string name, bool DefaultValue = false, string parentId = null, Func<int, bool> func = null, Action onChange = null)
//        {
//            if (CustomOptionsHolder.TSROptions.Any(x => x.name == name)) return;

//            var v = new CustomOption(CustomSetting.TSRSettings, name, [Off(), On()], DefaultValue ? On() : Off(), parentId, func, onChange, parent);
//            CustomOptionsHolder.TSROptions.Add(v);

//        }
//        public static void TSRCreate(CustomOptionSelectorSetting parent, string name, Func<string>[] selections, Func<string> selection, string parentId = null, Func<int, bool> func = null, Action onChange = null)
//        {
//            if (CustomOptionsHolder.TSROptions.Any(x => x.name == name)) return;
//            var v = new CustomOption(CustomSetting.TSRSettings, name, selections, selection, parentId, func, onChange, parent);
//            CustomOptionsHolder.TSROptions.Add(v);
//        }


//        public static void RoleCreate(Teams teams, Roles roles, string name, bool DefaultValue = false, string parentId = null, Func<int, bool> func = null, Action onChange = null)
//        {
//            if (CustomOptionsHolder.RoleOptions.Any(x => x.name == name && x.team == teams && x.role == roles)) return;
//            var v = new CustomOption(CustomSetting.RoleSettings, name, [Off(), On()], DefaultValue ? On() : Off(), parentId, func, onChange, role: roles, team: teams);
//            CustomOptionsHolder.RoleOptions.Add(v);
//        }
//        public static void RoleCreate(Teams teams, Roles roles, string name, Func<string>[] selections, Func<string> selection, string parentId = null, Func<int, bool> func = null, Action onChange = null)
//        {
//            if (CustomOptionsHolder.RoleOptions.Any(x => x.name == name && x.team == teams && x.role == roles)) return;

//            var v = new CustomOption(CustomSetting.RoleSettings, name, selections, selection, parentId, func, onChange, role: roles, team: teams);
//            CustomOptionsHolder.RoleOptions.Add(v);
//        }



//        public void UpdateSelection(int newSelection)
//        {
//            entry.Value = ((newSelection % selections.Length) + selections.Length) % selections.Length;
//            Logger.Info($"{name}:{selection}");
//            ShareOptionSelections();
//            if (this.CustomSetting == CustomSetting.TSRSettings)
//            {
//                CustomOptionsHolder.AllCheck();

//            }
//            else
//            {
//                RoleOptionOptions.Check(this.team, this.role);
//            }
//            onChange?.Invoke();
//        }
//        public void Check(float i)
//        {
//            Value_TMP.text = GetSelectionName();
//            if (func == null || parentId == null)
//            {
//                @object.transform.localPosition = new Vector3(3, 2f - 0.48f * i, 0);
//                i++;
//            }
//            else
//            {
//                int result = -1;
//                //parentIdを探す
//                foreach (var customoptions in CustomOptionsHolder.Options)
//                {
//                    foreach (var option in customoptions)
//                    {
//                        if (option.name == parentId)
//                        {
//                            result = func(option.selection) ? 1 : 0;
//                        }
//                    }
//                }
//                bool res = false;
//                if (result == -1)
//                {
//                    Logger.Error($"(parentId:{parentId}) is not included in Holder.Options");
//                }
//                else
//                {
//                    res = result == 1 ? true : false;
//                }

//                Logger.Info(name + ":" + selections[selection] + ", func:" + res, "Check");
//                if (res)
//                {
//                    @object.transform.localPosition = new Vector3(3, 2f - 0.5f * i, 0);
//                    i++;
//                }
//                else
//                {
//                    @object.transform.localPosition = new Vector3(80, 200, 0);
//                }
//            }
//        }

//        public static void ShareOptionSelections()
//        {
//            if (PlayerControl.AllPlayerControls.Count <= 1 || AmongUsClient.Instance?.AmHost == false && PlayerControl.LocalPlayer == null) return;

//            MessageWriter writer = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId, (byte)Rpcs.ShareOptions, Hazel.SendOption.Reliable);


//            writer.WritePacked((uint)CustomOptionsHolder.Options.Select(x => x.Count).Sum());
//            foreach (var options in CustomOptionsHolder.Options)
//            {
//                foreach (CustomOption option in options)
//                {
//                    writer.WritePacked((uint)Convert.ToUInt32(option.selection));
//                }
//            }
//            writer.EndMessage();
//        }
//        public static void GetOptionSelections(MessageReader reader)
//        {
//            if (reader == null) return;

//            uint count = reader.ReadPackedUInt32();
//            foreach (var options in CustomOptionsHolder.Options)
//            {
//                foreach (CustomOption option in options)
//                {
//                    uint i = reader.ReadPackedUInt32();
//                    option.entry.Value = (int)i;
//                }
//            }
//            CustomOptionsHolder.AllCheck();
//        }
//    }

//    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSyncSettings))]
//    public class RpcSyncSettingsPatch
//    {
//        public static void Postfix()
//        {
//            CustomOption.ShareOptionSelections();
//        }
//    }
//    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Close))]
//    public class GameSettingMenuClosePatch
//    {
//        public static void Postfix()
//        {
//            HudManager.Instance.transform.FindChild("CustomSettings").FindChild("CustomRoleSettings").gameObject.active = false;
//            HudManager.Instance.transform.FindChild("CustomSettings").FindChild("TSRSettings").gameObject.active = false;
//        }
//    }
//}
