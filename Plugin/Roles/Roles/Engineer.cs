namespace TheSpaceRoles
{
    public class Engineer : CustomRole
    {
        public Engineer()
        {
            Team= Teams.Crewmate;
            Role = Roles.Engineer;
            Color = Helper.ColorFromColorcode("#0028f5");
            CanUseVent = true;
            CanUseVentMoving = true;
        }
        public override void HudManagerStart(HudManager hudManager)
        {
            //DestroyableSingleton<VentButton>.Instance.SetUsesRemaining(5);
        }
    }
}
