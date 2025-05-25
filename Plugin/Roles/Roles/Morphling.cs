using UnityEngine;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class Morphling : CustomRole
    {
        CustomButton MorphButton;
        CustomButton CopyButton;
        PlayerControl target;
        public static CustomOption MorphlingTime;
        public static CustomOption MorphCoolDown;
        public Morphling()
        {
            Team = Teams.Impostor;
            Role = Roles.Morphling;
            Color = Palette.ImpostorRed;
        }
        public override void OptionCreate()
        {
            if (MorphlingTime != null) return;

            MorphlingTime = CustomOption.Create(CustomOption.OptionType.Impostor, "role.morphling.morphlingtime", CustomCoolDownRangefloat(), 12);
            MorphCoolDown = CustomOption.Create(CustomOption.OptionType.Impostor, "role.morphling.morphcooldown", CustomCoolDownRangefloat(), 12);

            Options = [MorphlingTime, MorphCoolDown];
        }

        public override void HudManagerStart(HudManager __instance)
        {
            MorphButton = new CustomButton(
                __instance, "MorphlingMorphButton"
                , this,
                ButtonPos.Custom,
                KeyCode.F,
                0,
                () => 0,
                Sprites.GetSpriteFromResources("ui.button.evilhacker_hack.png", 100f),
                () =>
                {
                },
                () =>
                {
                    MorphButton.actionButton.gameObject.SetActive(false);
                },
                "Morph",
                true, EffectDuration: MorphlingTime.GetFloatValue(),
                OnEffectStart:
                () =>
                {

                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 0);
                    writer.Write(target.PlayerId);
                    writer.EndRpc();
                    RpcMorph(PlayerId, target.PlayerId);
                },

                OnEffectEnd:
                () =>
                {
                    var writer = CustomRPC.SendRpcUseAbility(Role, PlayerControl.PlayerId, 1);
                    writer.Write(target.PlayerId);
                    writer.EndRpc();
                    RpcMorphEnd(PlayerId);
                    CopyButton.Timer = MorphCoolDown.GetFloatValue();
                    MorphButton.actionButton.gameObject.SetActive(false);
                    CopyButton.actionButton.gameObject.SetActive(true);
                }
                );
            Logger.Info("button:Morphling Morph");
            {
                CopyButton = new CustomButton(
                    __instance, "MorphlingCopyButton"
                    , this,
                    ButtonPos.Custom,
                    KeyCode.F,
                    0f,
                    () => CustomButton.SetTarget(),
                    Sprites.GetSpriteFromResources("ui.button.morphing_copy.png", 100f),
                    () =>
                    {
                        target = GetPlayerById(CustomButton.SetTarget());
                        MorphButton.actionButton.gameObject.SetActive(true);
                        CopyButton.actionButton.gameObject.SetActive(false);
                    },
                    () =>
                    {
                        CopyButton.Timer = MorphCoolDown.GetFloatValue();
                        CopyButton.actionButton.gameObject.SetActive(true);
                    },
                    "Copy",
                    false);
                Logger.Info("button:Morphling Copy");
            }

            MorphButton.actionButton.gameObject.SetActive(false);
            CopyButton.actionButton.gameObject.SetActive(true);
        }
        public static void RpcMorph(int playerId, int target)
        {

            Helper.SetCosmetics(GetPlayerById(playerId), GetPlayerById(target));
            Helper.SetName(GetPlayerById(playerId), GetPlayerById(target));
        }
        public static void RpcMorphEnd(int playerId)
        {
            Helper.SetCosmetics(GetPlayerById(playerId), GetPlayerById(playerId));
            Helper.SetName(GetPlayerById(playerId), GetPlayerById(playerId));

        }
    }
}
