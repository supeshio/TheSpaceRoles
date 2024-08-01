using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class Sheriff : CustomRole
    {
        public static CustomButton SheriffKillButton;
        public Sheriff()
        {

            teamsSupported = [Teams.Crewmate];
            Role = Roles.Sheriff;
            Color = ColorFromColorcode("#ffd700");
            HasKillButton = true;
        }
        public override void HudManagerStart(HudManager __instance)
        {
            if (DataBase.AllPlayerRoles.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var r))
            {

            }
            SheriffKillButton = new CustomButton(
                __instance,
                CustomButton.SelectButtonPos(0),
                KeyCode.Q,
                30,
                () => KillButtons.KillButtonSetTarget(2.5f, Color),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    var pc = GetPlayerControlFromId(KillButtons.KillButtonSetTarget(2.5f, Color));

                    if (DataBase.AllPlayerTeams[pc.PlayerId] != Teams.Crewmate)
                    {
                        RpcMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, pc, DeathReason.SheriffKill);


                    }
                    else
                    {
                        RpcMurderPlayer.RpcMurder(pc, PlayerControl.LocalPlayer, DeathReason.SheriffSuicide);
                    }
                },
                () => SheriffKillButton.Timer = SheriffKillButton.maxTimer,
                "Kill",
                false);

        }
    }
}
