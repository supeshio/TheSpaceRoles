using HarmonyLib;

namespace TheSpaceRoles
{

    [HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
    public class ShowModStampPatch
    {
        public static void Postfix(ModManager __instance)
        {
            __instance.ShowModStamp();
            LateTask.Update();
        }
    }
}
