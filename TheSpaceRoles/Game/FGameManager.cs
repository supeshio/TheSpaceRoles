using AmongUs.GameOptions;
using InnerNet;
using TSR.Game.Role;

namespace TSR.Game
{
    public enum CustomGameState : int
    {
        None = 0,
        NotInGame,
        WaitingForStarting,
        Intro,
        PlayingGame,
        EndGame,

    }
    public class FGameManager
    {
        public FGameManager()
        {
            //this.Highlight = new PlayerHighlight();
            this.Local = new LocalPlayerData();
        }
        public static FGameManager Manager;
        public static bool AmHost => AmongUsClient.Instance.AmHost;
        public static CustomGameState GameState()
        {
            switch (AmongUsClient.Instance.GameState)
            {
                case InnerNetClient.GameStates.NotJoined:
                    return CustomGameState.NotInGame;
                case InnerNetClient.GameStates.Joined:
                    return CustomGameState.WaitingForStarting;
                case InnerNetClient.GameStates.Started:
                    return CustomGameState.PlayingGame;
                case InnerNetClient.GameStates.Ended:
                    return CustomGameState.EndGame;
                default:
                    return CustomGameState.None;


            }
        }
        public static GameModes CurrentGameMode => GameOptionsManager.Instance.currentGameMode;
        public static NetworkModes NetworkMode() { return AmongUsClient.Instance.NetworkMode; }
        public LocalPlayerData Local { get; private set; }
        public class LocalPlayerData
        {
            public LocalPlayerData()
            {
            }
            public PlayerTargetHighlight Highlight;
            public PlayerTargetBase? Target;
        }

        [SmartPatch(typeof(GameManager), nameof(GameManager.FixedUpdate))]
        public static class FGameManagerGMFixedUpdatePatch
        {
            public static void Postfix(GameManager __instance)
            {
                if (FGameManager.GameState() == CustomGameState.PlayingGame | FGameManager.NetworkMode()==NetworkModes.FreePlay)
                {
                    FGameManager.Manager.Local.Target?.SetTarget();
                }
            }
        }
        [SmartPatch(typeof(PlayerControl), nameof(PlayerControl.ToggleHighlight))]
        public static class FGameManagerPlayerControlToggleHighlightPatch
        {
            public static bool Prefix(PlayerControl __instance)
            {
                return false;
            }
        }
    }
}