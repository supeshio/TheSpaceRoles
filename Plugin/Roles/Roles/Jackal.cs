using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;
namespace TheSpaceRoles
{
    public class Jackal : CustomRole
    {
        CustomButton JackalKillButton;
        CustomButton JackalSidekickButton;
        public Jackal()
        {

            Team = Teams.Jackal;
            Role = Roles.Jackal;
            Color = ColorFromColorcode("#00b4eb");
            CanUseVent = true;
            //HasKillButton = true;
        }
        CustomOption KillCoolDown;
        CustomOption SidekickCoolDown;
        public override void OptionCreate()
        {
            KillCoolDown = CustomOption.Create(CustomOption.OptionType.Neutral, "role.jackal.killcooldown", CustomCoolDownRangefloat(), 12);
            SidekickCoolDown = CustomOption.Create(CustomOption.OptionType.Neutral, "role.jackal.sidekickcooldown", CustomCoolDownRangefloat(), 12);
            Options = [KillCoolDown, SidekickCoolDown];
        }
        public override void HudManagerStart(HudManager __instance)
        {
            if (DataBase.AllPlayerData.TryGetValue(PlayerControl.LocalPlayer.PlayerId, out var r))
            {

            }
            KillCoolDown = CustomOptionsHolder.roleFamilarOptions[Roles.Jackal].First(x => x.nameId == "role.jackal.killcooldown");
            Logger.Info(KillCoolDown.GetHashCode().ToString());
            JackalKillButton = new CustomButton(
                __instance, "JackalKillButton", this,
                ButtonPos.Kill,
                KeyCode.Q,
                KillCoolDown.GetFloatValue(),
                () => KillButtons.KillButtonSetTarget(OSAS.killdistance_short.GetValueFromSelector(), Color, notIncludeTeamIds: [Teams.Jackal]),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    var pc = GetPlayerById(KillButtons.KillButtonSetTarget(OSAS.killdistance_short.GetValueFromSelector(), Color, notIncludeTeamIds: [Teams.Jackal]));
                    CheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, pc, DeathReason.SheriffKill);

                },
                () => JackalKillButton.Timer = JackalKillButton.maxTimer,
                "Kill",
                false);

            JackalSidekickButton = new CustomButton(
                __instance, "JackalSidekickButton"
                , this,
                ButtonPos.Custom,
                KeyCode.F,
                SidekickCoolDown.GetFloatValue(),
                () => CustomButton.SetTarget(notIncludeTeams: [Teams.Jackal]),
                Sprites.GetSpriteFromResources("ui.button.jackal_sidekick.png", 100),
                () =>
                {
                    var pc = GetPlayerById(CustomButton.SetTarget(notIncludeTeams: [Teams.Jackal]));
                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 0);
                    writer.Write(pc);
                    writer.EndRpc();
                    SidekickPlayer(PlayerId, pc.PlayerId);
                },
                () =>
                {
                    JackalSidekickButton.Timer = JackalSidekickButton.maxTimer;
                },
                "Sidekick",
                false, remainUses: 1);

        }
        public static void SidekickPlayer(int sourceId, int targetId)
        {
            RoleSelect.ChangeMainRole(targetId, (int)Roles.Sidekick);
        }
    }

}

