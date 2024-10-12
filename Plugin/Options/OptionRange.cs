using System.Collections.Generic;

namespace TheSpaceRoles
{
    public enum OptionSelectionAdditionalSelector
    {
        unlimted,
        right,
        left,
        on,
        off,
        killdistance_veryshort,
        killdistance_short,
        killdistance_medium,
        killdistance_long,
        killdistance_default,
    }
    public enum OptionSelectionCountType
    {
        time,
        size,
        killdistance
    }
    public static class Range
    {
        public static string GetStringFromSelector(this OptionSelectionAdditionalSelector selector) =>
            selector switch
            {
                OptionSelectionAdditionalSelector.killdistance_veryshort or
                OptionSelectionAdditionalSelector.killdistance_short or
                OptionSelectionAdditionalSelector.killdistance_medium or
                OptionSelectionAdditionalSelector.killdistance_long or
                OptionSelectionAdditionalSelector.killdistance_default
                => Translation.GetString($"option.selection.killdistance.{selector.ToString()[13..]}"),
                _ => Translation.GetString($"option.selection.{selector.ToString().ToLower()}"),
            };


        public static List<float> KillDistances = new() { 0.5f, 1f, 1.8f, 2.5f };
        public static List<string> KIllDistanceNames = ["veryshort", "short", "medium", "long"];
        public class RangeBase
        {
            public virtual string GetRange(int i)
            {
                var list = GetSelectors();
                return list[i];
            }

            public virtual string[] GetSelectors() => [];
        }

        public class FloatRange : RangeBase
        {
            public override string[] GetSelectors()
            {
                List<string> list = [];
                for (float i = min; i <= max; i += step)
                {
                    list.Add(i.ToString());
                }
                foreach (var item in addtional)
                {
                    list.Add(item.GetStringFromSelector());
                }
                return list.ToArray();
            }


            float max;
            float min;
            float step;
            List<OptionSelectionAdditionalSelector> addtional;
            public FloatRange(float min, float max, float step, List<OptionSelectionAdditionalSelector> selectors = null)
            {
                this.min = min;
                this.max = max;
                this.step = step;
                if(selectors != null)
                {
                    this.addtional = selectors;
                    addtional.Sort();

                }
                else
                {

                    this.addtional = [];
                }
            }
        }
        public class SelectorRange : RangeBase
        {
            public override string[] GetSelectors()
            {
                List<string> list = [];
                foreach (var item in addtional)
                {
                    list.Add(item.GetStringFromSelector());
                }
                return [.. list];
            }


            List<OptionSelectionAdditionalSelector> addtional;
            public SelectorRange(List<OptionSelectionAdditionalSelector> selectors)
            {
                this.addtional = selectors;
                addtional.Sort();
            }
        }




    }
}
