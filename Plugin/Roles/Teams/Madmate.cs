using System;
using UnityEngine;

namespace TheSpaceRoles
{
    public class MadmateTeam : CustomTeam
    {
        public MadmateTeam()
        {
            Team = Teams.Madmate;
            Color = Helper.ColorFromColorcode("#ff3636");
            HasKillButton = false;
            CanRepairSabotage = true;
            CanUseAdmin = true;
            CanUseBinoculars = true;
            CanUseCamera = true;
            CanUseDoorlog = true;
            CanUseVent = true;
            CanUseVital = true;
            HasTask = false;
        }

        public override bool WinCheck()
        {
            return false;
        }
        public override bool AdditionalWinCheck(Teams winteam)
        {
            return winteam == Teams.Impostor;
        }
        public override Tuple<ChangeLightReason, float> GetLightMod(ShipStatus shipStatus, float num)
        {
            float ImpostorLightMod = GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
            float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
            /*|| (Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId && Jackal.hasImpostorVision))*/


            //return shipStatus.MaxLightRadius * ImpostorLightMod;
            return (ChangeLightReason.Crewmate, Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod).ToTuple();

        }
    }
}
