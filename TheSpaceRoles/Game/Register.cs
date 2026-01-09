using TSR.Game.Role;
using TSR.Game.Role.Role.CustomRole;
using TSR.Game.Role.Role.CustomTeam;

namespace TSR.Game
{
    public static class Register
    {
        public static void Init()
        {
            //TeamBaseRegister
            TeamBaseRegister.RegisterTeam<ImpostorTeam>();
            TeamBaseRegister.RegisterTeam<CrewmateTeam>();

            //RoleBaseRegister
            RoleBaseRegister.RegisterRole<Impostor>();
            RoleBaseRegister.RegisterRole<Crewmate>();
            RoleBaseRegister.RegisterRole<Sheriff>();

        }
    }
}
