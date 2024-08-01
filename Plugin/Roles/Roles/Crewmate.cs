namespace TheSpaceRoles
{
    public class Crewmate : CustomRole
    {
        public Crewmate()
        {
            teamsSupported = [Teams.Crewmate];
            Role = Roles.Crewmate;
            Color = Palette.CrewmateBlue;
        }
    }
}
