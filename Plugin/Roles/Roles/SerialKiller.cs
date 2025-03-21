﻿using UnityEngine;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    public class SerialKiller : CustomRole
    {
        public float Timer = 60f;
        public float Maxtimer = 60f;
        public bool TimerStarted = false;
        public CustomButton SerialKillerKillButton;
        public SerialKiller()
        {

            Team = Teams.Impostor;
            Role = Roles.SerialKiller;
            Color = Palette.ImpostorRed;
            HasKillButton = false;
        }
        public override void HudManagerStart(HudManager __instance)
        {
            SerialKillerKillButton = new CustomButton(
                __instance, "SerialKillerButton"
                , this,
                ButtonPos.Kill,
                KeyCode.Q,
                20,
                () => KillButtons.KillButtonSetTarget(2.5f, Color, [Teams.Impostor]),
                __instance.KillButton.graphic.sprite,
                () =>
                {
                    var pc = GetPlayerById(KillButtons.KillButtonSetTarget(2.5f, Color, [Teams.Impostor]));
                    CheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, pc, DeathReason.SerialKillerKill);
                    TimerStarted = true;
                    Timer = Maxtimer;

                },
                () =>
                {
                    SerialKillerKillButton.Timer = SerialKillerKillButton.maxTimer;
                },
                "Kill",
                false);
            Maxtimer = 60f;
            Timer = Maxtimer;
            Logger.Info("button:SerialKillerButton");

        }
        public override void Update()
        {
            Logger.Message("Update", Roles.SerialKiller.ToString());
            if (TimerStarted)
            {

                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    if (Dead == false)
                    {

                        UnCheckedMurderPlayer.RpcMurder(PlayerControl.LocalPlayer, PlayerControl.LocalPlayer, DeathReason.SerialKillerSuicide);
                    }
                    Dead = true;
                }

            }
            SerialKillerKillButton.AdditionalText.text = Translation.GetString("role.serialkiller.timer_remain", [((int)Timer).ToString()]);

        }
    }
}
