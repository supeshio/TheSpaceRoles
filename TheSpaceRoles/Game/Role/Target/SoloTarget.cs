using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSR.Game.Role.Target
{
    public class ClassicSoloTarget : PlayerTargetBase
    {
        private Func<List<FPlayerControl>> getAllPlayers;
        public ClassicSoloTarget(Func<List<FPlayerControl>> func)
        {
            getAllPlayers = func;
        }
        public override List<FPlayerControl> AllPlayers()
        {
            var p = getAllPlayers();
            //Logger.Info(p.Count.ToString(),"count");
            return p;
        }

        public override void SetTarget()
        {
            var k = GetPlayersInAbilityRangeSorted(false);
            //Logger.Info(k.Count.ToString(),"cout");
            if (k.Count > 0)
            {
                k = [k.ElementAt(0)];
            }
            else
            {
                k= [];
            }
            currentTarget = k;
            base.SetTarget();
        }
    }
}
