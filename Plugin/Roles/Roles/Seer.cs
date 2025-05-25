using System.Linq;

namespace TheSpaceRoles
{
    public class Seer : CustomRole
    {
        public Seer()
        {
            Team = Teams.Crewmate;
            Role = Roles.Seer;
            Color = Helper.ColorFromColorcode("#61b26c");
        }
        public static CustomOption CanSeeSoul;
        public override void OptionCreate()
        {
            if (CanSeeSoul != null) return;

            CanSeeSoul = CustomOption.Create(CustomOption.OptionType.Crewmate, "role.seer.canseesoul", true);

            Options = [CanSeeSoul];
        }
        public override void Murder(PlayerControl pc, PlayerControl target)
        {
            if (target != PlayerControl.LocalPlayer && !PlayerControl.LocalPlayer.CachedPlayerData.IsDead)
                FlashPatch.ShowFlash(Helper.ColorFromColorcode("#00ff00"));

        }
        public override void AfterMeetingEnd()
        {
            DeathGhost.DisapperGhosts();
            if (CanSeeSoul.GetBoolValue())
            {
                DeathGhost.ShowGhosts(DataBase.AllPlayerData.Where(x => x.Value.DeathMeetingCount + 1 == DataBase.MeetingCount).Select(x => x.Key).ToArray());
            }
        }
    }
}
