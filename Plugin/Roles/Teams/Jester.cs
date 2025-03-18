using System;
using UnityEngine;

namespace TheSpaceRoles
{
    public class JesterTeam : CustomTeam
    {
        public JesterTeam()
        {
            Team = Teams.Jester;
            Color = Helper.ColorFromColorcode("#ec62a5");
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
            var v = DataBase.GetPlayerCountInTeam();
            if (this.Role.Exiled)
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


            return (ChangeLightReason.Impostor, Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod).ToTuple();
            //return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod;

        }
    }
}
