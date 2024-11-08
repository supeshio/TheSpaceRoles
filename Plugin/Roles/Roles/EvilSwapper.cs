using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TheSpaceRoles.Ranges;

namespace TheSpaceRoles
{
    public class EvilSwapper : CustomRole
    {
        public static List<Target> targets = [];
        public PlayerControl SwapPC1;
        public PlayerControl SwapPC2;
        public EvilSwapper()
        {
            Team = Teams.Impostor;
            Role = Roles.EvilSwapper;
            Color = Palette.ImpostorRed;
        }
        public static CustomOption SwapCount;
        public override void OptionCreate()
        {
            if (SwapCount != null) return;

            SwapCount = CustomOption.Create(CustomOption.OptionType.Impostor,"role.evilswapper.swapcount", new CustomIntRange(1, 15, 1), 2);

            Options = [SwapCount];
        }
        public override void HudManagerStart(HudManager hudManager)
        {
            targets = [];
        }
        public override void MeetingStart(MeetingHud meeting)
        {
            SwapPC1 = SwapPC2 = null;
            TargetReset(meeting);
        }
        public override void VotingResultChangePost(MeetingHud meeting, ref List<MeetingHud.VoterState> states)
        {
            Logger.Info("Swap");
            if (SwapPC1 != null && SwapPC2 != null)
            {
                Logger.Info($"{SwapPC1.Data.PlayerName}({SwapPC1.PlayerId})<=>{SwapPC2.Data.PlayerName}({SwapPC2})");
                for (int i = 0; i < states.Count; i++)
                {
                    var state = states[i];
                    if (state.VotedForId == SwapPC1.PlayerId)
                    {
                        state.VotedForId = SwapPC2.PlayerId;
                    }
                    else
                        if (state.VotedForId == SwapPC2.PlayerId)
                    {
                        state.VotedForId = SwapPC1.PlayerId;
                    }
                    states[i] = state;
                }
            }
        }
        public void TargetReset(MeetingHud meeting, int[] untargetingplayerids = null)
        {
            if (targets != null | targets.Count > 0)
            {
                foreach (var target in targets)
                {
                    GameObject.Destroy(target.renderer.gameObject);
                }
            }
            targets = [];
            if (!PlayerControl.LocalPlayer.Data.IsDead)
            {

                foreach (var player in MeetingHud.Instance.playerStates)
                {
                    if (player != null && !player.AmDead && player.TargetPlayerId != PlayerControl.LocalPlayer.PlayerId)
                    {
                        if (untargetingplayerids != null)
                        {
                            if (!untargetingplayerids.Contains(player.TargetPlayerId))
                            {

                                targets.Add(new Target(player, meeting, this));
                            }
                        }
                        else
                        {

                            targets.Add(new Target(player, meeting, this));
                        }
                    }
                }
            }
            foreach (var player in MeetingHud.Instance.playerStates)
            {
                Logger.Message($"player:{player.NameText.text},Dead:{player.AmDead},null{player == null}");
            }
        }
        public void PrepareTargetPlayerSwap()
        {
            if (SwapPC1 == null)
            {
                Logger.Info($"SwapPC1={targetplayer.Data.PlayerName}");
                SwapPC1 = targetplayer;
                targets.DoIf(x => x.playerId == targetplayer.PlayerId, x => x.gameObject.SetActive(false));
            }
            else if (SwapPC2 == null)
            {
                Logger.Info($"SwapPC2={targetplayer.Data.PlayerName}");
                SwapPC2 = targetplayer;
                targets.Do(x => x.gameObject.SetActive(false));
                var m = CustomRPC.SendRpcUsebility(Roles.EvilSwapper, PlayerControl.LocalPlayer.PlayerId, 0);
                m.Write(SwapPC1.PlayerId);
                m.Write(SwapPC2.PlayerId);
            }
        }
        public static void RpcSwap(int playerid, int id1, int id2)
        {
            var swap = (EvilSwapper)DataBase.AllPlayerRoles[playerid];
            swap.SwapPC1 = Helper.GetPlayerById(id1);
            swap.SwapPC2 = Helper.GetPlayerById(id2);

        }
        public static PlayerControl targetplayer;

        public class Target
        {
            public GameObject gameObject;
            public PlayerVoteArea voteArea;
            public int playerId;
            public PassiveButton passiveButton;
            public SpriteRenderer renderer;
            public Target(PlayerVoteArea playerVoteArea, MeetingHud meeting, EvilSwapper swapper)
            {
                this.voteArea = playerVoteArea;
                this.playerId = playerVoteArea.TargetPlayerId;
                this.passiveButton = new PassiveButton();
                this.gameObject = new();
                gameObject.name = "SwapButton";
                gameObject.transform.SetParent(playerVoteArea.transform);
                gameObject.transform.localPosition = new(-0.9f, 0, -1);
                this.renderer = gameObject.AddComponent<SpriteRenderer>();
                renderer.sprite = Sprites.GetSpriteFromResources("ui.swap.png", 800f);
                renderer.gameObject.layer = HudManager.Instance.gameObject.layer;
                //renderer.material = MeetingHud.Instance.SkipVoteButton.GetComponent<SpriteRenderer>().material;

                renderer.color = Helper.ColorEditHSV(Color.white, a: 0.6f);
                var box = gameObject.AddComponent<BoxCollider2D>();
                box.size = renderer.bounds.size;
                passiveButton = gameObject.AddComponent<PassiveButton>();
                passiveButton.OnClick = new();
                passiveButton.OnMouseOut = new();
                passiveButton.OnMouseOver = new();
                passiveButton._CachedZ_k__BackingField = 0.1f;
                passiveButton.CachedZ = 0.1f;
                passiveButton.Colliders = new[] { box };
                passiveButton.OnClick.AddListener((System.Action)(() =>
                {
                    Logger.Info($"{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).Data.PlayerName} is targeting");
                    targetplayer = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId);
                    swapper.PrepareTargetPlayerSwap();

                }));
                passiveButton.OnMouseOver.AddListener((System.Action)(() =>
                {
                    renderer.color = Color.red;
                }));
                passiveButton.OnMouseOut.AddListener((System.Action)(() =>
                {

                    renderer.color = Helper.ColorEditHSV(Color.white, a: 0.6f);
                }));
            }
        }
    }
}
