namespace TheSpaceRoles
{
    public class Jester : CustomRole
    {
        public Jester()
        {

            Team = Teams.Jester;
            Role = Roles.Jester;
            Color = new JesterTeam().Color;
        }
    }
}
