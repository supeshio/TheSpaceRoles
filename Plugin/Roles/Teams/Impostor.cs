using System;

namespace TheSpaceRoles
{
    public class ImpostorTeam : CustomTeam
    {
        public ImpostorTeam()
        {
            Team = Teams.Impostor;
            Color = Palette.ImpostorRed;
            HasKillButton = true;
            CanRepairSabotage = true;
            CanUseAdmin = true;
            CanUseBinoculars = true;
            CanUseCamera = true;
            CanUseDoorlog = true;
            CanUseVent = true;
            CanUseVital = true;
            HasTask = false;
            ShowingAdminIncludeDeadBodies = true;
            ShowingMapAllowedToMove = true;
            ImpostorMap = true;
            AdminMap = false;
        }
        public override bool WinCheck()
        {
            var v = DataBase.GetPlayerCountInTeam();
            if (v[Teams.Impostor] >= DataBase.GetAsCrewmatePlayerCount() && v[Teams.Jackal] == 0)
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


            return (ChangeLightReason.Impostor, shipStatus.MaxLightRadius * ImpostorLightMod).ToTuple();
            //return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod;

        }
    }
}
