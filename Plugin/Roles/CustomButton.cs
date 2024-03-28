using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheSpaceRoles
{

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


        public ActionButton actionButton;
        public HudManager hudManager;
        public float maxTimer;
        public float Timer;

        public bool isEffectActive = false;

        public bool hasEffect;
        public bool canEffectCancel;
        public float maxEffectTimer;
        public Func<int> CanUse;
        public string buttonText = "";
        public bool atFirsttime;

        public Sprite sprite;
        public KeyCode keyCode;
        public Action OnClick;
        public Action OnMeetingEnds;
        public Action OnEffectEnds;
        private static readonly int Desat = Shader.PropertyToID("_Desat");


        public bool IsDead = false;
        public CustomButton(
            HudManager hudManager,
            Vector2 pos,
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
            Action OnEffectStarts = null,
            Action OnEffectEnds = null
            )
        {
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
            actionButton = Instantiate(hudManager.KillButton, hudManager.KillButton.transform.parent);
            actionButton.buttonLabelText.text = buttonText;
            actionButton.graphic.sprite = sprite;
            actionButton.transform.position.Set(pos.x, pos.y, -9);
            actionButton.cooldownTimerText.text = ((int)Timer).ToString();
            PassiveButton passiveButton = actionButton.GetComponent<PassiveButton>();
            passiveButton.enabled = true;

            passiveButton.OnClick.AddListener((UnityEngine.Events.UnityAction)Click);
            SetActive(true);
            try
            {
                Logger.Info(buttonText + " button Is created");

            }
            catch (Exception ex) { Logger.Error(ex.ToString()); }


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
                    isEffectActive = false;
                    Timer = maxTimer;
                    actionButton.cooldownTimerText.color = Palette.EnabledColor;
                    OnEffectEnds();
                }

            }
            if (Timer <= 0) this.actionButton.cooldownTimerText.text = "";
            var canuse = CanUse();
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
            OnMeetingEnds();
        }
    }
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


}
