using HarmonyLib;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(KillAnimation), nameof(KillAnimation.CoPerformKill))]
    class KillAnimationPatch
    {
        public static bool AnimCancel = false;

        public static void Prefix(KillAnimation __instance, [HarmonyArgument(0)] ref PlayerControl source, [HarmonyArgument(1)] ref PlayerControl target)
        {
            if (AnimCancel)
            {
                source = target;
                Logger.Info("source = target");

            }
            AnimCancel = false;
        }
        public static void Postfix(KillAnimation __instance, [HarmonyArgument(0)] ref PlayerControl source, [HarmonyArgument(1)] ref PlayerControl target)
        {
            Logger.Info($"KillAnim {source.PlayerId}->{target.PlayerId}");
        }
    }
    /*
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.Exiled))]
    public static class PCExiled
    {
        public static void Prefix(PlayerControl __instance)
        {

            Logger.Info("P,Exiled?");
            if (__instance.PlayerId == PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.buttons.Do(x => x.Death());
            }
        }
    }*//*
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.MurderPlayer))]
    public static class PlayerControlMurderPlayerPatch
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            CheckedMurderPlayer.RpcMurder(__instance, target, DeathReason.ImpostorKill, false);
        }
    }*/

}
