using System;
using UnityEngine;
using AmongUs.GameOptions;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace TheSpaceRoles
{
    public class Seer : CustomRole
    {
        public Seer()
        {
            Team = Teams.Crewmate;
            Role = Roles.Seer;
            Color = Helper.ColorFromColorcode("#61b26c");
        }
        public override void Murder(PlayerControl pc, PlayerControl target)
        {
            if(target!=PlayerControl.LocalPlayer&&!PlayerControl.LocalPlayer.CachedPlayerData.IsDead)
            FlashPatch.ShowFlash(Helper.ColorFromColorcode("#00ff00"));
            
        }
    }
}
