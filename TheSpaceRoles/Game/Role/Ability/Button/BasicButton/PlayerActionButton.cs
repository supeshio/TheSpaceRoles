using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace TSR.Game.Role.Ability.Button.BasicButton
{
    public abstract class PlayerActionButton : TimerButton
    {
        public FPlayerControl currentTarget;
        protected float cooldownSecondsRemaining;
        protected int usesRemaining = 1;
        protected static List<FPlayerControl> GetTarget() => FGameManager.Manager.Local.Target?.currentTarget ?? [];
        //public override void DoClick()
        //{
        //    base.AbilityButton.DoClick();
        //}
        private void Start()
        {
            this.ActionButton.isCoolingDown = true;
            this.ActionButton.graphic.SetCooldownNormalizedUvs();
        }
        public override void Update()
        {

            if (GetTarget().Count > 0)
            {
                this.ActionButton.SetEnabled();
            }
            else
            {
                this.ActionButton.SetDisabled();
            }
            base.Update();
        }
    }
}
