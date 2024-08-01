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
    }
}
