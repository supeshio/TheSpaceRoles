namespace TheSpaceRoles
{
    public class MadmateTeam : CustomTeam
    {
        public MadmateTeam()
        {
            Team = Teams.Madmate;
            Color = Helper.ColorFromColorcode("#9e1212");
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
    }
}
