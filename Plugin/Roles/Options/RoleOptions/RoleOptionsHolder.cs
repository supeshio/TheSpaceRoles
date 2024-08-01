using System;
using System.Collections.Generic;
using System.Linq;

namespace TheSpaceRoles
{
    public static class RoleOptionsHolder
    {
        public static List<RoleOptions> roleOptions = new();
        public static Roles selectedRoles = Roles.None;
        public static void RoleOptionsCreate()
        {

            roleOptions = [];
            int i = 0;
            //一旦全部取得にしてるけど後で変えるかも?

            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                if (GetLink.CustomRoleLink.Any(x => x.Role == role))
                {
                    roleOptions.Add(new RoleOptions(role, i));

                    i++;
                }
            }
        }

    }
}
