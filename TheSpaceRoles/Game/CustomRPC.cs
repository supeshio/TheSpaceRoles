using Hazel;
using TSR.Game.Role;

namespace TSR.Game
{
    [SmartPatch]
    public static class CustomRPC
    {
        [SmartPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static void Prefix(PlayerControl __instance)
        {
        }

        [SmartPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
        public static void Postfix(byte callId, MessageReader reader)
        {
            if (callId >= 80)
            {
                Logger.Message($"{(Rpcs)callId}");
            }
            else
            {

                Logger.Message($"{(Rpcs)callId}");
            }
            switch ((Rpcs)callId)
            {
                case Rpcs.SetRole:
                    Logger.Message("SetRoleRPC");
                    int playerId = reader.ReadInt32();
                    string roleId = reader.ReadString();
                    RoleAssigner.ClientSetRole(FPlayerControl.AllPlayer[playerId], RoleBase.GetRoleBase(roleId));
                    break;

                case Rpcs.Check:
                    CheckRpc(reader);
                    break;

                case Rpcs.Checked:
                    CheckedRpc(reader);
                    break;
            }
        }

        private static void CheckedRpc(MessageReader reader)
        {
            if (!FGameManager.AmHost)
            {
                int k = reader.ReadInt32();
                //Checker checker = GetChecker((CheckEnum)k);
                //checker.Success = true;
            }
        }

        private static void CheckRpc(MessageReader reader)
        {
            if (FGameManager.AmHost)
            {
                int k = reader.ReadInt32();
                int p = reader.ReadInt32();
                //Checker checker = GetChecker((CheckEnum)k);
                //checker.Checking();
            }
        }

        //private static Checker GetChecker(CheckEnum K)
        //{
        //    return K switch
        //    {
        //        //CheckEnum.SetRole => IIntroRunner.IntroSetRoleChecker,
        //        _ => null
        //    };
        //}
        public static MessageWriter StartRpc(Rpcs rpc)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)rpc, SendOption.Reliable);
            return writer;
        }

        public static MessageWriter SendRpcUseAbility(int playerId ,string roleId, int id)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)Rpcs.UseAbility, SendOption.Reliable);
            writer.Write(playerId);
            writer.Write(roleId);
            writer.Write(id);
            return writer;
        }

        public static void EndRpc(this MessageWriter writer)
        {
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
    }

    public enum CheckEnum : int
    {
        None = 0,
        SetRole,
    }

    public enum Rpcs : int
    {
        Check = 80,
        Checked,
        SetNativeRole,
        SetRole,
        SetTeam,
        ChangeRole,
        GameEnd,
        ShareOptions,
        UseAbility,
        CheckedMurderPlayer,//A kill B : A/B/Reasons
        UnCheckedMurderPlayer,//A kill B but not killanimation is only B : A/B/Reasons
        CheckVersion,
    }

    public enum DeathReason : int
    {
        KillByCommand,
        Disconnected,
        VotedOut,
        ImpostorKill,
        SheriffKill,
        SheriffSuicide,
        BittenByVampire,
        SerialKillerKill,
        SerialKillerSuicide,
        JackalKill,
        ShotByNiceGuesser,
        MisfiredByNiceGuesser,
        ShotByEvilGuesser,
        MisfiredByEvilGuesser,
    }
}