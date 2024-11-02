using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using static TheSpaceRoles.CustomOption;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class Mayor : CustomRole
    {
        public Mayor()
        {
            team = Teams.Crewmate;
            Role = Roles.Mayor;
            Color = Helper.ColorFromColorcode("#7f921a");
        }
        public Func<string>[] Count()
        {
            Func<string>[] f = [];
            for (int i = 1; i < 20; i++)
            {
                f.AddItem(() => i.ToString());
            }
            return f;
        }
        public static CustomOption VoteCount;
        public override void OptionCreate()
        {
            VoteCount = Create(OptionType.Crewmate, "role.mayor.VotingCount", new CustomIntRange(2, 15, 1), 1);
            Options = [VoteCount];

        }
        public override void VotingResultChange(MeetingHud meeting, ref List<MeetingHud.VoterState> states)
        {
            var v = states.First(x => x.VoterId == PlayerId);
            Logger.Message(v.VoterId.ToString(), "MayorVote");
            for (int i = 0; i < VoteCount.GetIntValue(); i++)
            {
                states.Add(v);

            }
        }
    }

}
