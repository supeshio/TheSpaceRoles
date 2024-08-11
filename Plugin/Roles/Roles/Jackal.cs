using UnityEngine;
using static TheSpaceRoles.Helper;
namespace TheSpaceRoles
{
    public class Jackal : CustomRole
    {
        CustomButton JackalKillButton;
        public Jackal()
        {

            team = Teams.Jackal;
            Role = Roles.Jackal;
            Color = ColorFromColorcode("#00b4eb");
            CanUseVent = true;
            //HasKillButton = true;
        }
        public override void HudManagerStart(HudManager __instance)
        {
            if (DataBase.AllPlayerRoles.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var r))
            {

            }
            JackalKillButton = new CustomButton(
                __instance, "JackalKillButton",
                ButtonPos.Kill,
                KeyCode.Q,
                30,
                () => KillButtons.KillButtonSetTarget(2.5f, Color, notIncludeTeamIds: [Teams.Jackal]),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    var pc = GetPlayerControlFromId(KillButtons.KillButtonSetTarget(2.5f, Color, notIncludeTeamIds: [Teams.Jackal]));
                    CheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, pc, DeathReason.SheriffKill);

                },
                () => JackalKillButton.Timer = JackalKillButton.maxTimer,
                "Kill",
                false);

        }
    }
}

