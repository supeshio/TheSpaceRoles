using System;

namespace TheSpaceRoles
{
    public enum CustomOptionSelectorSetting
    {
        General,
        InformationEquipment,
        Starter
    }
    public class CustomOptionSelectorHolder
    {
        public static void CreateSelector()
        {
            foreach (CustomOptionSelectorSetting option in Enum.GetValues(typeof(CustomOptionSelectorSetting)))
            {
                _ = new CustomOptionSelector(option);
            }
        }
    }
}
