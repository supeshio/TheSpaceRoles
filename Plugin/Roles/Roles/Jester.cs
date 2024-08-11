namespace TheSpaceRoles
{
    public class Jester : CustomRole
    {
        public Jester()
        {

            team = Teams.Jester;
            Role = Roles.Jester;
            Color = new JesterTeam().Color;
        }
    }
}
