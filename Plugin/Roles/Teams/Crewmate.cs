using System;
using System.Linq;
using UnityEngine;

namespace TheSpaceRoles
{
    public class CrewmateTeam : CustomTeam
    {
        public CrewmateTeam()
        {
            Team = Teams.Crewmate;
            Color = Palette.CrewmateBlue;
            HasKillButton = false;
            CanRepairSabotage = true;
            CanUseAdmin = true;
            CanUseBinoculars = true;
            CanUseCamera = true;
            CanUseDoorlog = true;
            CanUseVent = false;
            CanUseVital = true;
            HasTask = true;
        }
        public override Tuple<ChangeLightReason, float> GetLightMod(ShipStatus shipStatus, float num)
        {
            float ImpostorLightMod = GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
            float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
            /*|| (Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId && Jackal.hasImpostorVision))*/


            //return shipStatus.MaxLightRadius * ImpostorLightMod;
            return (ChangeLightReason.Crewmate, Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod).ToTuple();

        }

        public override Teams CheckCount()
        {
            return Teams.Crewmate;
        }
    }
}
