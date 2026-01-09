using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSR.Game
{
    public class CustomDeathReason
    {
        public CustomDeathReason(string reason) { this.reason = reason; }
        public string reason { private set; get; }
    }
}
