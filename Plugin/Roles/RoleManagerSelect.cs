using HarmonyLib;
using System;
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

            AmongUsClient.Instance.FinishRpcImmediately(Rpc.SendRpc(Rpcs.DataBaseReset));
            DataBase.ResetAndPrepare();
            foreach (int pid in DataBase.AllPlayerControls().Select(x => x.PlayerId))
            {
                var name = DataBase.AllPlayerControls().First(x => x.PlayerId == pid).cosmetics.nameText.text;

                name = Regex.Replace(name, "<color[^>]*?>", string.Empty);
                name = Regex.Replace(name, "<\\color[^>]*?>", string.Empty);
                DataBase.AllPlayerControls().First(x => x.PlayerId == pid).cosmetics.nameText.color = new();


            }
        }
        [HarmonyPatch(typeof(RoleManager))]
        [HarmonyPatch(nameof(RoleManager.SelectRoles)), HarmonyPostfix]

        public static void Select()
        {
            //Resetするべ
            //今回はC3 I1
            //Sheriff 1
            //です
            if (PlayerControl.LocalPlayer.AmOwner)
            {


                int[] players = DataBase.AllPlayerControls().Select(x => (int)x.PlayerId).ToArray();

                Dictionary<Teams, int> teams = new() { };
                foreach (Teams team in Enum.GetValues(typeof(Teams)))
                {
                    teams.Add(team, 0);
                }

                Dictionary<Teams, Dictionary<Roles, int>> roles = [];

                foreach (Roles role in Enum.GetValues(typeof(Roles)))
                {
                    if (GetLink.CustomRoleLink.Any(x => x.Role == role))
                    {
                        foreach (var team in GetLink.GetCustomRole(role).teamsSupported)
                        {

                            string s = team + "_" + role + "_" + "spawncount";
                            var value = TSR.Instance.Config.Bind($"Preset", s, 0).Value;
                            //var value = TSR.Instance.Config.Bind($"Preset{CustomOption.preset}", s, 0).Value;
                            if (value > 0)
                            {
                                if (!roles.ContainsKey(team))
                                {
                                    roles.Add(team, []);
                                }
                                roles[team].Add(role, value);
                                //Logger.Info($"{team}_{role}:{value}");
                                teams[team] += value;
                            }
                        }
                        string s_ = "-1_" + role + "_" + "spawncount";
                        //var value_ = TSR.Instance.Config.Bind($"Preset{CustomOption.preset}", s_, 0).Value;
                        var value_ = TSR.Instance.Config.Bind($"Preset", s_, 0).Value;
                        if (value_ > 0)
                        {

                            if (!roles.ContainsKey((Teams)(-1)))
                            {
                                roles.Add((Teams)(-1), []);
                            }
                            roles[(Teams)(-1)].Add(role, value_);
                            Logger.Info($"Additional_{role}:{value_}");
                        }
                    }
                }
                //SetTeam
                SendRpcSetTeam(teams);

                //SetRoles
                foreach (var team in roles)
                {
                    if ((int)team.Key == -1)
                    {

                        foreach (var role in team.Value)
                        {
                            Logger.Info($"Additional_{role.Key}:{role.Value}");
                            for (int i = 0; i < role.Value; i++)
                            {
                                SendRpcSetRole(role.Key, DataBase.AllPlayerTeams.Select(x => x.Key).ToArray());

                            }
                        }
                    }
                    else
                    {
                        foreach (var role in team.Value)
                        {
                            Logger.Info($"{team.Key}_{role.Key}:{role.Value}");
                            for (int i = 0; i < role.Value; i++)
                            {
                                Logger.Info($"{team.Key}_{role.Key}({i})");
                                if (players.Any(x => DataBase.AllPlayerTeams[x] == team.Key))
                                {

                                    int[] teamplayers = players.Where(x => DataBase.AllPlayerTeams[x] == team.Key).ToArray();
                                    var v = SendRpcSetRole(role.Key, teamplayers);
                                    players.ToList().RemoveAll(x => x == v);
                                }
                            }
                        }

                    }
                }
                /*

                SendRpcSetRole(Roles.Sheriff, DataBase.AllPlayerTeams.Where(x => new Sheriff().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
                //SendRpcSetRole(Roles.Vampire, DataBase.AllPlayerTeams.Where(x => new Vampire().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
                SendRpcSetRole(Roles.SerialKiller, DataBase.AllPlayerTeams.Where(x => new SerialKiller().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
                */

                RemainingPlayerSetRoles();
                SendRpcSetRole(Roles.Mini, DataBase.AllPlayerTeams.Where(x => new Mini().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());







                foreach (var item in DataBase.AllPlayerRoles)
                {
                    Logger.Info(item.Key + " : " + string.Join(",", item.Value.Select(x => x.Role)));
                }


            }


            //いったんシェリフだけ自動的に一人に割り振ろうと思う
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

                    var roles = GetLink.GetCustomRoleNormal(DataBase.AllPlayerTeams[item]).Role;
                    SetRole(item, (int)roles);

                    //Rpc
                    var writer = Rpc.SendRpc(Rpcs.SetRole);
                    writer.Write((int)item);
                    writer.Write((int)roles);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
            }

        }
        public static int SendRpcSetRole(Roles roles, int[] players)
        {
            if (players.Length == 0) { Logger.Warning("players ないよおおおおおお"); return -1; }
            //設定作ってないけどここでほんとは分岐
            //ここではmainとして扱う
            var a = players[Random(0, players.Length - 1)];
            SetRole(a, (int)roles);

            //Rpc
            var writer = Rpc.SendRpc(Rpcs.SetRole);
            writer.Write(a);
            writer.Write((int)roles);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            return a;
        }

        public static void SetRole(int playerId, int roleId)
        {

            Logger.Info($"Player:{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text}({playerId}) -> Role:{(Roles)roleId}");


            if (DataBase.AllPlayerRoles.ContainsKey(playerId))
            {
                var list = DataBase.AllPlayerRoles[playerId].ToList();
                var p = GetLink.GetCustomRole((Roles)roleId);
                p.ReSet(playerId, DataBase.AllPlayerTeams[playerId]);
                p.Team.Role = p;
                list.Add(p);
                DataBase.AllPlayerRoles[playerId] = [.. list];
            }
            else
            {
                var p = GetLink.GetCustomRole((Roles)roleId);
                p.ReSet(playerId, DataBase.AllPlayerTeams[playerId]);
                p.Team.Role = p;
                DataBase.AllPlayerRoles.Add(playerId, [p]);

            }


        }
        public static void SendRpcSetTeam(Dictionary<Teams, int> teams)
        {
            List<byte> ImpIds = DataBase.AllPlayerControls().Where(x => x.Data.Role.TeamType == RoleTeamTypes.Impostor).Select(x => x.PlayerId).ToList();
            List<byte> CrewIds = DataBase.AllPlayerControls().Where(x => x.Data.Role.TeamType != RoleTeamTypes.Impostor).Select(x => x.PlayerId).ToList();
            //Logger.Info(string.Join("\n", DataBase.AllPlayerControls().Select(x => x.Data.Role.TeamType).ToArray()));
            //Logger.Info($"imp:{ImpIds.Count}  c:{CrewIds.Count}");

            foreach ((Teams teams1, int count) in teams)
            {


                if (teams1 != Teams.Crewmate && teams1 != Teams.Impostor)
                {
                    for (int i = 0; i < count; i++)
                    {

                        int r = Random(0, CrewIds.Count - 1);

                        SetTeam(CrewIds[r], (int)teams1);
                        CrewIds.RemoveAt(r);
                    }
                }
                else if (teams1 == Teams.Impostor)
                {
                    if (ImpIds.Count > count)
                    {
                        for (int i = 0; i < count - ImpIds.Count; i++)
                        {

                            int r = Random(0, ImpIds.Count - 1);

                            SetTeam(ImpIds[r], (int)teams1);

                            ImpIds.RemoveAt(r);
                        }
                    }
                    else if
                     (ImpIds.Count < count)
                    {

                        for (int i = 0; i < ImpIds.Count - count; i++)
                        {

                            int r = Random(0, ImpIds.Count - 1);

                            SetTeam(CrewIds[r], (int)teams1);

                            ImpIds.RemoveAt(r);
                        }
                    }
                }

            }
            foreach (var pId in CrewIds)
            {
                SetTeam(pId, (int)Teams.Crewmate);

            }
            foreach (var pId in ImpIds)
            {
                SetTeam(pId, (int)Teams.Impostor);

            }
            foreach (var database in DataBase.AllPlayerTeams)
            {

                var writer = Rpc.SendRpc(Rpcs.SetTeam);
                writer.Write(database.Key);
                writer.Write((int)database.Value);
                AmongUsClient.Instance.FinishRpcImmediately(writer);

            }

        }
        public static void SetTeam(int playerId, int teamId)
        {



            //ここにどのroleIdがどのロールに対応するかを判定して

            //var p = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).PlayerId;
            Logger.Info($"Player:{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text}({playerId}) -> Team:{(Teams)teamId}");

            DataBase.AllPlayerTeams.Add(playerId, (Teams)teamId);
        }

        public static void GameStartAndPrepare()
        {
            //CustomRole_GameStart.GameStart();
        }
    }
}
