using System;

namespace TSR.Game.Options
{
    public static class CustomOptionManager
    {
        //private static string OptionsDirectory = Path.Combine(Path.GetDirectoryName(Application.dataPath)!, Assets.ResourcesLoader.ass, "Option");
    }

    public static class CustomOptionHolder
    {
        //public static List<OptionBase> Options = [];

        public static void Load()
        {
            // foreach (RoleId role in Enum.GetValues(typeof(RoleId)))
            // {
            //     //Options.Add(new OptionDataDecimal((CustomOptionTab.Tab)10000, (OptionId)((int)role) + 10000, string.Format("role.{0}.count", role.ToString()), new DecimalRange()));
            // }
            // foreach (TeamId team in Enum.GetValues(typeof(TeamId)))
            // {
            //     //Options.Add(new OptionDataDecimal((CustomOptionTab.Tab)20000, (OptionId)((int)team) + 20000, string.Format("team.{0}.count", team.ToString()), new DecimalRange()));
            // }
        }
    }
}