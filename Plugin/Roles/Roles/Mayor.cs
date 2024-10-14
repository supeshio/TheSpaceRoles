using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Rewired.Dev;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.ProBuilder;
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
            VoteCount = Create(OptionType.Crewmate, "role.mayor.VotingCount", new CustomIntRange(2,15,1), 1);
            Options = [VoteCount];

        }
        //public override void CheckForEndVoting(MeetingHud meeting, ref Dictionary<byte, int> dictionary)
        //{
        //    var p =
        //    meeting.playerStates.First(x => x.TargetPlayerId == PlayerControl.LocalPlayer.PlayerId);

        //    byte votedFor = p.VotedFor;
        //    if (votedFor != 252 && votedFor != 255 && votedFor != 254)
        //    {
        //        PlayerControl pc = Helper.GetPlayerById(PlayerControl.LocalPlayer.PlayerId);
        //        if (pc == null || pc.Data == null || pc.Data.IsDead || pc.Data.Disconnected)
        //        {

        //        }
        //        else
        //        {
        //            dictionary[votedFor] += VoteCount.GetIntValue()-1;

        //        }

        //    }
        //}
        public override MeetingHud.VoterState[] VotingResultChange(MeetingHud meeting, ref List<MeetingHud.VoterState> states)
        {
            var v = states.First(x => x.VoterId == PlayerId);
            Logger.Message(v.VoterId.ToString(),"MayorVote");
            return [new MeetingHud.VoterState() { VoterId = v.VoterId, VotedForId = v.VotedForId }];
        }
    }

}
