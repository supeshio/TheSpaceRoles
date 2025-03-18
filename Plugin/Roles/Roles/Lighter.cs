using Rewired;
using System;
using UnityEngine;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class Lighter : CustomRole
    {
        public Lighter()
        {
            Team = Teams.Crewmate;
            Role = Roles.Lighter;
            Color = Helper.ColorFromColorcode("#eee5be");
        }

        public static CustomOption LightCoolDown;
        public static CustomOption LightSeconds;
        public static CustomOption LightSize;
        public override void OptionCreate()
        {
            if (LightCoolDown != null) return;

            LightCoolDown = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.lighter.lightcooldown", range:CustomCoolDownRangefloat(), 12);
            LightSeconds = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.lighter.lightseconds", new CustomFloatRange(2.5f,120f,2.5f),3);
            LightSize = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.lighter.lightsize", new CustomFloatRange(0.25f,5f,0.25f),7);

            Options = [LightCoolDown, LightSeconds,LightSize];
        }
        public override void Update()
        {
            if (Input.GetKeyInt(KeyCode.L))
            {
                DestroyableSingleton<LightSource>.Instance.SetupLightingForGameplay(true, 8.0f, DestroyableSingleton<LightSource>.Instance.transform);
            }
            else{
                DestroyableSingleton<LightSource>.Instance.flashlightSize = GameOptionsManager.Instance.normalGameHostOptions.CrewLightMod;

            }
        }
        CustomButton LightButton;
        public override void HudManagerStart(HudManager __instance)
        {
            LightButton = new CustomButton(
                __instance, "ShifterShiftButton"
                ,
                ButtonPos.Custom,
                KeyCode.F,
                LightCoolDown.GetFloatValue(),()=>0,
                Sprites.GetSpriteFromResources("ui.button.lighter_light.png", 100f),
                () =>
                {
                    var pc = Helper.GetPlayerById(CustomButton.SetTarget());
                },
                () =>
                {
                    LightButton.Timer = LightButton.maxTimer;
                },
                "Light",true,EffectDuration:LightSeconds.GetFloatValue()
                );
            Logger.Info("button:Shifter Shifting");

        }
        public override Tuple<ChangeLightReason, float> GetLightMod(ShipStatus shipStatus, float num)
        {
            if (!LightButton.isEffectActive)
            {
                return base.GetLightMod(shipStatus, num);
            }
            else
            {


                float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;

                return (ChangeLightReason.LighterLight,shipStatus.MaxLightRadius*LightSize.GetFloatValue()).ToTuple();
            }
        }
    }
}
