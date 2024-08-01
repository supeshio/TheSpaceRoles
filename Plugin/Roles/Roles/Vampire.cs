using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class Vampire : CustomRole
    {
        public static CustomButton VampireBitebutton;
        public PlayerControl BittenPlayerControl;
        public Vampire()
        {

            teamsSupported = [Teams.Impostor];
            Role = Roles.Vampire;
            Color = Palette.ImpostorRed;
            HasKillButton = false;
        }
        public override void HudManagerStart(HudManager __instance)
        {
            if (DataBase.AllPlayerRoles.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var r))
            {

            }
            VampireBitebutton = new CustomButton(
                __instance, "VampireKillButton",
                ButtonPos.Kill,
                KeyCode.Q,
                30,
                () => KillButtons.KillButtonSetTarget(2.5f, Color, [Teams.Impostor]),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    BittenPlayerControl = GetPlayerControlFromId(KillButtons.KillButtonSetTarget(2.5f, Color, [Teams.Impostor]));
                },
                () => VampireBitebutton.Timer = VampireBitebutton.maxTimer,
                "Kill",
                true, false, 10f, OnEffectEnd: () =>
                {
                    UnCheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, BittenPlayerControl, DeathReason.BittenByVampire, false);
                });

        }
    }
}
