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
                List<MeetingHud.VoterState> voterStates = [];

                DataBase.AllPlayerRoles[PlayerControl.LocalPlayer.PlayerId].Do(x => voterStates.AddRange(x.VotingResultChange(__instance, ref list)));

                list.AddRange([.. voterStates]);
                list = [.. list.OrderBy(x => x.VoterId)];


                byte frequentry = list.Where(x=>x.VotedForId!=252&&x.VotedForId!=254&&x.VotedForId!=255).Select(x=>x.VotedForId).ToList().MaxFrequency(out bool tie);
                NetworkedPlayerInfo exiled = GameData.Instance.AllPlayers.ToArray().FirstOrDefault(v => !tie && v.PlayerId == list.First(x=>x.VoterId==frequentry).VoterId && !v.IsDead);
                Helper.AddChat($"VoterList:\n{list.Join(x=>$"Voter:{x.VoterId},VotedFor:{x.VotedForId}",",\n")}\n,Exiled:{exiled?.PlayerName},Tie:{tie}");
                Helper.AddChat($"Dictionary:\n{list.Where(x => x.VotedForId != 252 && x.VotedForId != 254 && x.VotedForId != 255).Join(x => $"{x.VotedForId}", ",")}");
                __instance.RpcVotingComplete(list.ToArray(), exiled, tie);
                foreach (var item in list)
                {
                    Logger.Message($"Player:{item.VoterId} : {item.VotedForId} ","Voter");
                }
            }
            return false;

        }
        //[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
        class MeetingHudPopulateVotesPatch
        {

            static bool Prefix(MeetingHud __instance, [HarmonyArgument(0)]ref Il2CppStructArray<MeetingHud.VoterState> states)
            {
                // Swapper swap
                //PlayerVoteArea swapped1 = null;
                //PlayerVoteArea swapped2 = null;

                //foreach (PlayerVoteArea playerVoteArea in __instance.playerStates)
                //{
                //    if (playerVoteArea.TargetPlayerId == Swapper.playerId1) swapped1 = playerVoteArea;
                //    if (playerVoteArea.TargetPlayerId == Swapper.playerId2) swapped2 = playerVoteArea;
                //}
                //bool doSwap = animateSwap && swapped1 != null && swapped2 != null && Swapper.swapper != null && !Swapper.swapper.Data.IsDead;
                //if (doSwap)
                //{
                //    __instance.StartCoroutine(Effects.Slide3D(swapped1.transform, swapped1.transform.localPosition, swapped2.transform.localPosition, 1.5f));
                //    __instance.StartCoroutine(Effects.Slide3D(swapped2.transform, swapped2.transform.localPosition, swapped1.transform.localPosition, 1.5f));

                //    Swapper.numSwaps--;
                //}
                //foreach (var item in PlayerControl.AllPlayerControls)
                //{
                //    DataBase.AllPlayerRoles[item.PlayerId][0].VotingResult(__instance, ref states);
                //}


                //__instance.TitleText.text = DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.MeetingVotingResults, new Il2CppReferenceArray<Il2CppSystem.Object>(0));
                //int num = 0;
                //for (int i = 0; i < __instance.playerStates.Length; i++)
                //{
                //    PlayerVoteArea playerVoteArea = __instance.playerStates[i];
                //    byte targetPlayerId = playerVoteArea.TargetPlayerId;
                //    // Swapper change playerVoteArea that gets the votes

                //    playerVoteArea.ClearForResults();
                //    int num2 = 0;



                //    //bool mayorFirstVoteDisplayed = false;
                //    Dictionary<int, int> votesApplied = new Dictionary<int, int>();
                //    for (int j = 0; j < states.Length; j++)
                //    {
                //        MeetingHud.VoterState voterState = states[j];
                //        PlayerControl voter = Helper.GetPlayerById(voterState.VoterId);
                //        if (voter == null) continue;

                //        NetworkedPlayerInfo playerById = GameData.Instance.GetPlayerById(voterState.VoterId);
                //        if (playerById == null)
                //        {
                //            Logger.Error($"Couldn't find player info for voter: {voterState.VoterId}");
                //        }
                //        else if (i == 0 && voterState.SkippedVote && !playerById.IsDead)
                //        {
                //            __instance.BloopAVoteIcon(playerById, num, __instance.SkippedVoting.transform);
                //            num++;
                //        }
                //        else if (voterState.VotedForId == targetPlayerId && !playerById.IsDead)
                //        {
                //            __instance.BloopAVoteIcon(playerById, num2, playerVoteArea.transform);
                //            num2++;
                //        }

                //        if (!votesApplied.ContainsKey(voter.PlayerId))
                //            votesApplied[voter.PlayerId] = 0;

                //        votesApplied[voter.PlayerId]++;

                //        // Major vote, redo this iteration to place a second vote
                //    }
                //}

                return false;
            }
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
