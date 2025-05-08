using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{
    public static class CustomSetting
    {
        public static GameObject SettingBG;

        public abstract class Setting
        {
            public string SettingName;
            public string SkipSettingName;
            public string ParentSettingName;
            public Action<bool> Showing;
            public TextMeshPro Text;
            public SpriteRenderer Arrow;
            public GameObject SettingObj;

        }

        public class SettingOption : Setting
        {
            public SettingOption(string SettingName, string SkipSettingName, string ParentSettingName, Action<bool> Showing) {
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



        public static void Create()
        {
            SettingBG = new GameObject("SettingBG");
            SettingBG.layer = Data.UILayer;
            SettingBG.transform.position = new Vector3(0, 0, -500);
        }

        public static void Update()
        {

        }
        public static void Show()
        {

        }
        public static void Hide()
        {

        }
    }
}
