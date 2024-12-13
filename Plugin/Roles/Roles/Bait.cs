using System;
using UnityEngine;
using AmongUs.GameOptions;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

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
                LateTask.AddTask(1f,()=> { pc.CmdReportDeadBody(target.Data);
                Logger.Info("BaitReport "+ $"{pc.PlayerId} - {target.PlayerId}", "Bait"); });
            }
        }
        public override void Update()
        {
            if (PlayerControl.LocalPlayer.PlayerId == PlayerId)
            {
                // Bait Vents (From TOR系)
                if (ShipStatus.Instance?.AllVents != null)
                {
                    var ventsWithPlayers = new List<int>();
                    foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                    {
                        if (player == null) continue;

                        if (player.inVent)
                        {
                            Vent target = ShipStatus.Instance.AllVents.OrderBy(x => Vector2.Distance(x.transform.position, player.GetTruePosition())).FirstOrDefault();
                            if (target != null) ventsWithPlayers.Add(target.Id);
                        }
                    }

                    foreach (Vent vent in ShipStatus.Instance.AllVents)
                    {
                        if (vent.myRend == null || vent.myRend.material == null) continue;
                        if (ventsWithPlayers.Contains(vent.Id) || (ventsWithPlayers.Count > 0))
                        {
                            vent.myRend.material.SetFloat("_Outline", 1f);
                            vent.myRend.material.SetColor("_OutlineColor", Color.yellow);
                        }
                        else
                        {
                            vent.myRend.material.SetFloat("_Outline", 0);
                        }
                    }
                }

            }

        }
    }
}
