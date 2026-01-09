namespace TSR.Patch;

[SmartPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
public class ShowModStampPatch
{
    public static void Postfix(ModManager __instance)
    {
        __instance.ShowModStamp();
    }
}