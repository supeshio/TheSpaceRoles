using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TSR.Game.Role.Ability.Button.BasicButton
{
    public abstract class TimerButton : CustomButton
    {
        //public TimerButton(string Name, float MaxCoolDown,Sprite Icon,Func<bool> IsButtonUsable,Action UseButton, int RemainUse = -1, Func<string> TitleText = null)
        //{
        //    this.Name = Name;
        //    this.MaxCoolDown = MaxCoolDown;
        //    this.RemainUse = RemainUse;
        //    this.Icon = Icon;
        //    this.UseButton = UseButton;
        //    this.IsButtonUsable = IsButtonUsable;
        //    this.TitleText = TitleText;
        //}

        public override void Update()
        {
            if (!this.Player.IsNull || !this.Player.AmOwner || MeetingHud.Instance)
            {
                return;
            }
            if (this.Timer > 0f)
            {
                this.Timer -= Time.deltaTime;
                DestroyableSingleton<HudManager>.Instance.AbilityButton.SetCoolDown(this.Timer, this.GetMaxCoolDown());
            }
            else if (this.Timer <= 0)
            {
                CoolDownZero();
            }
            base.Update();
        }
        protected virtual bool CanInteract()
        {
            return this.Timer <= 0 & RemainUse!=0 ;
        }
        protected void CoolDownZero() { }
        public void Use()
        {
            RemainUse -= 1;
            ResetTimer();
            ActionButton.ResetCoolDown();
        }
        public void ResetTimer()
        {
            Timer = GetMaxCoolDown();
            SetRemainUse();
        }
        public void SetRemainUse()
        {
            ActionButton.SetUsesRemaining(RemainUse);
        }
        public void Delete()
        {
            GameObject.Destroy(this.ActionButton.gameObject);
        }
        // Token: 0x0600143C RID: 5180 RVA: 0x00051CF0 File Offset: 0x0004FEF0
        public void ChangeButtonText(string Text)
        {
            this.ActionButton.buttonLabelText.text = Text;
        }

        // Token: 0x0600143D RID: 5181 RVA: 0x00051D09 File Offset: 0x0004FF09
        public void ChangeGraphic(Sprite newSprite)
        {
            this.ActionButton.graphic.sprite = newSprite;
            this.ActionButton.graphic.SetCooldownNormalizedUvs();
        }
    }
}
