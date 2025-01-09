using Rewired;
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

        public static CustomOption ReportSeconds;
        public static CustomOption CanSeePlayersinVent;
        public override void OptionCreate()
        {
            if (ReportSeconds != null) return;

            ReportSeconds = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.lighter.reportseconds", new CustomIntRange(1, 15), 0);
            CanSeePlayersinVent = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.lighter.canseeplayerinvent", true);

            Options = [ReportSeconds, CanSeePlayersinVent];
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
                30,()=>0,
                Sprites.GetSpriteFromResources("ui.button.evilhacker_hack.png", 100f),
                () =>
                {
                    var pc = Helper.GetPlayerById(CustomButton.SetTarget());
                },
                () =>
                {
                    LightButton.Timer = LightButton.maxTimer;
                },
                "Light",true,EffectDuration:10f
                );
            Logger.Info("button:Shifter Shifting");

        }
        public override float GetLightMod(ShipStatus shipStatus, float num)
        {
            if (!LightButton.isEffectActive)
            {
                return base.GetLightMod(shipStatus, num);
            }
            else
            {


                float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;

                return shipStatus.MaxLightRadius*2f;
            }
        }
    }
}
