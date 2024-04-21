using HarmonyLib;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckMurder))]
    public static class DeletePet
    {
        public static void Postfix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl player)
        {
            player.RpcSetPet("");
        }
    }
    [HarmonyPatch(typeof(PetBehaviour), nameof(PetBehaviour.SetMourning))]
    public static class Pet
    {
        public static void Prefix(PetBehaviour __instance
            )
        {
            Logger.Info("DeletePet");
            __instance.gameObject.transform.localPosition = new(0, 0, 1000);
            __instance.targetPlayer.RpcSetPet("");
        }
    }
}
