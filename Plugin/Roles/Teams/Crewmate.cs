using System.Linq;

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
                if (!DataBase.AllPlayerTeams.Any(x => x.Value == Teams.Crewmate && x.Key == item.PlayerId))
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
    }
}
