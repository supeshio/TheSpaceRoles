namespace TSR.Game.Role
{

    [SmartPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class ShowRoleNameInMeetingPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            Logger.Info("MeetingHud Started");
            FPlayerControl.MeetingSetUp(__instance);
        }
    }
}
