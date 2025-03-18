using UnityEngine;
using static TheSpaceRoles.CustomButton;
using static TheSpaceRoles.Helper;
using static TheSpaceRoles.Ranges;

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
        public bool IsDeadOnce=false;
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
            HasKillButton = CustomTeam.HasKillButton&&CustomTeam.HasKillButton;
            Init();

            IsDeadOnce = true;
            return true;
        }





        public static CustomOption SHasKillButton;
        public override void OptionCreate()
        {
            SHasKillButton = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.schrodingersCat", false);

            Options = [SHasKillButton];
        }
    }
}
