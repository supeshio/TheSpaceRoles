using System.Collections.Generic;
using System.Linq;
using TSR.Game.Options.OptionControlUI.OptionTab.Option.Level;
using UnityEngine;

namespace TSR.Game.Options.OptionControlUI.OptionTab.Option
{
    public static class OptionLevelManager
    {
        /// <summary>
        /// key : GroupId
        /// </summary>
        /// <returns>optionLevel</returns>
        public static Dictionary<string,OptionLevel> OptionLevels { get; private set; } = new Dictionary<string, OptionLevel>();

        public static void PrepareOptions()
        {
            var list = CustomOption.CustomOptions.Values.ToList();
            foreach (CustomOptionAttribute t in list)
            {
                switch (t)
                {
                    case CustomOptionBoolAttribute:
                        //bool
                        break;
                    case CustomOptionIntAttribute:
                        //int
                        break;
                    case CustomOptionFloatAttribute:
                        //float
                        break;
                    case CustomOptionEnumAttribute:
                        //string
                        break;
                }
            }

            var p = CreateOptionGroup("tsr:sheriff", OptionTab.RoleTab.MainTabScroller.Inner);
            p.AddItem(new StandardLevel("tsr:sheriff.kill-button.max-cooldown",p.Inner));;
            p.AddItem(new StandardLevel("tsr:sheriff.kill-button.count",p.Inner));;
            
            p.Update(-GroupLevel.GroupSpace,OptionTab.OptionTabBase.MainGroupSizeX );
        }

        private static GroupLevel CreateOptionGroup(string id,Transform parent)
        {
            return new GroupLevel(id,parent);
        }
    }
}
