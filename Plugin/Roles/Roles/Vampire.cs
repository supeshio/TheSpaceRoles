using UnityEngine;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.CustomOptionsHolder;
using static TheSpaceRoles.CustomOption;

namespace TheSpaceRoles
{
    public class Vampire : CustomRole
    {
        public static CustomButton VampireBitebutton;
        public PlayerControl BittenPlayerControl;
        public Vampire()
        {

            team = Teams.Impostor;
            Role = Roles.Vampire;
            Color = Palette.ImpostorRed;
            HasKillButton = false;
        }
        public static CustomOption KillDelayTime;
        public static CustomOption KillCoolDown;
        public static CustomOption GarlicAreaSize;
        public override void OptionCreate(HudManager hudManager)
        {
            if(KillDelayTime != null)return;

            KillDelayTime = CustomOption.Create(CustomOption.OptionType.Impostor, "role_vampire_killdelaytime",GetSeconds(20,1,false),10);
            KillCoolDown = Create(CustomOption.OptionType.Impostor, "role_vampire_killcooldown");
            GarlicAreaSize = Create(CustomOption.OptionType.Impostor, "role_vampire_garlicareasize",);
            //キル遅延時間、キルク、ニンニク内だと特殊キルができなくなりその上でさらに通常キルもできなくなるかの設定
            Options.Add();
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
