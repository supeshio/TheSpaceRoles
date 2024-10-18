using HarmonyLib;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Rewired.Internal;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    [HarmonyPatch(typeof(MeetingHud))]
    public static class MeetingHudVote
    {
        [HarmonyPatch(nameof(MeetingHud.CheckForEndVoting)), HarmonyPrefix]
        private static bool CheckForVoting(MeetingHud __instance)
        {
            if (__instance.playerStates.All((PlayerVoteArea ps) => ps.AmDead || ps.DidVote))
            {
                //List<MeetingHud.VoterState> dictionary = [];
                //DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.CheckForEndVoting(__instance, ref dictionary));
                List<MeetingHud.VoterState> list = [];
                for (int i = 0; i < __instance.playerStates.Length; i++)
                {
                    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                    byte votedFor = playerVoteArea.VotedFor;
                    //if (votedFor != 252 && votedFor != 255 && votedFor != 254)
                    //{
                    //    dictionary.Add(new MeetingHud.VoterState
                    //    {
                    //        VoterId = playerVoteArea.TargetPlayerId,
                    //        VotedForId = playerVoteArea.VotedFor
                    //    });
                    //}
                    list.Add( new MeetingHud.VoterState
                    {
                        VoterId = playerVoteArea.TargetPlayerId,
                        VotedForId = playerVoteArea.VotedFor
                    });
                }
                foreach (var item in DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId])
                {
                    item.VotingResultChange(__instance, ref list);
                }
                foreach (var item in DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId])
                {
                    item.VotingResultChangePost(__instance, ref list);
                }

                list = [.. list.OrderBy(x => x.VoterId)];


                byte frequentry = list.Where(x=>x.VotedForId!=252&&x.VotedForId!=254&&x.VotedForId!=255).Select(x=>x.VotedForId).ToList().MaxFrequency(out bool tie);
                NetworkedPlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => !tie && v.PlayerId == list.First(x=>x.VoterId==frequentry).VoterId && !v.IsDead);
                //Helper.AddChat($"VoterList:\n{list.Join(x=>$"Voter:{x.VoterId},VotedFor:{x.VotedForId}",",\n")}\n,Exiled:{exiled?.PlayerName},Tie:{tie}");
                //Helper.AddChat($"Dictionary:\n{list.Where(x => x.VotedForId != 252 && x.VotedForId != 254 && x.VotedForId != 255).Join(x => $"{x.VotedForId}", ",")}");
                __instance.RpcVotingComplete(list.ToArray(), exiled, tie);
                //foreach (var item in list)
                //{
                //    Logger.Message($"Player:{item.VoterId} : {item.VotedForId} ","Voter");
                //}
            }
            return false;

        }
        [HarmonyPatch(nameof(MeetingHud.Start)), HarmonyPostfix]
        private static void Start(MeetingHud __instance)
        {
            DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.MeetingStart(__instance));

        }
        [HarmonyPatch(nameof(MeetingHud.Update)), HarmonyPostfix]
        private static void Update(MeetingHud __instance)
        {
            DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.MeetingUpdate(__instance));

        }
        [HarmonyPatch(nameof(MeetingHud.CoStartCutscene)), HarmonyPostfix]
        private static void CustScene(MeetingHud __instance)
        {
            DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => x.BeforeMeetingStart(__instance));

        }
    }
}
