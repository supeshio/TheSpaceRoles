using System.Linq;
using UnityEngine;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.CustomOptionsHolder;
using static TheSpaceRoles.Helper;

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
        public static CustomOption KillDistance;
        public static CustomOption UseGarlic;
        public static CustomOption GarlicAreaSize;
        public override void OptionCreate()
        {
            if (KillDelayTime != null) return;

            Logger.Info(string.Join(",", GetAreasize(5, 1, false).Select(x => x()).ToList()), "GetAreaSize_Vampire");
            KillDelayTime = CustomOption.Create(CustomOption.OptionType.Impostor, "role.vampire.killdelaytime", CustomOptionsHolder.GetSeconds(20, 1, false), 9);
            KillDistance = Create(CustomOption.OptionType.Impostor, "role.vampire.killdistance", GetKillDistances(), 4);
            KillCoolDown = Create(CustomOption.OptionType.Impostor, "role.vampire.killcooldown", CustomOptionsHolder.GetSeconds(), 12);
            UseGarlic = Create(CustomOption.OptionType.Impostor, "role.vampire.usegarlic", true);
            GarlicAreaSize = Create(CustomOption.OptionType.Impostor, "role.vampire.garlicareasize", GetAreasize(10, 1, false), 4, Show: UseGarlic.GetBool);

            //キル遅延時間、キルク、ニンニク内だと特殊キルができなくなりその上でさらに通常キルもできなくなるかの設定
            Options = [KillDelayTime, KillCoolDown, GarlicAreaSize, KillDistance];
        }
        public override void HudManagerStart(HudManager __instance)
        {
            DataBase.AllPlayerRoles.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var r);
            VampireBitebutton = new CustomButton(
                __instance, "VampireKillButton",
                ButtonPos.Kill,
                KeyCode.Q,
                KillCoolDown.GetKillCoolDownOption(),
                () => KillButtons.KillButtonSetTarget(KillDistance.GetKillDistance(), Color, [Teams.Impostor]),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    BittenPlayerControl = GetPlayerControlFromId(KillButtons.KillButtonSetTarget(KillDistance.GetKillDistance(), Color, [Teams.Impostor]));
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
