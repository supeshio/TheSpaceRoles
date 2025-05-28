using System;

namespace TheSpaceRoles
{
    public class JackalTeam : CustomTeam
    {
        public JackalTeam()
        {
            Team = Teams.Jackal;
            Color = Helper.ColorFromColorcode("#09afff");
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
        public override Tuple<ChangeLightReason, float> GetLightMod(ShipStatus shipStatus, float num)
        {
            float ImpostorLightMod = GameOptionsManager.Instance.currentNormalGameOptions.ImpostorLightMod;
            float CrewLightMod = GameOptionsManager.Instance.currentNormalGameOptions.CrewLightMod;
            /*|| (Jackal.jackal != null && Jackal.jackal.PlayerId == player.PlayerId && Jackal.hasImpostorVision))*/


            return (ChangeLightReason.Impostor, shipStatus.MaxLightRadius * ImpostorLightMod).ToTuple();
            //return Mathf.Lerp(shipStatus.MinLightRadius, shipStatus.MaxLightRadius, num) * CrewLightMod;

        }
        public override Teams CheckCount()
        {
            return Teams.Jackal;
        }
    }
}
