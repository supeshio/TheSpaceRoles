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
        killcool_default,
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
                OSAS.killdistance_default => GameOptionsManager.Instance.currentNormalGameOptions.KillDistance,
                OSAS.on => -1,
                OSAS.off => -2,
                OSAS.killcool_default => GameOptionsManager.Instance.currentNormalGameOptions.KillCooldown,
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
                OSAS.killcool_default => Translation.GetString($"option.selection.killdistance.default"),
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
                foreach (var item in addtional)
                {
                    list.Add(item.GetStringFromSelector());
                }
                for (float i = min; i <= max; i += step)
                {
                    list.Add(i.ToString());
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
                for (float i = min; i <= max; i += step)
                {
                    list.Add(i);
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
        /// <summary>
        /// killcooldownのrange｡
        /// 12=(30s)
        /// </summary>
        /// <param name="selectors"></param>
        /// <returns></returns>
        public static CustomFloatRange CustomCoolDownRangefloat(List<OSAS> selectors = null)
        {

            float min = 2.5f;
            float max = 180f;
            float step = 2.5f;
            List<OSAS> s = selectors ?? [];
            s.Add(OSAS.killcool_default);
            return new CustomFloatRange(min, max, step, s);
        }


    }
}
