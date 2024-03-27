using System.Collections.Generic;
using static TheSpaceRoles.CustomOption;


namespace TheSpaceRoles
{
    public static class CustomOptionsHolder
    {
        public static void CreateCustomOptions()
        {
            options.Clear();

            Create("tsr","use_admin", true);
            Create("tsr","use_recodes_admin", true, "use_admin", Onfunc);

            List<string> o = new();
            for (float i = 0; i <= 180; i += 2.5f)
            {
                o.Add(Second(i));
            }
            Create("tsr","use", [..o], "0");

            Create("tsr","user", [.. o], "0");
        }
    }
}
