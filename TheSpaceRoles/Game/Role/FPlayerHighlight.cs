using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TSR.Game.Role
{
    public class PlayerTargetHighlight(Func<Color> color)
    {
        private Func<Color> Color { get; set; } = color;

        public void SetHighlight(List<FPlayerControl> target)
        {
            if (FPlayerControl.LocalPlayer.IsNull || FPlayerControl.LocalPlayer.NPI == null)
            {
                return;
            }
            target.DoIf(x => !x.IsNull&&!x.IsLocalPlayer, x => LightOn(x));
            FPlayerControl.AllPlayer.DoIf(x => !x.IsNull && !target.Contains(x) && !x.IsLocalPlayer, x => LightOff(x));
            //base.SetDisabled();
            //button
        }
        private void LightOff(FPlayerControl target)
        {
            target.ToggleHighlight(false, Color?.Invoke());
        }
        private void LightOn(FPlayerControl target)
        {
            target.ToggleHighlight(true, Color?.Invoke());
        }
    }
}
