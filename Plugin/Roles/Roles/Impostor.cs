namespace TheSpaceRoles
{
    public class Impostor : CustomRole
    {
        public Impostor()
        {

            team = Teams.Impostor;
            Role = Roles.Impostor;
            Color = Palette.ImpostorRed;
            HasKillButton = true;
        }
    }
}
