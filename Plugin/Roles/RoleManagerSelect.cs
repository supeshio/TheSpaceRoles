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

                Dictionary<Teams, int> roles = new Dictionary<Teams, int>() { { Teams.Impostor, 1 } };
                SendRpcSetTeam(roles);
                SendRpcSetRole(Roles.Sheriff, DataBase.AllPlayerTeams.Where(x => new Sheriff().teamsSupported.Contains(x.Value)).Select(x => x.Key).ToArray());
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
        public static void SendRpcSetRole(Roles roles, int[] players)
        {
            if (players.Length == 0) { Logger.Warning("players ないよおおおおおお"); return; }
            //設定作ってないけどここでほんとは分岐
            //ここではmainとして扱う
            var a = players[Random(0, players.Length - 1)];
            SetRole(a, (int)roles);

            //Rpc
            var writer = Rpc.SendRpc(Rpcs.SetRole);
            writer.Write(a);
            writer.Write((int)roles);
            AmongUsClient.Instance.FinishRpcImmediately(writer);

        }

        public static void SetRole(int playerId, int roleId)
        {

            Logger.Info($"Player:{DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).cosmetics.nameText.text}({playerId}) -> Role:{(Roles)roleId}");


            if (DataBase.AllPlayerRoles.ContainsKey(playerId))
            {
                var list = DataBase.AllPlayerRoles[playerId].ToList();
                var p = GetLink.GetCustomRole((Roles)roleId);
                p.PlayerId = playerId;
                p.PlayerName = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).name.Replace("<color=.*>", string.Empty).Replace("</color>", string.Empty);
                list.Add(p);
                DataBase.AllPlayerRoles[playerId] = [.. list];
            }
            else
            {
                var p = GetLink.GetCustomRole((Roles)roleId);
                p.PlayerId = playerId;
                p.PlayerName = DataBase.AllPlayerControls().First(x => x.PlayerId == playerId).name.Replace("<color=.*>", string.Empty).Replace("</color>", string.Empty);

                DataBase.AllPlayerRoles.Add(playerId, [p]);

            }


        }
        public static void SendRpcSetTeam(Dictionary<Teams, int> teams)
        {
            List<byte> ImpIds = DataBase.AllPlayerControls().Where(x => x.Data.Role.TeamType == RoleTeamTypes.Impostor).Select(x => x.PlayerId).ToList();
            List<byte> CrewIds = DataBase.AllPlayerControls().Where(x => x.Data.Role.TeamType != RoleTeamTypes.Impostor).Select(x => x.PlayerId).ToList();
            Logger.Info(string.Join("\n", DataBase.AllPlayerControls().Select(x => x.Data.Role.TeamType).ToArray()));
            Logger.Info($"imp:{ImpIds.Count}  c:{CrewIds.Count}");

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
