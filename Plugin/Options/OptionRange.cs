using System.Collections.Generic;

namespace TheSpaceRoles
{
    /// <summary>
    /// OptionSelectionAdvancedSelector
    /// </summary>
    public enum OSAS : int
    {
        unlimted = int.MinValue,
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
    public static class Ranges
    {
        public static float GetValueFromSelector(this OSAS selector) =>
            selector switch
            {
                OSAS.killdistance_veryshort or
                OSAS.killdistance_short or
                OSAS.killdistance_medium or
                OSAS.killdistance_long //or
                //OSAS.killdistance_default
                => KillDistances[KillDistanceNames.IndexOf(selector.ToString()[13..])],
                OSAS.on => -1,
                OSAS.off => -2,
                _ => (int)selector,
            };

        public static string GetStringFromSelector(this OSAS selector) =>
            selector switch
            {
                OSAS.killdistance_veryshort or
                OSAS.killdistance_short or
                OSAS.killdistance_medium or
                OSAS.killdistance_long or
                OSAS.killdistance_default
                => Translation.GetString($"option.selection.killdistance.{selector.ToString()[13..]}"),
                _ => Translation.GetString($"option.selection.{selector.ToString().ToLower()}"),
            };


        public static List<float> KillDistances = new() { 0.5f, 1f, 1.8f, 2.5f };
        public static List<string> KillDistanceNames = ["veryshort", "short", "medium", "long", "default"];
        public class CustomRange
        {
            public virtual float GetValue(int i)
            {
                var list = GetValues();
                return list[i];
            }

            public virtual string GetRange(int i)
            {
                var list = GetSelectors();
                return list[i];
            }
            public virtual string[] GetSelectors() => [];
            public virtual float[] GetValues() => [];
        }
        public static CustomRange KillDistanceRange() => new CustomSelectionRange([OSAS.killdistance_default, OSAS.killdistance_veryshort, OSAS.killdistance_short, OSAS.killdistance_medium, OSAS.killdistance_long]);
        public class CustomFloatRange : CustomRange
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
                return [.. list];
            }
            public override float[] GetValues()
            {
                List<float> list = [];
                for (float i = min; i <= max; i += step)
                {
                    list.Add(i);
                }
                foreach (var item in addtional)
                {
                    list.Add(item.GetValueFromSelector());
                }
                return [.. list];
            }


            float max;
            float min;
            float step;
            List<OSAS> addtional;
            public CustomFloatRange(float min, float max, float step, List<OSAS> selectors = null)
            {
                this.min = min;
                this.max = max;
                this.step = step;
                if (selectors != null)
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
        public class CustomIntRange : CustomRange
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
            public override float[] GetValues()
            {
                List<float> list = [];
                for (float i = min; i <= max; i += step)
                {
                    list.Add(i);
                }
                foreach (var item in addtional)
                {
                    list.Add(item.GetValueFromSelector());
                }
                return list.ToArray();
            }


            int max;
            int min;
            int step;
            List<OSAS> addtional;
            public CustomIntRange(int min, int max, int step = 1, List<OSAS> selectors = null)
            {
                this.min = min;
                this.max = max;
                this.step = step;
                if (selectors != null)
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
        public class CustomSelectionRange : CustomRange
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
            public override float[] GetValues()
            {
                List<float> list = [];
                foreach (var item in addtional)
                {
                    list.Add(item.GetValueFromSelector());
                }
                return [.. list];
            }


            List<OSAS> addtional;
            public CustomSelectionRange(List<OSAS> selectors)
            {
                this.addtional = selectors;
                addtional.Sort();
            }
        }
        public class CustomBoolRange : CustomRange
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
            public override float[] GetValues()
            {
                List<float> list = [];
                foreach (var item in addtional)
                {
                    list.Add(item.GetValueFromSelector());
                }
                return [.. list];
            }


            List<OSAS> addtional = [OSAS.on, OSAS.off];
            public CustomBoolRange()
            {
                addtional.Sort();
            }
        }



    }
}
