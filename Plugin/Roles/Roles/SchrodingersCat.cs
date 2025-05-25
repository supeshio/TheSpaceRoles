using UnityEngine;

namespace TheSpaceRoles
{
    public class SchrodingersCat : CustomRole
    {
        public SchrodingersCat()
        {
            Team = Teams.Schrodingers;
            Role = Roles.SchrodingersCat;
            Color = Color.grey;
        }
        public bool IsDeadOnce = false;
        public override void HudManagerStart(HudManager __instance)
        {

            IsDeadOnce = false;
        }

        public override bool BeMurdered(PlayerControl target)
        {
            if (IsDeadOnce)
            {
                Logger.Info(IsDeadOnce.ToString());
                return false;
            }

            CustomTeam = RoleData.GetCustomTeamFromTeam(target.GetCustomRole().Team);
            Team = CustomTeam.Team;
            Color = CustomTeam.Color;
            HasKillButton = CustomTeam.HasKillButton && CustomTeam.HasKillButton;
            Init();

            IsDeadOnce = true;
            return true;
        }





        public static CustomOption SHasKillButton;
        public override void OptionCreate()
        {
            SHasKillButton = CustomOption.Create(CustomOption.OptionType.Neutral, "role.schrodingerscat.haskillbutton", false);

            Options = [SHasKillButton];
        }
    }
}
