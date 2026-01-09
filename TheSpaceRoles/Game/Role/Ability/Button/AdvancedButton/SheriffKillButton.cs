using Il2CppSystem.Runtime.CompilerServices;
using Sentry.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSR.Game.Options;
using TSR.Game.Role.Ability.Button.BasicButton;
using static TSR.Game.Role.Ability.IAbilityBase;
using UnityEngine;
using UnityEngine.Events;

namespace TSR.Game.Role.Ability.Button.AdvancedButton
{
    public class SheriffKillButton : PlayerActionButton
    {
        //private string Name;
        //private Sprite Icon;
        //private int RemainKill = -1;

        /*private void UseButton() {
            var target = CustomButton.SetPlayerTarget(() => FPlayerControl.AllPlayer.Where(x => x.Team != TeamId.Crewmate).ToList(), 0.5f);
        }*/

        //private bool IsButtonUsable() => CustomButton.SetPlayerTarget(() => FPlayerControl.AllPlayer.Where(x => x.Team != TeamId.Crewmate).ToList(), 0.5f) != null;
        public readonly List<string> NotTargetTeam = ["tsr:crewmate"];
        public override void Init()
        {
            Logger.Info("CREATED");
            //remain 10
            RemainUse = 10;
            PassiveButton.OnClick = new();
            PassiveButton.OnClick.AddListener((UnityAction)(()=>DoClick()));
            PassiveButton.OnClick.AddListener((UnityAction)(() => Logger.Info("clicked",ButtonText())));
        }
        public void DoClick()
        {
            if (!CanInteract()) return;
            if (GetTarget().Count == 0) return;
            Logger.Info("CanInteract", ButtonText());

            foreach (var target in GetTarget())
            {
                var team = target.Team;
                if(NotTargetTeam.Contains(team))
                {
                    FPlayerControl.LocalPlayer.RpcMurder(FPlayerControl.LocalPlayer,true);
                }
                else
                {
                    FPlayerControl.LocalPlayer.RpcMurder(target, true);
                }
            }
            Use();
        }
        public override Sprite ButtonIcon() => HudManager.Instance.KillButton.defaultKillSprite;
        public override string ButtonName() => "SheriffKillButton";
        public override string ButtonText() => "Kill";

        [CustomOptionFloat("tsr:sheriff","tsr:sheriff.kill-button.max-cooldown", 30f,0,255f,0.25f,"option.id.second")]
        public float MaxCoolDown { get; set; }
        //

        [CustomOptionInt("tsr:sheriff","tsr:sheriff.kill-button.count", 15,0,255,1,"option.id.times")]
        public float Count { get; set; }
        //
        
        public override float GetMaxCoolDown()
        {
            return MaxCoolDown;
        }

        //private Func<bool> IsButtonUsable;

    }
    
}
