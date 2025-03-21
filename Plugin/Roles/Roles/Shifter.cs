using System;
using System.Collections.Generic;
using UnityEngine;
using static TheSpaceRoles.Helper;
namespace TheSpaceRoles
{
    public class Shifter : CustomRole
    {
        public PlayerControl target;
        public CustomButton ShiftButton;
        public Shifter()
        {
            Team = Teams.Crewmate;
            Role = Roles.Shifter;
            Color = Helper.ColorFromColorcode("#666666");
        }
        public override void HudManagerStart(HudManager __instance)
        {
            ShiftButton = new CustomButton(
                __instance, "ShifterShiftButton"
                , this,
                ButtonPos.Custom,
                KeyCode.F,
                30,
                () => CustomButton.SetTarget(),
                Sprites.GetSpriteFromResources("ui.button.evilhacker_hack.png", 100f),
                () =>
                {
                    var pc = GetPlayerById(CustomButton.SetTarget());
                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 0);
                    writer.Write(pc);
                    writer.EndRpc();
                    ShiftRole(pc.PlayerId);
                },
                () =>
                {
                    ShiftButton.Timer = ShiftButton.maxTimer;
                },
                "Shift",
                false,remainUses:1);
            Logger.Info("button:Shifter Shifting");

        }
        public void ShiftRole(int targetId)
        {
            target = GetPlayerById(targetId);
        }
        public override void AfterMeetingEnd()
        {
            if (target.GetCustomRole().Team == Teams.Crewmate)
            {
                RoleSelect.ChangeMainRole(PlayerId,(int)target.GetCustomRole().Role);
                RoleSelect.ChangeMainRole(target.PlayerId,(int)Roles.Crewmate);
                
            }
            
        }
    }
}
