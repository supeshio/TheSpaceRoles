using Rewired.Utils.Classes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheSpaceRoles
{
    public enum CustomOptionSelectorSetting{
        General,
        InformationEquipment,
        Starter
    }
    public class CustomOptionSelectorHolder
    {
        public static void CreateSelector()
        {
            foreach(CustomOptionSelectorSetting option in Enum.GetValues(typeof(CustomOptionSelectorSetting)))
            {
                _ = new CustomOptionSelector(option);
            }
        }
    }
}
