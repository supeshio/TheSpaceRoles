namespace TSR.Patch
{
    [SmartPatch(typeof(GameStartManager), nameof(GameStartManager.BeginGame))]
    public static class GameStartManagerPatch
    {
    }
}