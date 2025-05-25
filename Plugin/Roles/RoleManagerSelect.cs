using AmongUs.GameOptions;
using AmongUs.InnerNet.GameDataMessages;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Rewired.Glyphs.UnityUI.UnityUITextMeshProGlyphHelper;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class RoleSelect
    {
        [HarmonyPatch(typeof(GameManager))]
        [HarmonyPatch(nameof(GameManager.StartGame)), HarmonyPostfix]
        public static void Reset()
        {
            if (AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay)
            {
                DataBase.ResetAndPrepare();
                Logger.Info("FreePlayStart");
                foreach (var pc in PlayerControl.AllPlayerControls)
                {
                    SendRpcSetRole(Roles.Crewmate, pc.PlayerId);
                }
            }
        }

        [HarmonyPatch(typeof(RoleManager))]
        [HarmonyPatch(nameof(RoleManager.SelectRoles)), HarmonyPrefix]
        public static void SelectRolesPatch() {
            GameOptionsManager.Instance.currentNormalGameOptions.NumImpostors = 1;
        }
        [HarmonyPatch(typeof(RoleManager))]
        [HarmonyPatch(nameof(RoleManager.SelectRoles)), HarmonyPostfix]

        public static void Select()
        {
            AmongUsClient.Instance.FinishRpcImmediately(CustomRPC.SendRpc(Rpcs.DataBaseReset));
            DataBase.ResetAndPrepare();
            DataBase.RoleSelect(GameData.Instance.PlayerCount);

            if (AmongUsClient.Instance.AmHost)
            {
                Logger.Info("I am Host.");
                Dictionary<Teams, List<Roles>> RolesInSettings = [];

                foreach ((var role, var count) in CustomOptionsHolder.RoleOptions_Count)
                {
                    int c = count.GetIntValue();
                    if (!RolesInSettings.ContainsKey(Helper.GetCustomRole(role).Team))
                    {
                        RolesInSettings.Add(Helper.GetCustomRole(role).Team, []);
                    }
                    for (int i = 0; i < c; i++)
                    {
                        RolesInSettings[Helper.GetCustomRole(role).Team].Add(role);
                    }
                }

                Dictionary<Teams, int> TeamPlayerConuts = [];
                foreach ((var team, var count) in CustomOptionsHolder.TeamOptions_Count)
                {
                    TeamPlayerConuts.Add(team, count.GetIntValue());
                }
                TeamPlayerConuts.Add(Teams.Crewmate, DataBase.GamePlayerCount - TeamPlayerConuts.Values.Sum());

                if (GameOptionsManager.Instance.CurrentGameOptions.NumImpostors > TeamPlayerConuts[Teams.Impostor])
                {
                    List<PlayerControl> Impostors = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.RoleType == RoleTypes.Impostor).ToList();
                    for (int i = 0; i > GameOptionsManager.Instance.CurrentGameOptions.NumImpostors - TeamPlayerConuts[Teams.Impostor]; i++)
                    {
                        int rn = RandomNext(Impostors.Count);
                        MessageWriter writer = CustomRPC.SendRpc(Rpcs.SetNativeRole);
                        writer.Write((byte)Impostors[rn].PlayerId);
                        writer.Write((byte)RoleTypes.Crewmate);
                        writer.EndRpc();
                        RpcReader.SetNativeRole(Impostors[rn].PlayerId, RoleTypes.Crewmate);

                        Impostors.RemoveAt(rn);

                    }
                }
                else if (GameOptionsManager.Instance.CurrentGameOptions.NumImpostors < TeamPlayerConuts[Teams.Impostor])
                {

                    List<PlayerControl> Crewmates = PlayerControl.AllPlayerControls.ToArray().Where(x => x.Data.RoleType == RoleTypes.Crewmate).ToList();
                    for (int i = 0; i > Mathf.Abs(GameOptionsManager.Instance.CurrentGameOptions.NumImpostors - TeamPlayerConuts[Teams.Impostor]); i++)
                    {
                        int rn = RandomNext(Crewmates.Count);
                        MessageWriter writer = CustomRPC.SendRpc(Rpcs.SetNativeRole);
                        writer.Write((byte)Crewmates[rn].PlayerId);
                        writer.Write((byte)RoleTypes.Impostor);
                        writer.EndRpc();
                        RpcReader.SetNativeRole(Crewmates[rn].PlayerId, RoleTypes.Crewmate);

                        Crewmates.RemoveAt(rn);
                    }
                }

                List<Roles> RoleList = [];
                List<PlayerControl> pl = PlayerControl.AllPlayerControls.ToArray().ToList();

                void setrole(Roles role)
                {
                    if (pl.Count == 0)
                    {
                        Logger.Info("SettingRoles is too much");
                    }
                    else
                    {
                        int rn = RandomNext(pl.Count);
                        SendRpcSetRole(role, pl[rn].PlayerId);
                        pl.RemoveAt(rn);
                        RoleList.Add(role);

                    }
                }
                //インポスターとクルーメイトは先にやる

                var counth = TeamPlayerConuts[Teams.Impostor];
                Teams teamh = Teams.Impostor;
                for (int i = 0; i < counth; i++)
                {
                    List<Roles> role = RolesInSettings[teamh];
                    if (role.Count > 0)
                    {
                        int rn = RandomNext(role.Count);
                        setrole(role[rn]);
                        RolesInSettings[teamh].RemoveAt(rn);
                    }
                    else
                    {
                        setrole(RoleData.GetCustomRole_NormalFromTeam(teamh).Role);
                    }

                }

                counth = TeamPlayerConuts[Teams.Crewmate];
                teamh = Teams.Crewmate;
                for (int i = 0; i < counth; i++)
                {
                    List<Roles> role = RolesInSettings[teamh];
                    if (role.Count > 0)
                    {
                        int rn = RandomNext(role.Count);
                        setrole(role[rn]);
                        RolesInSettings[teamh].RemoveAt(rn);
                    }
                    else
                    {
                        setrole(RoleData.GetCustomRole_NormalFromTeam(teamh).Role);
                    }

                }

                foreach ((var team,var count) in TeamPlayerConuts)
                {
                    if(team == Teams.Crewmate || team == Teams.Impostor)continue;
                    for (int i = 0; i < count; i++) {
                        List<Roles> role = RolesInSettings[team];
                        if (role.Count > 0)
                        {
                            int rn = RandomNext(role.Count);
                            setrole(role[rn]);
                            RolesInSettings[team].RemoveAt(rn);
                        }
                        else
                        {
                            setrole(RoleData.GetCustomRole_NormalFromTeam(teamh).Role);
                        }

                    }
                    
                }
                //foreach (var role in RoleList)
                //{
                //    int rn = RandomNext(pl.Count);
                //    SendRpcSetRole(role, pl[rn].PlayerId);
                //    pl.RemoveAt(rn);
                //    if (pl.Count == 0)
                //    {
                //        Logger.Info("SettingRoles is too much");
                //        break;
                //    }
                //}


                //Dictionary<Teams, List<Roles>> tr = [];


                //List<int> players = DataBase.AllPlayerControls().Select(x => (int)x.PlayerId).ToList();

                //Logger.Info(players.Count.ToString());
                //foreach (var keyValuePair in CustomOptionsHolder.RoleOptions_Count)
                //{
                //    var role = keyValuePair.Key;
                //    var customOption = keyValuePair.Value.Selection();
                //    Logger.Info(role + $" is {customOption}");

                //    if (!role.GetCustomRole().canAssign) continue;


                //    for (int i = 0; i < customOption; i++)

                //    {
                //        if (tr.ContainsKey(RoleData.GetCustomRoleFromRole(role).Team))
                //        {

                //            tr[RoleData.GetCustomRoleFromRole(role).Team].Add(RoleData.GetCustomRoleFromRole(role).Role);
                //        }
                //        else
                //        {
                //            tr.Add(RoleData.GetCustomRoleFromRole(role).Team, [RoleData.GetCustomRoleFromRole(role).Role]);

                //        }
                //        Logger.Info($"{customOption}");
                //    }


                //}
                //if (GameOptionsManager.Instance.currentGameOptions.NumImpostors > CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].Selection())
                //{
                //    for (int i = 0; i < DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).Count() - CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].Selection(); i++)
                //    {

                //        var k = DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).ToList();
                //        k[RandomNext(k.Count)].RpcSetRole(AmongUs.GameOptions.RoleTypes.Crewmate);
                //    }
                //}
                //else if (GameOptionsManager.Instance.currentGameOptions.NumImpostors < CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].Selection())
                //{
                //    for (int i = 0; i < CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].Selection() - DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).Count(); i++)
                //    {

                //        var k = DataBase.AllPlayerControls().Where(x => x.Data.RoleType != AmongUs.GameOptions.RoleTypes.Impostor).ToList();
                //        k[RandomNext(k.Count)].RpcSetRole(AmongUs.GameOptions.RoleTypes.Impostor);
                //    }
                //}
                //Logger.Fatal(tr.Join(x => x.Key.ToString()));

                //List<int> p = DataBase.AllPlayerControls().Where(x => x.Data.RoleType == RoleTypes.Impostor).Select(x => (int)x.PlayerId).ToList();
                //var team = Teams.Impostor;
                //var teammembers = CustomOptionsHolder.TeamOptions_Count[team].Selection();
                //List<Roles> v = [];
                //if (tr.ContainsKey(team))
                //{

                //    v = tr[team];
                //}
                //for (int i = 0; i < teammembers; i++)
                //{
                //    if (p.Count == 0) continue;
                //    Logger.Info($"{team},{teammembers},{v.Count}");
                //    if (v.Count == 0)
                //    {

                //        var playertag = RandomNext(p.Count);
                //        SendRpcSetRole(RoleData.GetCustomRole_NormalFromTeam(team).Role, p[playertag]);
                //        Logger.Info(p[playertag].ToString() + $" is {RoleData.GetCustomRole_NormalFromTeam(team).Role}");
                //        players.Remove(p[playertag]);
                //    }
                //    else
                //    {
                //        var r = Helper.RandomNext(v.Count);
                //        var role = v[r];
                //        var playertag = RandomNext(p.Count);
                //        SendRpcSetRole(role, p[playertag]);
                //        Logger.Info(p[playertag].ToString() + $" is {role}");
                //        players.Remove(p[playertag]);
                //        v.RemoveAt(r);

                //    }
                //}
                //Logger.Info("playercount" + players.Count.ToString());
                //foreach (var t in tr)
                //{
                //    Logger.Info($"{t.Key}:{string.Join(",", t.Value.Select(x => x.ToString()))}");
                //}
                //foreach (var item in tr)
                //{
                //    team = item.Key;
                //    if (team == Teams.Crewmate) continue;
                //    if (team == Teams.Impostor) continue;
                //    teammembers = CustomOptionsHolder.TeamOptions_Count[team].Selection();
                //    v = item.Value;
                //    for (int i = 0; i < teammembers; i++)
                //    {
                //        if (players.Count == 0) continue;
                //        Logger.Info($"{team},{teammembers},{v.Count}");
                //        if (v.Count == 0)
                //        {

                //            var player = RandomNext(players.Count);
                //            SendRpcSetRole(RoleData.GetCustomRole_NormalFromTeam(team).Role, players[player]);
                //            Logger.Info(players[player].ToString() + $" is {RoleData.GetCustomRole_NormalFromTeam(team).Role}");
                //            players.RemoveAt(player);
                //        }
                //        else
                //        {
                //            var r = Helper.RandomNext(v.Count);
                //            var role = v[r];
                //            var player = RandomNext(players.Count);
                //            SendRpcSetRole(role, players[player]);
                //            Logger.Info(players[player].ToString() + $" is {role}");
                //            players.RemoveAt(player);
                //            v.RemoveAt(r);

                //        }
                //    }
                //}
                //Logger.Info("playercount" + players.Count.ToString());
                //team = Teams.Crewmate;
                //teammembers = players.Count;
                //v = tr[team];
                //while (players.Count != 0)
                //{
                //    Logger.Info($"{team},{teammembers},{v.Count}");
                //    if (v.Count == 0)
                //    {

                //        var playertag = RandomNext(players.Count);
                //        SendRpcSetRole(RoleData.GetCustomRole_NormalFromTeam(team).Role, players[playertag]);
                //        Logger.Info(players[playertag].ToString() + $" is {RoleData.GetCustomRole_NormalFromTeam(team).Role}");
                //        players.RemoveAt(playertag);
                //    }
                //    else
                //    {
                //        var r = Helper.RandomNext(v.Count);
                //        var role = v[r];
                //        var playertag = RandomNext(players.Count);
                //        SendRpcSetRole(role, players[playertag]);
                //        Logger.Info(players[playertag].ToString() + $" is {role}");
                //        players.RemoveAt(playertag);
                //        v.RemoveAt(r);

                //    }
                //}
            }
            else
            {

                Logger.Info("I am not Host.");
            }
        }
        public static int SendRpcSetRole(Roles role, int playerId)
        {
            //設定作ってないけどここでほんとは分岐
            //ここではmainとして扱う
            SetRole(playerId, (int)role);

            //Rpc
            var writer = CustomRPC.SendRpc(Rpcs.SetRole);
            writer.Write(playerId);
            writer.Write((int)role);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return playerId;
        }
        public static void SetRole(int playerId, int roleId)
        {

            Logger.Info($"Player:{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text}({playerId}) -> Role:{(Roles)roleId}");


            //if (DataBase.AllPlayerRoles.ContainsKey(playerId))
            //{
            //    var list = DataBase.AllPlayerRoles[playerId].ToList();
            //    var pl = RoleData.GetCustomRoleFromRole((Roles)roleId);
            //    pl.ReSet(playerId);
            //    pl.CustomTeam.Role = pl;
            //    list.Add(pl);
            //    DataBase.AllPlayerRoles[playerId] = [.. list];
            //}
            //else
            //{
            var p = RoleData.GetCustomRoleFromRole((Roles)roleId);
            p.ReSet(playerId);
            if (DataBase.AllPlayerData.ContainsKey(playerId))
            {
                DataBase.AllPlayerData.Remove(playerId);
            }
            DataBase.AllPlayerData.Add(playerId, new(GetPlayerById(playerId), p));
            //}

        }
        public static void ChangeMainRole(int playerId, int roleId)
        {

            Logger.Info($"Player:{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).Data.PlayerName}({playerId}) -> Role:{(Roles)roleId}", "ChangeRole");
            var p = RoleData.GetCustomRoleFromRole((Roles)roleId);
            p.ReSet(playerId);
            DataBase.AllPlayerData[playerId].CustomRole = p;
        }

        public static void GameStartAndPrepare()
        {
            //CustomRole_GameStart.GameStart();
        }
    }
}
