namespace TheSpaceRoles
{
    //[HarmonyPatch]
    //public static class RoleOptions
    //{
    //    [HarmonyPatch(typeof(RolesSettingsMenu),nameof(RolesSettingsMenu.Start))]
    //    public static void Postfix()
    //    {
    //        var roletab = GameSettingMenu.Instance.RoleSettingsTab;
    //        var v = GameObject.Instantiate(roletab.roleOptionSettingOrigin);
    //        Transform parent = GameSettingMenu.Instance.RoleSettingsTab.roleChances[0].transform.parent;
    //        GameSettingMenu.Instance.RoleSettingsTab.roleChances.ToArray().Do(x => GameObject.Destroy(x.gameObject));
    //        GameSettingMenu.Instance.RoleSettingsTab.roleChances.Clear();
    //        var rolesetting = GameObject.Instantiate(GameSettingMenu.Instance.RoleSettingsTab.roleOptionSettingOrigin.gameObject, parent).GetComponent<RoleOptionSetting>();
    //        rolesetting.Initialize();
    //        //rolesetting.SetClickMask(GameSettingMenu.Instance.RoleSettingsTab.AllButton);
    //        rolesetting.titleText.text = "水素の音";
    //        //rolesetting.SetClickMask(GameSettingMenu.Instance.RoleSettingsTab.roleChances[0].);
    //    }
    ////}
    //[HarmonyPatch]
    //public static class RoleOptions
    //{

    //}
}
