using HarmonyLib;
using System;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{
    public class SettingPlugin : MonoBehaviour
    {
    }
    [HarmonyPatch]
    public static class CustomSetting
    {
        public static GameObject SettingBG;

        public static PassiveButton BGCol;
        public abstract class Setting
        {
            public string SettingName;
            public string SkipSettingName;
            public string ParentSettingName;
            public Action<bool> Showing;
            public TextMeshPro Text;
            public SpriteRenderer Arrow;
            public GameObject SettingObj;
            public Func<float> Ymove = () => 0;
            public Setting Create(string SettingName, string SkipSettingName, string ParentSettingName, Action<bool> Showing)
            {
                this.SettingName = SettingName;
                this.SkipSettingName = SkipSettingName;
                this.ParentSettingName = ParentSettingName;
                this.Showing = Showing;
                this.SettingObj = new GameObject(SettingName);

                this.Text = SettingObj.AddComponent<TextMeshPro>();
                this.Arrow = SettingObj.AddComponent<SpriteRenderer>();
                return this;
            }
        }

        public class SettingOption : Setting
        {
            public SettingOption(string SettingName, string SkipSettingName, string ParentSettingName, Action<bool> Showing)
            {
                this.SettingName = SettingName;
                this.SkipSettingName = SkipSettingName;
                this.ParentSettingName = ParentSettingName;
                this.Showing = Showing;
                this.SettingObj = new GameObject(SettingName);

                this.Text = SettingObj.AddComponent<TextMeshPro>();
                this.Arrow = SettingObj.AddComponent<SpriteRenderer>();


            }
        }

        public class SettingOptionHeader : Setting
        {
        }


        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start)), HarmonyPostfix]
        public static void Create(HudManager __instance)
        {
            SettingBG = new GameObject("SettingBG")
            {
                layer = Data.UILayer
            };
            SettingBG.transform.SetParent(__instance.transform);
            SettingBG.transform.position = new Vector3(0, 0, -500);
            var backsp = SettingBG.AddComponent<SpriteRenderer>();
            var back = DestroyableSingleton<GameStartManager>.Instance.GameSizePopup.GetComponent<SpriteRenderer>().sprite;
            //backsp.sprite = Sprites.GetSpriteFromResources("ui.MainPanel01.png");
            backsp.transform.localScale = new Vector3(1f, 1f, 1.0f);
            backsp.sprite = back;
            backsp.drawMode = SpriteDrawMode.Sliced;
            //backsp.sprite = new Vector4(10, 10, 10, 10);
            //backsp.sprite.border.Set(100, 100, 100, 100);
            backsp.size = new Vector2(9f, 5f);
            backsp.gameObject.AddComponent<BoxCollider2D>().size = backsp.size;
            var backspbutton = backsp.gameObject.AddComponent<PassiveButton>();
            backspbutton.Colliders = new[] { backsp.gameObject.GetComponent<BoxCollider2D>() };
            //backsp.color = Helper.ColorFromColorcode("#2f2f2fcf");
            SettingBG.SetActive(false);
            backspbutton.enabled = false;
            BGCol = backspbutton;

            //TAB
            // - RoleOption,GameOption,
        }
        enum OptionTabs
        {
            RoleOption,
            GameOption
        }

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        [HarmonyPostfix]
        public static void Update(HudManager __instance)
        {


            if (Input.GetKeyDown(KeyCode.H))
            {
                Logger.Message("H");
                if (active)
                {
                    Hide();
                }
                else
                {

                    Show();
                }
            }

        }
        public static bool active = false;
        public static void Show()
        {
            active = true;
            SettingBG.SetActive(true);
            BGCol.enabled = true;
        }
        public static void Hide()
        {
            active = false;
            SettingBG.SetActive(false);
            BGCol.enabled = false;
        }

    }
}
