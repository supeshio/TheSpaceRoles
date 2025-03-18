using System;
using UnityEngine;

namespace TheSpaceRoles
{
    public class SchrodingersTeam : CustomTeam
    {
        public SchrodingersTeam()
        {
            Team = Teams.Schrodingers;
            Color = Color.grey;
            HasKillButton = false;
            CanRepairSabotage = true;
            CanUseAdmin = true;
            CanUseBinoculars = true;
            CanUseCamera = true;
            CanUseDoorlog = true;
            CanUseVent = false;
            CanUseVital = true;
            HasTask = false;
            ShowingAdminIncludeDeadBodies = true;
            ShowingMapAllowedToMove = true;
            ImpostorMap = false;
            AdminMap = false;
        }
        public override bool WinCheck()
        {
            return false;
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
