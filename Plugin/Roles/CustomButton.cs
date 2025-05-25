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
        public static Dictionary<ButtonPos, ActionButton> pos = [];

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
        public int RemainCount;

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
        public CustomRole role;


        public bool IsDead = false;
        public CustomButton(
            HudManager hudManager,
            string name,
            CustomRole role,
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
            int remainUses = -1
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
            this.RemainCount = remainUses;
            this.role = role;
            actionButton = Instantiate(hudManager.AbilityButton, hudManager.KillButton.transform.parent);
            if (remainUses > 0)
            {

                actionButton.SetUsesRemaining(remainUses);
            }
            else
            {
                actionButton.SetInfiniteUses();
            }
            actionButton.buttonLabelText.text = buttonText;
            actionButton.graphic.sprite = sprite;
            actionButton.transform.SetAsLastSibling();
            //actionButton.buttonLabelText.outlineColor = role.Color;
            pos = [];
            pos.TryAdd(ButtonPos.Use, (ActionButton)HudManager.Instance.PetButton ?? HudManager.Instance.UseButton);
            pos.TryAdd(ButtonPos.Report, (ActionButton)HudManager.Instance.ReportButton);
            pos.TryAdd(ButtonPos.Sabotage, (ActionButton)HudManager.Instance.SabotageButton);
            pos.TryAdd(ButtonPos.Kill, (ActionButton)HudManager.Instance.KillButton);
            pos.TryAdd(ButtonPos.Vent, (ActionButton)HudManager.Instance.ImpostorVentButton);
            pos.TryAdd(ButtonPos.Custom, (ActionButton)HudManager.Instance.AbilityButton);

            actionButton.transform.SetSiblingIndex(pos[buttonPos].transform.GetSiblingIndex() + 1);
            actionButton.cooldownTimerText.text = ((int)Timer).ToString();

            actionButton.gameObject.name = name;
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
            AdditionalText.m_sharedMaterial = Data.NormalMaterial;
            AdditionalText.text = "";





            DataBase.Buttons.Add(this);
        }
        public void HudUpdate()
        {
            //1.ボタンの背景の色が変わらない問題
            if (Input.GetKeyDown(this.keyCode) && actionButton.gameObject.active)
            {
                Click();
            }

            PlayerControl local = PlayerControl.LocalPlayer;

            if (Timer > 0 & RemainCount != 0)
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
            else
            {
                atFirsttime = true;
                Timer = 0;
                if (hasEffect && isEffectActive)
                {
                    Timer = maxTimer;
                    isEffectActive = false;
                    actionButton.SetCooldownFill(1);
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
                    if (RemainCount != 0)
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

            if (RemainCount != 0)
            {
                if (Timer <= 0 && !isEffectActive)
                {
                    //成功
                    if (hasEffect)
                    {
                        //エフェクト処理

                        if (CanUse() == -1) return;
                        isEffectActive = true;
                        OnEffectStart?.Invoke();
                        Timer = maxEffectTimer;
                        OnClick?.Invoke();
                        actionButton.graphic.color = actionButton.buttonLabelText.color = Palette.AcceptedGreen;
                        actionButton.cooldownTimerText.color = Palette.AcceptedGreen;
                        actionButton.graphic.material.SetFloat(Desat, 0f);
                    }
                    else
                    {

                        if (CanUse() == -1) return;
                        OnClick?.Invoke();
                        Timer = maxTimer;
                    }
                    if (RemainCount > 0)
                    {
                        RemainCount--;
                        actionButton.SetUsesRemaining(RemainCount);
                    }
                }
            }
            actionButton.SetFillUp(Timer, maxTimer);


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
        /// <param name="notIncludeTeams">含まないチーム</param>
        /// <param name="notIncludeIds">含まないPlayerID</param>
        /// <param name="target">誰基準か</param>
        /// <param name="canBeTargetInVentPlayer">ベント内のプレイヤーを含むか</param>
        /// <returns>一番近いplayerのid  もし-1が帰ってきたらターゲットいないです</returns>
        public static int SetTarget(float targetdistance = 1f, Color? color = null, Teams[] notIncludeTeams = null, int[] notIncludeIds = null, PlayerControl target = null, bool canBeTargetInVentPlayer = false)
        {
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                var sprite = pc?.cosmetics?.currentBodySprite?.BodySprite;
                if (sprite?.material != null)
                {
                    sprite.material.SetFloat("_Outline", 0f);
                    if (color != null)
                    {
                        sprite.material.SetColor("_OutlineColor", (Color)color);
                    }
                }
            }

            if (target == null)
            {
                target = PlayerControl.LocalPlayer;
                if (target == null)
                {
                    UnityEngine.Debug.LogError("SetTarget: LocalPlayer is null");
                    return -1;
                }
            }

            int id = -1;
            float distance = float.MaxValue;

            foreach (var x in PlayerControl.AllPlayerControls)
            {
                if (x == null || x == target || x.Data == null || x.Data.IsDead) continue;

                if (notIncludeIds?.Contains(x.PlayerId) == true) continue;

                var role = Helper.GetCustomRole(x.PlayerId);
                if (role == null) continue;

                if (notIncludeTeams?.Contains(role.Team) == true) continue;

                if (x.inVent && !canBeTargetInVentPlayer) continue;

                Vector2 vector = x.GetTruePosition() - target.GetTruePosition();
                float magnitude = vector.magnitude;

                if (magnitude <= distance &&
                    !PhysicsHelpers.AnyNonTriggersBetween(target.GetTruePosition(), vector.normalized, magnitude, Constants.ShipAndObjectsMask))
                {
                    id = x.PlayerId;
                    distance = magnitude;
                }
            }

            if (id != -1 && distance <= targetdistance)
            {
                var targetPlayer = Helper.GetPlayerById(id);
                var sprite = targetPlayer?.cosmetics?.currentBodySprite?.BodySprite;
                if (sprite?.material != null)
                {
                    sprite.material.SetFloat("_Outline", 1f);
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"SetTarget: Unable to highlight PlayerId {id} (sprite/material missing)");
                }
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
            public static void Postfix()
            {
                if (DataBase.Buttons.Count == 0) return;
                DataBase.Buttons.Do(x => x.HudUpdate());
            }
        }
        [HarmonyPatch]
        public static class MeetingHudPatch
        {

            [HarmonyPatch(typeof(MeetingIntroAnimation))]
            [HarmonyPatch(nameof(MeetingIntroAnimation.Start))]
            public static void Prefix()
            {

                if (DataBase.Buttons.Count == 0) return;
                DataBase.Buttons.Do(x => x.MeetingStarts());

            }

        }

        [HarmonyPatch(typeof(ExileController), nameof(ExileController.WrapUp))]
        public static class WrapUpPatch
        {
            public static void Prefix()
            {

                if (DataBase.Buttons.Count == 0) return;
                DataBase.Buttons.Do(x => x.MeetingEnds());
            }
        }
        [HarmonyPatch]
        public static class ActionButtonPatch
        {

        }
    }



}
