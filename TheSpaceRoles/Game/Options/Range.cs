using System.Collections.Generic;

namespace TSR.Game.Options
{
    public class DecimalRange(decimal Min = 0, decimal Max = 15, decimal Step = 1, decimal Default = 0)
    {
        public decimal Min = Min;
        public decimal Max = Max;
        public decimal Step = Step;
        public decimal Default = Default;

        public List<decimal> Get()
        {
            List<decimal> list = [];
            for (decimal i = Min; i <= Max; i += Step)
            {
                list.Add(i);
            }
            return list;
        }
    }
}