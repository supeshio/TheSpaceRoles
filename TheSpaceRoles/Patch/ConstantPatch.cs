using HarmonyLib;

namespace TSR.Patch
{
    [HarmonyPatch(typeof(Constants), nameof(Constants.GetBroadcastVersion))]
    public static class ConstantsGetBroadcastVersionPatch
    {
        public static void Postfix(ref int __result)
        {
            bool IsLocalGame = AmongUsClient.Instance.NetworkMode == NetworkModes.LocalGame;
            bool IsFreePlay = AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay;
            if (IsLocalGame || IsFreePlay)
            {
                return;
            }
            Logger.Info("Version:" + __result);
            Logger.Info("ModderVersion:" + Constants.MODDER_VERSION);
            Logger.Info("ModderVersion:" +string.Join(',', Constants.CompatVersions));
            __result += 25;
            //__result += Constants.MODDER_VERSION;
        }
    }

    // AU side bug?
    [HarmonyPatch(typeof(Constants), nameof(Constants.IsVersionModded))]
    public static class ConstantsIsVersionModdedPatch
    {
        public static bool Prefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}