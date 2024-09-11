using HarmonyLib;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheSpaceRoles
{

    public enum ButtonPos
    {
        Use = 0,
        Report,
        Sabotage,
        Kill,
        Vent,
        Custom,

    }
    public class CustomButton : ActionButton
    {
        /// <summary>
        /// 新役職作るとき入れるとこ
        /// CustomButtonを入れる
        /// </summary>
        public static List<CustomButton> buttons = new() { };


        public static Vector2 SelectButtonPos(int c) => c switch
        {
            0 => new Vector2(0, 0),
            1 => new Vector2(-1, 0),
            2 => new Vector2(-2, 0),
            3 => new Vector2(0, 1),
            4 => new Vector2(-1, 1),
            5 => new Vector2(-2, 1),
            6 => new Vector2(-8, 0),
            _ => new Vector2(-3, 3),
        };

        public string Name;
        public ActionButton actionButton;
        public HudManager hudManager;
        public ButtonPos ButtonPosition;
        public float maxTimer;
        public float Timer;

        public bool isEffectActive = false;

        public bool hasEffect;
        public bool canEffectCancel;
        public float maxEffectTimer;
        public Func<int> CanUse;
        public string buttonText = "";
        public bool atFirsttime;
        public TextMeshPro AdditionalText;

        public Sprite sprite;
        public KeyCode keyCode;
        public Action OnClick;
        public Action OnMeetingEnds;
        public Action OnEffectStart;
        public Action OnEffectUpdate;
        public Action OnEffectEnd;
        private static readonly int Desat = Shader.PropertyToID("_Desat");


        public bool IsDead = false;
        public CustomButton(
            HudManager hudManager,
            string name,
            ButtonPos buttonPos,
            KeyCode keycode,
            float maxTimer,
            Func<int> canUse,
            Sprite sprite,
            Action Onclick,
            Action OnMeetingEnds,
            string buttonText,
            bool HasEffect,
            bool canEffectCancel = false,
            float EffectDuration = 0,
            Action OnEffectStart = null,
            Action OnEffectUpdate = null,
            Action OnEffectEnd = null
            )
        {
            this.Name = name;
            this.ButtonPosition = buttonPos;
            this.hudManager = hudManager;
            this.keyCode = keycode;
            this.OnClick = Onclick;
            this.OnMeetingEnds = OnMeetingEnds;
            this.CanUse = canUse;
            this.sprite = sprite;
            this.buttonText = buttonText;
            this.hasEffect = HasEffect;
            this.maxTimer = maxTimer;
            Timer = 10f;
            this.canEffectCancel = canEffectCancel;
            this.maxEffectTimer = EffectDuration;
            this.atFirsttime = true;
            this.OnEffectStart = OnEffectStart;
            this.OnEffectUpdate = OnEffectUpdate;
            this.OnEffectEnd = OnEffectEnd;
            actionButton = Instantiate(hudManager.KillButton, hudManager.KillButton.transform.parent);
            actionButton.buttonLabelText.text = buttonText;
            actionButton.graphic.sprite = sprite;
            actionButton.cooldownTimerText.text = ((int)Timer).ToString();
            actionButton.gameObject.name = name;
            this.actionButton.transform.SetSiblingIndex((int)ButtonPosition);
            PassiveButton passiveButton = actionButton.GetComponent<PassiveButton>();
            passiveButton.enabled = true;

            passiveButton.OnClick.AddListener((UnityEngine.Events.UnityAction)Click);
            SetActive(true);
            try
            {
                Logger.Info(buttonText + " button Is created");

            }
            catch (Exception ex) { Logger.Error(ex.ToString()); }



            var gameob = new GameObject("Text");
            gameob.transform.SetParent(actionButton.transform);
            gameob.SetActive(true);
            AdditionalText = gameob.AddComponent<TextMeshPro>();
            AdditionalText.color = Color.white;
            AdditionalText.fontSizeMin = AdditionalText.fontSize = AdditionalText.fontSizeMax = 1.2f;
            AdditionalText.alignment = TextAlignmentOptions.Center;
            AdditionalText.outlineWidth = 0.8f;
            AdditionalText.enableWordWrapping = false;
            AdditionalText.autoSizeTextContainer = true;
            AdditionalText.transform.localPosition = new Vector3(0f, 0.6f, -1f);
            AdditionalText.gameObject.layer = HudManager.Instance.gameObject.layer;
            AdditionalText.m_sharedMaterial = Data.textMaterial;
            AdditionalText.text = "";





            DataBase.buttons.Add(this);
        }
        public void HudUpdate()
        {
            if (Input.GetKeyDown(this.keyCode))
            {
                Click();
            }

            PlayerControl local = PlayerControl.LocalPlayer;

            if (Timer >= 0)
            {
                if (this.hasEffect && this.isEffectActive)
                {
                    Timer -= Time.deltaTime;
                }
                else if (local.moveable)
                {
                    Timer -= Time.deltaTime;

                }
            }
            if (Timer <= 0)
            {
                atFirsttime = true;
                Timer = 0;
                if (hasEffect && isEffectActive)
                {
                    Timer = maxTimer;
                    isEffectActive = false;
                    actionButton.cooldownTimerText.color = Palette.EnabledColor;

                    this.OnEffectEnd?.Invoke();
                }

            }
            if (Timer <= 0) this.actionButton.cooldownTimerText.text = "";
            var canuse = CanUse();
            if (!isEffectActive)
            {

                if (canuse != -1)
                {
                    actionButton.graphic.color = actionButton.buttonLabelText.color = Palette.EnabledColor;
                    actionButton.graphic.material.SetFloat(Desat, 0f);
                }
                else
                {

                    actionButton.graphic.color = actionButton.buttonLabelText.color = Palette.DisabledClear;
                    actionButton.graphic.material.SetFloat(Desat, 1f);
                }

            }
            else
            {

                this.OnEffectUpdate?.Invoke();
            }


            if (atFirsttime == false)
            {
                actionButton.SetCoolDown(Timer, (hasEffect && isEffectActive) ? maxEffectTimer : maxTimer);

            }
            else
            {
                actionButton.SetCoolDown(Timer, 10f);

            }

        }
        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                actionButton.gameObject.SetActive(true);
                actionButton.graphic.enabled = true;
            }
            else
            {
                actionButton.gameObject.SetActive(false);
                actionButton.graphic.enabled = false;
            }
        }
        public void Death()
        {
            IsDead = true;
            SetActive(false);
        }
        public void Click()
        {

            if (Timer <= 0 && !isEffectActive)
            {
                if (hasEffect)
                {

                    if (CanUse() == -1) return;
                    isEffectActive = true;
                    Timer = maxEffectTimer;
                    OnClick();
                    actionButton.graphic.color = actionButton.buttonLabelText.color = Palette.AcceptedGreen;
                    actionButton.cooldownTimerText.color = Palette.AcceptedGreen;
                    actionButton.graphic.material.SetFloat(Desat, 0f);
                }
                else
                {

                    if (CanUse() == -1) return;
                    OnClick();
                    Timer = maxTimer;
                }
            }


        }
        public void MeetingStarts()
        {
            SetActive(false);
        }
        public void MeetingEnds()
        {
            //if()表示してもいいかどうかしらべなきゃいけない
            if (IsDead) return;
            SetActive(true);
            atFirsttime = true;

            OnMeetingEnds?.Invoke();
        }
    }
    [HarmonyPatch]
    public static class CustomButtonstatic
    {

        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
        public static class HudUpdate
        {
            public static void Prefix(HudManager __instance)
            {
                if (DataBase.buttons.Count == 0) return;
                DataBase.buttons.Do(x => x.HudUpdate());
            }
        }
        [HarmonyPatch]
        public static class MeetingHudPatch
        {

            [HarmonyPatch(typeof(MeetingIntroAnimation))]
            [HarmonyPatch(nameof(MeetingIntroAnimation.Start))]
            public static void Prefix()
            {

                if (DataBase.buttons.Count == 0) return;
                DataBase.buttons.Do(x => x.MeetingStarts());

            }

        }

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        public static class WrapUpPatch
        {
            public static void Prefix()
            {

                if (DataBase.buttons.Count == 0) return;
                DataBase.buttons.Do(x => x.MeetingEnds());
            }
        }
        [HarmonyPatch]
        public static class ActionButtonPatch
        {

        }
    }



}
