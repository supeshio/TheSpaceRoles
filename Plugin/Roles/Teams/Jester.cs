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
    }
}
