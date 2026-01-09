using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSR.Game.Role.Ability
{
    public class AbilityRange
    {
        public static AbilityRange Inf = new AbilityRange(float.MaxValue);
        public enum Range : uint
        {
            None=0,
            VeryShort,
            Short,
            Medium,
            Long,
            Inf,
            Num,
        }
        public static float GetRange(Range range)
        {
            switch (range)
            {
                case Range.None:
                    return 0f;
                case Range.VeryShort:
                    return 0.5f;
                case Range.Short:
                    return 1f;
                case Range.Medium:
                    return 1.8f;
                case Range.Long:
                    return 2.5f;
            }
            return 0.5f;
        }
        public static string GetRangeText(Range range)
        {
            switch (range)
            {
                case Range.None:
                    return Translation.Get("ability.range.none");
                case Range.VeryShort:
                    return Translation.Get("ability.range.veryshort");
                case Range.Short:
                    return Translation.Get("ability.range.short");
                case Range.Medium:
                    return Translation.Get("ability.range.medium");
                case Range.Long:
                    return Translation.Get("ability.range.long");
                case Range.Inf:
                    return Translation.Get("ability.range.inf");
            }
            return Translation.Get("ability.range.unknown");
        }
        public AbilityRange(Range range)
        {
            ARange = range;
            Distance = GetRange(range);
        }
        public AbilityRange(float x)
        {
            ARange = Range.Num;
            Distance = x;
        }
        public Range ARange { get; }
        public float Distance { get; }
    }
}
