namespace TheSpaceRoles
{
    public class Impostor : CustomRole
    {
        public Impostor()
        {

            Team = Teams.Impostor;
            Role = Roles.Impostor;
            Color = Palette.ImpostorRed;
            HasKillButton = true;
        }
    }
}
