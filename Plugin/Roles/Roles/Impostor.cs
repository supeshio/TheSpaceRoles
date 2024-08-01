namespace TheSpaceRoles
{
    public class Impostor : CustomRole
    {
        public Impostor()
        {

            teamsSupported = [Teams.Impostor];
            Role = Roles.Impostor;
            Color = Palette.ImpostorRed;
            HasKillButton = true;
        }
    }
}
