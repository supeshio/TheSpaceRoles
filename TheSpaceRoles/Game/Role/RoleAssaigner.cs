using AmongUs.GameOptions;
using System.Collections.Generic;
using System.Linq;
using TSR.Game.Role.Ability;
using static TSR.Helper;

namespace TSR.Game.Role
{

    [SmartPatch]
    public static class RoleAssigner
    {
        [SmartPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
        public static class RoleManagerAssignSelectRolesPatch
        {
            //参考SNR
            public static bool Prefix(RoleManager __instance)
            {
                var list2 = AmongUsClient.Instance.allClients.ToArray().Where(x => x.Character != null & x.Character.Data != null & !x.Character.Data.Disconnected & !x.Character.Data.IsDead).Select(x => x.Character.Data).ToList();

                IGameOptions currentGameOptions = GameOptionsManager.Instance.CurrentGameOptions;
                // バニラとの変更点ここ
                // インポスターの数をそのまま使ってバリデーションを防いでる
                int numImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
                __instance.DebugRoleAssignments(list2.ToIl2CppList(), ref numImpostors);
                GameManager.Instance.LogicRoleSelection.AssignRolesForTeam(list2.ToIl2CppList(), currentGameOptions, RoleTeamTypes.Impostor, numImpostors, new Il2CppSystem.Nullable<RoleTypes>(RoleTypes.Impostor));
                GameManager.Instance.LogicRoleSelection.AssignRolesForTeam(list2.ToIl2CppList(), currentGameOptions, RoleTeamTypes.Crewmate, int.MaxValue, new Il2CppSystem.Nullable<RoleTypes>(RoleTypes.Crewmate));
                return false;
            }
            public static void Postfix(RoleManager __instance)
            {
                //List<FPlayerControl> CrewPC() => [.. FPlayerControl.AllPlayer.Where(x=>x.PC.CachedPlayerData.Role.TeamType== RoleTeamTypes.Crewmate) ];
                //List<FPlayerControl> ImpPC() => [.. FPlayerControl.AllPlayer.Where(x => x.PC.CachedPlayerData.Role.TeamType == RoleTeamTypes.Impostor )];
                //List<FPlayerControl> PC() => [.. FPlayerControl.AllPlayer.Where(x => !x.RoleAssigned)];
                var assignRoles = CalcAssignRole();
                Logger.Info("Roles : \n"+ string.Join("\n",assignRoles));
                SetRoles(assignRoles, FPlayerControl.AllPlayer.ToList());
                //SetAllRole(RoleId.Crewmate, CrewPC().Where(x => x.Role == RoleId.None).ToList());
                //SetAllRole(RoleId.Impostor, ImpPC().Where(x => x.Role == RoleId.None).ToList());
            }
        }
        public static List<string> CalcAssignRole()
        {
            int numImpostors = GameOptionsManager.Instance.CurrentGameOptions.NumImpostors;
            Dictionary<string, int> teamPlayerCount = new();
            //後で人数決定を行うので一旦このまま
            //TODO:人数決定ロジック
            int numPlayers = FPlayerControl.AllPlayer.Length;
            int numCrewmates = numPlayers - numImpostors;
            //TODO:暫定措置
            teamPlayerCount["tsr:impostor"] = numImpostors;
            teamPlayerCount["tsr:crewmate"] = numCrewmates;
            //teamPlayerCount[TeamId.Jackal] = 1;//JK

            //RoleIdをいれよ
            List<string> roles =
            [
                "tsr:sheriff"
            ];

            List<string> resultRoles = roles;

            foreach (var team in TeamBaseRegister.TeamBaseTypeMap.Keys)
            {
                if (!teamPlayerCount.ContainsKey(team))
                    teamPlayerCount[team] = 0;

                int k = roles.ToArray().Count(x => RoleBase.GetRoleBase(x).Team() == team);//役職のカウント

                //resultRoles.AddRange(roles.ToArray().Where(x => RoleBase.GetRoleBase(x).Team() == team));//結果の役職に

                int f =  teamPlayerCount[team] - k;//役職持っていない人のカウント
                if (teamPlayerCount[team] > 0)
                {
                    Logger.Info(team + ":" + f.ToString());
                    for (int i = 0; i < f; i++)
                    {
                        resultRoles.Add(TeamBase.GetTeamBase(team).DefaultRole);
                    }
                }
            }
            return resultRoles;
        }
        //陣営ごとにプレイヤー数が決まっている
        //役職はそのなかで振り分け

        /// <summary>
        /// 指定したFPlayerControlリストに、入力されたRoleIdリストからランダムに役職を割り当てます。
        /// 役職IDリストが足りない場合は、余ったプレイヤーには何も割り当てません。
        /// </summary>
        /// <param name="roleIds">割り当てる役職IDリスト</param>
        /// <param name="players">割り当て対象のFPlayerControlリスト</param>
        public static void SetRoles(List<string> roleIds, List<FPlayerControl> players)
        {
            Logger.Info("");
            int playerCount = players.Count;
            int roleCount = roleIds?.Count ?? 0;
            if (roleIds == null)
            {
                Logger.Error("roleIdsがnullです", "SetAllRole");
                return;
            }

            // RoleIdリストをシャッフル
            var shuffledRoleIds = roleIds.OrderBy(_ => Random(int.MaxValue)).ToList();

            // プレイヤーに役職を割り当て
            for (int i = 0; i < playerCount; i++)
            {
                if (i >= roleCount) break; // 余ったプレイヤーには何も割り当てない
                var fp = players[i];
                var roleId = shuffledRoleIds[i];
                var role = RoleBase.GetRoleBase(roleId);
                if (role == null)
                {
                    Logger.Error($"RoleId {roleId} のRoleBase生成に失敗", "SetAllRole");
                    continue;
                }
                RpcSetRole(fp, role);
            }
        }
        /// <summary>
        /// 指定したFPlayerControlリストの全員に、指定したRoleIdの役職を割り当てます。
        /// </summary>
        /// <param name="roleId">割り当てる役職ID</param>
        /// <param name="players">割り当て対象のFPlayerControlリスト</param>
        public static void SetAllRole(string roleId, List<FPlayerControl> players)
        {
            Logger.Info($"SetAllRole: roleId={roleId}, count={players.Count}");
            var role = RoleBase.GetRoleBase(roleId);
            if (role == null)
            {
                Logger.Error($"RoleId {roleId} のRoleBase生成に失敗", "SetAllRole");
                return;
            }
            foreach (var fp in players)
            {
                RpcSetRole(fp, RoleBase.GetRoleBase(roleId));
            }
        }
        public static void RpcChangeRole(FPlayerControl fp, RoleBase role)
        {
            CustomButton.ClearButtons();
            RpcSetRole(fp,role);
            fp.GameStartReset();
            CustomButton.GameStartResetButtons();
        }
        public static void RpcSetRole(FPlayerControl fp, RoleBase role)
        {
            ClientSetRole(fp, role);
            RpcSetNativeRole(fp);
        }
        public static void ClientSetRole(FPlayerControl fp, RoleBase role)
        {
            if (fp != null && role != null) // Add this line
            {
                fp.PC.StartCoroutine(fp.PC.CoSetRole(AmongUs.GameOptions.RoleTypes.Crewmate, false));
                fp.SetRole(role);
            }
            else
            {
                Logger.Error("role or fp is null.");
            }
            if (!FPlayerControl.FTeams.TryGetValue(role.Team(), out var va))
            {
                TeamBase.GetTeamBase(role.Team()).Players = [];
            }
            TeamBase.GetTeamBase(role.Team()).Players.Add(fp);
            Logger.Info($"{fp.PlayerName}({fp.PlayerId})->{fp.Role}");
        }
        public static void RpcSetNativeRole(FPlayerControl fp)
        {
            var writer = CustomRPC.StartRpc(Rpcs.SetRole);
            writer.Write(fp.PlayerId);
            writer.Write((string)fp.Role);
            CustomRPC.EndRpc(writer);
        }
    }
}