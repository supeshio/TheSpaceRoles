using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using TSR.Game;
using UnityEngine;
using static Rewired.ComponentControls.TouchButton;

namespace TSR.Game.Role.Ability
{
    public abstract class CustomButton : IAbilityBase
    {
        public ActionButton ActionButton;
        public PassiveButton PassiveButton;
        public abstract Sprite ButtonIcon();
        public FPlayerControl Player;
        public abstract string ButtonName();
        public abstract string ButtonText();
        public abstract float GetMaxCoolDown();
        //public ActionButton Button;
        /// <summary>
        /// 0からカウントアップ,MaxCoolDownに達すると使用可能
        /// Effect使用時はMaxEffectTimerに達するまで効果が続く
        /// </summary>
        public float Timer;
        //public float MaxEffectTimer = -1f;
        public int RemainUse;
        //public Func<List<FPlayerControl>> Targets;
        public Func<string> TitleText = null;
        public Action UseButton;
        public Func<bool> IsButtonUsable;
        public TextMeshPro ButtonTitle;
        public bool IsButtonCreated { get; private set; } = false;
        
        protected virtual bool IsValidTarget(FPlayerControl target)
        {
            return !(target == null) && !target.Disconnected && !target.IsDead && target.PlayerId != this.Player.PlayerId && !(target.Role == null) && !(target.NPI == null) && !target.PC.inVent && !target.PC.inMovingPlat && target.PC.Visible;
        }
        public void Set()
        {
            this.ActionButton.graphic.sprite = ButtonIcon();
            this.ActionButton.buttonLabelText.text = ButtonText();
            this.ActionButton.gameObject.name = ButtonName();
        }
        public void CreateButton()
        {
            Logger.Message("Creating Custom Button: " + ButtonName());
            var actionbutton = GameObject.Instantiate<ActionButton>(HudManager.Instance.AbilityButton, HudManager.Instance.AbilityButton.transform.parent);
            //GameObject.Destroy(actionbutton.GetComponent<AbilityButton>());
            GameObject.Destroy(actionbutton.buttonLabelText.GetComponent<TextTranslatorTMP>());
            this.ActionButton = actionbutton;
            this.PassiveButton = actionbutton.GetComponent<PassiveButton>();
            actionbutton.SetEnabled();
            actionbutton.gameObject.SetActive(true);
            this.IsButtonCreated = true;
            Set();
        }
        public virtual void Update()
        {

        }
        public static void GameStartResetButtons()
        {
            foreach (var cbutton in FPlayerControl.LocalPlayer.FRole.CustomButtons)
            {
                ((IAbilityBase)cbutton).Init();
            }
        }
        public static void ClearButtons()
        {
            if (FPlayerControl.LocalPlayer?.FRole?.CustomButtons == null) return;
            if(FPlayerControl.LocalPlayer.FRole.CustomButtons.Count == 0) return;
            foreach(var cbutton in FPlayerControl.LocalPlayer.FRole.CustomButtons)
            {
                if(cbutton.IsButtonCreated)
                {
                    GameObject.Destroy(cbutton.ActionButton.gameObject);
                }
            }

        }
        public abstract void Init();
    }
}
