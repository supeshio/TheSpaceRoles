using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class TemplateRole : CustomRole
    {
        public TemplateRole()
        {
            Team = Teams.Crewmate;
            Role = Roles.Crewmate;
            Color = Palette.CrewmateBlue;
        }
        CustomButton TemplateButton;
        public override void HudManagerStart(HudManager __instance)
        {

            TemplateButton = new CustomButton(
                hudManager: __instance,
                name: "TemplateButton", this,
                buttonPos: ButtonPos.Custom,
                keycode: KeyCode.F,
                maxTimer: 30,
                canUse: () => CustomButton.SetTarget()/*-1以外なら成功判定*/,
                sprite: Sprites.GetSpriteFromResources("ui.button.template.png", 100f),
                Onclick: () =>
                {
                    var pc = GetPlayerById(CustomButton.SetTarget());
                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 0);
                    writer.Write(pc);
                    writer.EndRpc();
                },
                OnMeetingEnds: () =>
                {
                    TemplateButton.Timer = TemplateButton.maxTimer;
                },
                buttonText: "Shift",
                HasEffect: false,
                canEffectCancel: false,
                EffectDuration: 30,
                OnEffectStart: () => { },
                OnEffectUpdate: () => { },
                OnEffectEnd: () => { },
                remainUses: -1);

        }
        public static CustomOption Option1;
        public override void OptionCreate()
        {
            Option1 = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.template.temp", true);

            Options = [Option1];
        }

    }
}
