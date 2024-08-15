using AmongUs.GameOptions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static TheSpaceRoles.Helper;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class GameStarter
    {
        [HarmonyPatch(typeof(RoleManager))]
        [HarmonyPatch(nameof(RoleManager.SelectRoles)), HarmonyPrefix]
        public static void Reset()
        {
        }
        [HarmonyPatch(typeof(RoleManager))]
        [HarmonyPatch(nameof(RoleManager.SelectRoles)), HarmonyPostfix]

        public static void Select()
        {
            DataBase.AllPlayerTeams = new();
            DataBase.AllPlayerRoles = new();
            AmongUsClient.Instance.FinishRpcImmediately(Rpc.SendRpc(Rpcs.DataBaseReset));
            DataBase.ResetAndPrepare();
            foreach (int pid in DataBase.AllPlayerControls().Select(x => x.PlayerId))
            {
                var name = DataBase.AllPlayerControls().First(x => x.PlayerId == pid).cosmetics.nameText.text;

                name = Regex.Replace(name, "<color[^>]*?>", string.Empty);
                name = Regex.Replace(name, "<\\color[^>]*?>", string.Empty);
                DataBase.AllPlayerControls().First(x => x.PlayerId == pid).cosmetics.nameText.color = new();


            }
            //アサインされた役職を求める。
            foreach (var roleop in CustomOptionsHolder.RoleOptions_Count)
            {
                if(roleop.Value.selection() != 0)
                {
                    if (!DataBase.AssignedRoles.Contains(roleop.Key))
                    {
                        DataBase.AssignedRoles.Add(roleop.Key);
                    }
                }
            }


            //Resetするべ
            //今回はC3 I1
            //Sheriff 1
            //です
            if (PlayerControl.LocalPlayer.AmOwner)
            {
                Logger.Info("Owner");
                Dictionary<Teams, List<Roles>> tr = new();


                List<int> players = DataBase.AllPlayerControls().Select(x => (int)x.PlayerId).ToList();

                Logger.Info(players.Count.ToString());
                foreach (var keyValuePair in CustomOptionsHolder.RoleOptions_Count)
                {
                    var role = keyValuePair.Key;
                    var customOption = keyValuePair.Value.selection();
                    Logger.Info(role + $" is {customOption}");

                    for (int i = 0; i < customOption; i++)

                    {
                        if (tr.ContainsKey(RoleData.GetCustomRoleFromRole(role).team))
                        {

                            tr[RoleData.GetCustomRoleFromRole(role).team].Add(RoleData.GetCustomRoleFromRole(role).Role);
                        }
                        else
                        {
                            tr.Add(RoleData.GetCustomRoleFromRole(role).team, [RoleData.GetCustomRoleFromRole(role).Role]);

                        }
                        Logger.Info($"{customOption}");
                    }


                }
                if (DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).Count() > CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].selection())
                {
                    for (int i = 0; i < DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).Count() - CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].selection(); i++)
                    {

                        var k = DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).ToList();
                        k[RandomNext(k.Count())].RpcSetRole(AmongUs.GameOptions.RoleTypes.Crewmate);
                    }
                }
                else if (DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).Count() < CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].selection())
                {
                    for (int i = 0; i < CustomOptionsHolder.TeamOptions_Count[Teams.Impostor].selection() - DataBase.AllPlayerControls().Where(x => x.Data.RoleType == AmongUs.GameOptions.RoleTypes.Impostor).Count(); i++)
                    {

                        var k = DataBase.AllPlayerControls().Where(x => x.Data.RoleType != AmongUs.GameOptions.RoleTypes.Impostor).ToList();
                        k[RandomNext(k.Count())].RpcSetRole(AmongUs.GameOptions.RoleTypes.Impostor);
                    }
                }

                List<int> p = DataBase.AllPlayerControls().Where(x => x.Data.RoleType == RoleTypes.Impostor).Select(x => (int)x.PlayerId).ToList();
                var team = Teams.Impostor;
                var teammembers = CustomOptionsHolder.TeamOptions_Count[team].selection();
                var v = tr[team];
                for (int i = 0; i < teammembers; i++)
                {
                    if (p.Count == 0) continue;
                    Logger.Info($"{team},{teammembers},{v.Count}");
                    if (v.Count == 0)
                    {

                        var playertag = RandomNext(p.Count);
                        SendRpcSetRole(RoleData.GetCustomRole_NormalFromTeam(team).Role, p[playertag]);
                        Logger.Info(p[playertag].ToString() + $" is {RoleData.GetCustomRole_NormalFromTeam(team).Role}");
                        players.Remove(p[playertag]);
                    }
                    else
                    {
                        var r = Helper.RandomNext(v.Count);
                        var role = v[r];
                        var playertag = RandomNext(p.Count);
                        SendRpcSetRole(role, p[playertag]);
                        Logger.Info(p[playertag].ToString() + $" is {role}");
                        players.Remove(p[playertag]);
                        v.RemoveAt(r);

                    }
                }
                Logger.Info("playercount" + players.Count.ToString());
                foreach (var t in tr)
                {
                    Logger.Info($"{t.Key}:{string.Join(",", t.Value.Select(x => x.ToString()))}");
                }
                foreach (var item in tr)
                {
                    team = item.Key;
                    if (team == Teams.Crewmate) continue;
                    if (team == Teams.Impostor) continue;
                    teammembers = CustomOptionsHolder.TeamOptions_Count[team].selection();
                    v = item.Value;
                    for (int i = 0; i < teammembers; i++)
                    {
                        if (players.Count == 0) continue;
                        Logger.Info($"{team},{teammembers},{v.Count}");
                        if (v.Count == 0)
                        {

                            var player = RandomNext(players.Count);
                            SendRpcSetRole(RoleData.GetCustomRole_NormalFromTeam(team).Role, players[player]);
                            Logger.Info(players[player].ToString() + $" is {RoleData.GetCustomRole_NormalFromTeam(team).Role}");
                            players.RemoveAt(player);
                        }
                        else
                        {
                            var r = Helper.RandomNext(v.Count);
                            var role = v[r];
                            var player = RandomNext(players.Count);
                            SendRpcSetRole(role, players[player]);
                            Logger.Info(players[player].ToString() + $" is {role}");
                            players.RemoveAt(player);
                            v.RemoveAt(r);

                        }
                    }
                }
                Logger.Info("playercount" + players.Count.ToString());
                team = Teams.Crewmate;
                teammembers = players.Count;
                v = tr[team];
                while (players.Count != 0)
                {
                    Logger.Info($"{team},{teammembers},{v.Count}");
                    if (v.Count == 0)
                    {

                        var playertag = RandomNext(players.Count);
                        SendRpcSetRole(RoleData.GetCustomRole_NormalFromTeam(team).Role, players[playertag]);
                        Logger.Info(players[playertag].ToString() + $" is {RoleData.GetCustomRole_NormalFromTeam(team).Role}");
                        players.RemoveAt(playertag);
                    }
                    else
                    {
                        var r = Helper.RandomNext(v.Count);
                        var role = v[r];
                        var playertag = RandomNext(players.Count);
                        SendRpcSetRole(role, players[playertag]);
                        Logger.Info(players[playertag].ToString() + $" is {role}");
                        players.RemoveAt(playertag);
                        v.RemoveAt(r);

                    }
                }







                //    foreach (Teams team in Enum.GetValues(typeof(Teams)))
                //    {
                //        teams.Add(team, 0);
                //    }

                //    Dictionary<Teams, Dictionary<Roles, int>> roles = [];

                //    foreach (Roles role in Enum.GetValues(typeof(Roles)))
                //    {
                //        if (GetLink.CustomRoleLink.Any(x => x.Role == role))
                //        {
                //            var team = GetLink.GetCustomRole(role).teamsSupported;
                //                string s = team + "_" + role + "_" + "spawncount";
                //                var value = TSR.Instance.Config.Bind($"Preset", s, 0).Value;
                //                //var value = TSR.Instance.Config.Bind($"Preset{CustomOption.preset}", s, 0).Value;
                //                if (value > 0)
                //                {
                //                    if (!roles.ContainsKey(team))
                //                    {
                //                        roles.Add(team, []);
                //                    }
                //                    roles[team].Add(role, value);
                //                    //Logger.Info($"{team}_{role}:{value}");
                //                    teams[team] += value;
                //                }
                //            string s_ = "-1_" + role + "_" + "spawncount";
                //            //var value_ = TSR.Instance.Config.Bind($"Preset{CustomOption.preset}", s_, 0).Value;
                //            var value_ = TSR.Instance.Config.Bind($"Preset", s_, 0).Value;
                //            if (value_ > 0)
                //            {

                //                if (!roles.ContainsKey((Teams)(-1)))
                //                {
                //                    roles.Add((Teams)(-1), []);
                //                }
                //                roles[(Teams)(-1)].Add(role, value_);
                //                Logger.Info($"Additional_{role}:{value_}");
                //            }
                //        }
                //    }
                //    //SetTeam
                //    SendRpcSetTeam(teams);

                //    //SetRoles
                //    foreach (var team in roles)
                //    {
                //        if ((int)team.Key == -1)
                //        {

                //            foreach (var role in team.Value)
                //            {
                //                Logger.Info($"Additional_{role.Key}:{role.Value}");
                //                for (int i = 0; i < role.Value; i++)
                //                {
                //                    SendRpcSetRole(role.Key, DataBase.AllPlayerTeams.Select(x => x.Key).ToArray());

                //                }
                //            }
                //        }
                //        else
                //        {
                //            foreach (var role in team.Value)
                //            {
                //                Logger.Info($"{team.Key}_{role.Key}:{role.Value}");
                //                for (int i = 0; i < role.Value; i++)
                //                {
                //                    Logger.Info($"{team.Key}_{role.Key}({i})");
                //                    if (players.Any(x => DataBase.AllPlayerTeams[x] == team.Key))
                //                    {

                //                        int[] teamplayers = players.Where(x => DataBase.AllPlayerTeams[x] == team.Key).ToArray();
                //                        var v = SendRpcSetRole(role.Key, teamplayers);
                //                        players.ToList().RemoveAll(x => x == v);
                //                    }
                //                }
                //            }

                //        }
                //    }
                //    /*

                //    SendRpcSetRole(Roles.Sheriff, DataBase.AllPlayerTeams.Where(x => new Sheriff().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
                //    //SendRpcSetRole(Roles.Vampire, DataBase.AllPlayerTeams.Where(x => new Vampire().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
                //    SendRpcSetRole(Roles.SerialKiller, DataBase.AllPlayerTeams.Where(x => new SerialKiller().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
                //    */

                //    RemainingPlayerSetRoles();
                //    SendRpcSetRole(Roles.NiceMini, DataBase.AllPlayerTeams.Where(x => new NiceMini().teamsSupported==x.Value).Select(x => x.Key).ToArray());

                //RemainingPlayerSetRoles();





                //    foreach (var item in DataBase.AllPlayerRoles)
                //    {
                //        Logger.Info(item.Key + " : " + string.Join(",", item.Value.Select(x => x.Role)));
                //    }


                //}


                //いったんシェリフだけ自動的に一人に割り振ろうと思う
            }
        }
        public static void RemainingPlayerSetRoles()
        {
            foreach (var item in DataBase.AllPlayerControls().Select(x => x.PlayerId))
            {
                if (DataBase.AllPlayerRoles.ContainsKey(item))
                {

                }
                else
                {

                    var roles = RoleData.GetCustomRole_NormalFromTeam(DataBase.AllPlayerTeams[item]).Role;
                    SetRole(item, (int)roles);

                    //Rpc
                    var writer = Rpc.SendRpc(Rpcs.SetRole);
                    writer.Write((int)item);
                    writer.Write((int)roles);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            }

        }
        public static int SendRpcSetRole(Roles role, int playerId)
        {
            //設定作ってないけどここでほんとは分岐
            //ここではmainとして扱う
            SetRole(playerId, (int)role);

            //Rpc
            var writer = Rpc.SendRpc(Rpcs.SetRole);
            writer.Write(playerId);
            writer.Write((int)role);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return playerId;
        }
        //public static void SendRpcSetTeam(ref int[] playerIds)
        //{
        //    //設定作ってないけどここでほんとは分岐
        //    //ここではmainとして扱う
        //    SetRole(playerIds, (int)role);

        //    //Rpc
        //    var writer = Rpc.SendRpc(Rpcs.SetRole);
        //    writer.Write(playerIds);
        //    writer.Write((int)role);
        //    AmongUsClient.Instance.FinishRpcImmediately(writer);
        //    return playerId;
        //}

        public static void SetRole(int playerId, int roleId)
        {

            Logger.Info($"Player:{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text}({playerId}) -> Role:{(Roles)roleId}");


            if (DataBase.AllPlayerRoles.ContainsKey(playerId))
            {
                var list = DataBase.AllPlayerRoles[playerId].ToList();
                var p = RoleData.GetCustomRoleFromRole((Roles)roleId);
                p.ReSet(playerId);
                p.CustomTeam.Role = p;
                list.Add(p);
                DataBase.AllPlayerRoles[playerId] = [.. list];
            }
            else
            {
                var p = RoleData.GetCustomRoleFromRole((Roles)roleId);
                p.ReSet(playerId);
                p.CustomTeam.Role = p;
                DataBase.AllPlayerRoles.Add(playerId, [p]);

            }
            if (DataBase.AllPlayerTeams.ContainsKey(playerId))
            {

                DataBase.AllPlayerTeams[playerId] = RoleData.GetCustomRoleFromRole((Roles)roleId).team;
            }
            else
            {

                DataBase.AllPlayerTeams.Add(playerId, RoleData.GetCustomRoleFromRole((Roles)roleId).team);
            }

        }

        public static void GameStartAndPrepare()
        {
            //CustomRole_GameStart.GameStart();
        }
    }
}
