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
        public override bool WinCheck()
        {
            bool wincheck = true;
            foreach (var item in DataBase.AllPlayerControls())
            {
                if (!DataBase.AllPlayerData.Any(x => x.Value.Team == Teams.Crewmate && x.Key == item.PlayerId))
                {
                    wincheck = false; break;
                }
            }


            if (wincheck) return wincheck;
            if (DataBase.AlivingKillPlayer() == 0)
            {
                return true;
            }
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
