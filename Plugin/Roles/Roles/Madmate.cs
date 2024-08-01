namespace TheSpaceRoles
{
    public class Madmate : CustomRole
    {
        public static CustomButton VampireBitebutton;
        public PlayerControl BittenPlayerControl;
        public Madmate()
        {

            teamsSupported = [Teams.Madmate];
            Role = Roles.MadMate;
            Color = Palette.ImpostorRed;
            HasKillButton = false;
        }
        public override void HudManagerStart(HudManager __instance)
        {

            foreach (Vent vent in ShipStatus.Instance.AllVents)
            {
                vent.Right = null;
                vent.Center = null;
                vent.Left = null;
            }
        }
        public override void Update()
        {
        }
    }
}
