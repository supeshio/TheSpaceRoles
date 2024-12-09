using System;
using UnityEngine;
using AmongUs.GameOptions;
using System.Threading.Tasks;

namespace TheSpaceRoles
{
    public class Bait : CustomRole
    {
        public Bait()
        {
            Team = Teams.Crewmate;
            Role = Roles.Bait;
            Color = Helper.ColorFromColorcode("#4169e1");
        }
        public override void Murder(PlayerControl pc, PlayerControl target)
        {
            if (target.PlayerId == PlayerId)
            {
                LateTask.AddTask(1f,()=> { pc.CmdReportDeadBody(target.Data);Logger.Info("Report "+ $"{pc.PlayerId} - {target.PlayerId}"); });
            }
        }
    }
}
