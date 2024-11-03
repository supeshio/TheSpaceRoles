using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public int count;

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
            Action OnEffectEnd = null,
            int count = -1
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
            this.count = count;
            actionButton = Instantiate(hudManager.AbilityButton, hudManager.KillButton.transform.parent);
            if (count > 0) 
            {

                actionButton.SetUsesRemaining(count);
            }
            else
            {
                actionButton.SetInfiniteUses();
            }
            actionButton.buttonLabelText.text = buttonText;
            actionButton.graphic.sprite = sprite;
            actionButton.cooldownTimerText.text = ((int)Timer).ToString();
            actionButton.gameObject.name = name;
            actionButton.transform.SetSiblingIndex((int)ButtonPosition);
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

            if (Timer > 0&count != 0)
            {
                    if (this.hasEffect && this.isEffectActive)
                {
                    Timer -= Time.deltaTime;
                }
                else if (local.moveable)
                {
                    Timer -= Time.deltaTime;

                }
            }else
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

            if (count>0)
            {
                count --;
                actionButton.SetUsesRemaining(count);
            }

            if (count != 0)
            {

                if (atFirsttime == false)
                {
                    actionButton.SetCoolDown(Timer, (hasEffect && isEffectActive) ? maxEffectTimer : maxTimer);

                }
                else
                {
                    actionButton.SetCoolDown(Timer, 10f);

                }
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

            if (count != 0)
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

        /// <summary>
        /// ターゲットに対しての距離を測って一番近いplayerのidを出力
        /// </summary>
        /// <param name="targetdistance">ターゲットの許容距離</param>
        /// <param name="notIncludeTeamIds">含まないチーム</param>
        /// <param name="notIncludeIds">含まないPlayerID</param>
        /// <param name="target">誰基準か</param>
        /// <param name="canBeTargetInVentPlayer">ベント内のプレイヤーを含むか</param>
        /// <returns>一番近いplayerのid  もし-1が帰ってきたらターゲットいないです</returns>
        public static int SetTarget(float targetdistance = 1f, Color? color = null, Teams[] notIncludeTeamIds = null, int[] notIncludeIds = null, int target = -1, bool canBeTargetInVentPlayer = false)
        {
            DataBase.AllPlayerControls().Do(x => x.cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 0f));
            if (color != null)
            {
                DataBase.AllPlayerControls().Do(x => x.cosmetics.currentBodySprite.BodySprite.material.SetColor("_OutlineColor", (UnityEngine.Color)color));

            }
            int id = -1;
            float distance = float.MaxValue;

            if (target == -1)
            {
                target = PlayerControl.LocalPlayer.PlayerId;
            }
            foreach (var x in PlayerControl.AllPlayerControls)
            {
                if (notIncludeIds != null && notIncludeIds.Length > 0)
                {
                    if (notIncludeIds.Contains(x.PlayerId))
                    {
                        continue;
                    }
                }
                if (notIncludeTeamIds != null && notIncludeTeamIds.Length > 0)
                {
                        if (notIncludeTeamIds.Contains(DataBase.AllPlayerRoles[x.PlayerId].team))
                        {
                            continue;
                        }

                }
                if (x.inVent)
                {
                    if (!canBeTargetInVentPlayer) { continue; }
                }
                if (x.Data.IsDead)
                {
                    continue;
                }
                if (target == x.PlayerId) continue;
                PlayerControl p = Helper.GetPlayerControlFromId(target);
                Vector2 truePosition = x.GetTruePosition();
                Vector2 vector = new Vector2(p.transform.position.x, p.transform.position.y) - truePosition;
                float magnitude = vector.magnitude;
                if (magnitude <= distance && !PhysicsHelpers.AnyNonTriggersBetween(truePosition, vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                {
                    id = x.PlayerId;
                    distance = magnitude;
                }

            }
            if (targetdistance >= distance)
            {
                Helper.GetPlayerControlFromId(id).cosmetics.currentBodySprite.BodySprite.material.SetFloat("_Outline", 1f);
                return id;


            }
            return -1;



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
