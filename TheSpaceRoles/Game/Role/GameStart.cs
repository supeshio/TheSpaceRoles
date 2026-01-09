namespace TSR.Game.Role
{
    [SmartPatch(typeof(GameData), nameof(GameData.Awake))]
    public static class StartGame
    {
        public static void Postfix(GameData __instance)
        {
            if (FGameManager.Manager == null)
            {
                FGameManager.Manager = new();
                Logger.Info("Create fgm");
                //FPlayerControl.LocalPlayer.FRole.Init();
            }
        }
    }
}