using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class Sheriff : CustomRole
    {
        public static CustomButton SheriffKillButton;
        public Sheriff()
        {

            Team = Teams.Crewmate;
            Role = Roles.Sheriff;
            Color = ColorFromColorcode("#f8cd46");
            HasKillButton = false;
        }
        public override void HudManagerStart(HudManager __instance)
        {
            if (DataBase.AllPlayerData.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var r))
            {

            }
            SheriffKillButton = new CustomButton(
                __instance, "SheriffKillButton",
                ButtonPos.Kill,
                KeyCode.Q,
                30,
                () => KillButtons.KillButtonSetTarget(2.5f, Color),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    var pc = GetPlayerById(KillButtons.KillButtonSetTarget(2.5f, Color));

                    if (DataBase.AllPlayerData[pc.PlayerId].Team != Teams.Crewmate)
                    {
                        CheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, pc, DeathReason.SheriffKill);


                    }
                    else
                    {
                        UnCheckedMurderPlayer.RpcMurder(pc, PlayerControl.LocalPlayer, DeathReason.SheriffSuicide);
                    }
                },
                () => SheriffKillButton.Timer = SheriffKillButton.maxTimer,
                "Kill",
                false);

        }
    }
}
