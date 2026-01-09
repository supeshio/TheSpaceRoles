using TSR.Game.Role;
using TSR.Game.Role.Ability;

namespace TSR.Game
{
    public static class MCall
    {
        public static FGameManager FGM => FGameManager.Manager;

        [SmartPatch(typeof(GameManager), nameof(GameManager.StartGame))]
        public static class StartGame
        {
            public static void Prefix()
            {
                if (FGameManager.NetworkMode() == NetworkModes.FreePlay)
                {
                    FPlayerControl.FPCSet.FPCSET();
                    RoleAssigner.SetAllRole("tsr:crewmate", [.. FPlayerControl.AllPlayer]);
                }
            }
            public static void Postfix()
            {
                Logger.Info($"allp:{FPlayerControl.AllPlayer.Length}");

                foreach (var fpc in FPlayerControl.AllPlayer)
                {
                    fpc.GameStartReset();
                }

                Game.Role.Ability.CustomButton.GameStartResetButtons();
            }
        }

        [SmartPatch(typeof(MapBehaviour), nameof(MapBehaviour.Show))]
        public static class Map
        {
            public static void Prefix(MapBehaviour __instance, MapOptions opts)
            {
                foreach (var fpc in FPlayerControl.AllPlayer)
                {
                    //fpc.Value.FRole.OnGameStart();
                    //fpc.Value.FTeam.OnGameStart();
                    foreach (var abilitiy in fpc.Abilitiies)
                    {
                        ((IAbilityBase)abilitiy).MapOpened(ref __instance, ref opts);
                    }
                }
            }
        }
    }
}