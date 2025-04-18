using AmongUs.GameOptions;
using HarmonyLib;
using System;
using UnityEngine;
using static TheSpaceRoles.CustomButton;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class Ninja : CustomRole
    {
        public Ninja()
        {
            Team = Teams.Impostor;
            Role = Roles.Ninja;
            Color = Palette.ImpostorRed;
        }
        CustomButton NinjaButton;
        public override void HudManagerStart(HudManager __instance)
        {

            NinjaButton = new CustomButton(
                __instance, "NinjaButton"
                , this,
                ButtonPos.Custom/*ボタンタイプ*/,
                KeyCode.F/*ボタンキー*/,
                NinjaHideCoolDown.GetFloatValue()/*クールダウン*/,
                () => 0/*-1以外なら成功判定*/,
                Sprites.GetSpriteFromResources("ui.button.vampire_bite.png", 100f),
                () =>
                {

                },
                () =>
                {
                    NinjaButton.Timer = NinjaButton.maxTimer;
                },
                "Hide",
                true,EffectDuration:NinjaHideTime.GetFloatValue(),
                OnEffectStart: () => 
                {
                    var hide = CustomRPC.SendRpcUseAbility(Role, PlayerId, 0);
                    hide.EndRpc();
                    Hide();
                }, OnEffectEnd: () => {
                    var appear = CustomRPC.SendRpcUseAbility(Role, PlayerId, 1);
                    appear.EndRpc();
                    Appear();
                });
            //Effect

        }
        public override void MeetingStart(MeetingHud meeting)
        {
            var appear = CustomRPC.SendRpcUseAbility(Role, PlayerId, 1);
            appear.EndRpc();
        }
        public static CustomOption NinjaHideCoolDown;
        public static CustomOption NinjaHideTime;
        public static CustomOption NinjaSkillSpeed;
        public override void OptionCreate()
        {
            NinjaHideCoolDown = CustomOption.Create(CustomOption.OptionType.Impostor, "role.ninja.hidecooldown",CustomCoolDownRangefloat(),12);
            NinjaHideTime = CustomOption.Create(CustomOption.OptionType.Impostor, "role.ninja.hidetime",new CustomFloatRange(2.5f,60f,2.5f),5);
            NinjaSkillSpeed = CustomOption.Create(CustomOption.OptionType.Impostor, "role.ninja.speed", new CustomFloatRange(1.0f, 5.0f, 0.25f), 1);

            Options = [NinjaHideCoolDown,NinjaHideTime];
        }
        public static void NinjaHide(int playerId){
            ((Ninja)GetCustomRole(playerId)).Hide();
        }
        public static void NinjaHideEnd(int playerId)
        {
            ((Ninja)GetCustomRole(playerId)).Appear();
        }
        public float opacity=1f;
        public override void Update()
        {

            Helper.setOpacity(PlayerControl, opacity);
        }
        public void Hide()
        {
            speedMod =NinjaSkillSpeed.GetFloatValue();
            HudManager.Instance.StartCoroutine(Effects.Lerp(1.0f, new Action<float>((p) => {
                    opacity =  Mathf.Clamp01(1 - (PlayerControl == PlayerControl.LocalPlayer ? 0.6f : 1f * (p * p)));

                if (p >= 1f) opacity = 1 - (PlayerControl == PlayerControl.LocalPlayer ? 0.6f : 1f );
            })));



        }

        public void Appear()
        {

            speedMod =1f;
            HudManager.Instance.StartCoroutine(Effects.Lerp(1.0f, new Action<float>((p) => {
                opacity =  Mathf.Clamp01(PlayerControl == PlayerControl.LocalPlayer ? 0.6f : 1f * p * p);

                if (p >= 1f) opacity = 1f;
            })));


        }
    }
}
