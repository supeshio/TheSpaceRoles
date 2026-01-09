using TSR.Game;
using TSR.Game.Options.OptionControlUI;
using TSR.Patch.Command;

namespace TSR.Patch
{
    [SmartPatch(typeof(HudManager))]
    public static class HudManagerPatch
    {
        [SmartPatch(nameof(HudManager.Start)), SmartPostfix]
        public static void Start(HudManager __instance)
        {
            OptionUIManager.Create();
        }

        [SmartPatch(nameof(HudManager.Update)), SmartPostfix]
        public static void Update(HudManager __instance)
        {
            KeyCommands.DefaultCommands();
            FUIManager.Update();
        }
    }
}