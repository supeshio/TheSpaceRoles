namespace TheSpaceRoles
{
    //[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.Start))]
    //public static class ShipAdditionalRole
    //{
    //    public static void Postfix(ShipStatus __instance)
    //    {
    //        if (__instance.transform.FindChild("TaskAddConsole")?.gameObject == null) return;
    //        var con = GameObject.Instantiate(__instance.transform.FindChild("TaskAddConsole").gameObject, __instance.transform);
    //        con.name = "AddtionalRoleConsole";
    //        con.transform.localPosition += new Vector3(-1, -1, 0);
    //        var spriterend = con.GetComponent<SpriteRenderer>();
    //        spriterend.flipX = true;
    //    }

    //}
}
