using HarmonyLib;
using Hazel;

namespace TheSpaceRoles
{
    [HarmonyPatch]
    public static class Rpc
    {



        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static class Hud
        {
            public static void Prefix(PlayerControl __instance)
            {

            }
            public static void Postfix(byte callId, MessageReader reader)
            {
                if (callId >= 60)
                {
                    Logger.Info($"{(Rpcs)callId}");
                }
                switch ((Rpcs)callId)
                {
                    case Rpcs.SetRole:
                        int r1 = reader.ReadInt32();
                        int r2 = reader.ReadInt32();
                        GameStarter.SetRole(r1, r2);
                        break;
                    case Rpcs.SetTeam:
                        int t1 = reader.ReadInt32();
                        int t2 = reader.ReadInt32();
                        GameStarter.SetTeam(t1, t2);
                        break;
                    case Rpcs.RpcMurderPlayer:
                        RpcMurderPlayer.Murder(reader.ReadInt32(), reader.ReadInt32(), (DeathReason)reader.ReadInt32());
                        break;
                    case Rpcs.DataBaseReset:

                        DataBase.ResetAndPrepare();
                        break;
                    case Rpcs.SendRoomTimer:
                        LobbyTimer.GameStartManagerUpdatePatch.TimerSet(reader.ReadSingle(), reader.ReadSingle());
                        break;
                    case Rpcs.ShareOptions:
                        CustomOption.GetOptionSelections(reader);
                        break;
                    case Rpcs.UseAbility:
                        int useAbilityPlayerId = reader.ReadInt32();
                        int useAbilityRoleId = reader.ReadInt32();
                        int useAbilityId = reader.ReadInt32();
                        switch ((Roles)useAbilityRoleId)
                        {
                            case Roles.Mini:
                                Mini.SetAge(useAbilityPlayerId, reader.ReadInt32());
                                break;
                        }

                        break;

                }
            }
        }
        public static MessageWriter SendRpc(Rpcs rpc)
        {

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpc, SendOption.Reliable);
            return writer;

        }
        public static MessageWriter SendRpcUsebility(Rpcs rpc, Roles roleId, int playerId, int id)
        {

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpc, SendOption.Reliable);
            writer.Write(playerId);
            writer.Write((int)roleId);
            writer.Write(id);
            return writer;

        }

    }

    public enum Rpcs : int
    {
        SetRole = 80,
        SetTeam,
        ChangeRole,
        DataBaseReset,
        SendRoomTimer,
        GameEnd,
        ShareOptions,
        UseAbility,
        RpcMurderPlayer,//A kill B : A/B/Reasons
    }
    public enum DeathReason : int
    {
        Disconnected,
        VotedOut,
        ImpostorKill,
        SheriffKill,
        SheriffSuicide,
    }

}
