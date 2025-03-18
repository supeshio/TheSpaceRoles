using HarmonyLib;
using Hazel;
using Il2CppMono.Security.Cryptography;
using UnityEngine.Purchasing;

namespace TheSpaceRoles
{
    public static class CheckedMurderPlayer
    {
        public static void HappenedKill(int source, int target, DeathReason reason)
        {

                DataBase.AllPlayerData[target].DeathPosition = Helper.GetPlayerById(target).GetTruePosition();
                DataBase.AllPlayerData[target].DeathReason = reason;
                DataBase.AllPlayerData[target].DeathMeetingCount = DataBase.MeetingCount;
                if (source == PlayerControl.LocalPlayer.PlayerId)
                {
                    Helper.GetCustomRole(PlayerControl.LocalPlayer).Killed();
                }
                if (target == PlayerControl.LocalPlayer.PlayerId)
                {
                    Helper.GetCustomRole(PlayerControl.LocalPlayer).WasKilled();
                    DataBase.Buttons.Do(x => x.actionButton.Hide());
                }
            


        }
        public static void Murder(int id1, int id2, DeathReason reason)
        {

            Logger.Message($"{id1} -> {id2}", "Murder");
            DataBase.AllPlayerData.Do(x => x.Value.CustomRole.Murder(Helper.GetPlayerById(id1), Helper.GetPlayerById(id2)));
            Helper.GetPlayerById(id1).MurderPlayer(Helper.GetPlayerById(id2), MurderResultFlags.Succeeded);
            /*

            DataBase.AllPlayerDeathReasons.Add(id2, reason);
            if (id2 == PlayerControl.LocalPlayer.PlayerId)
            {
                DataBase.buttons.Do(x => x.Death());

                Logger.Info($"Death, reason:{reason}");
            }*/
            HappenedKill(id1, id2, reason);
        }
        public static void RpcMurder(PlayerControl source, PlayerControl target, DeathReason reason, bool DoCustomRpcMurder = true)
        {

            if (Helper.GetCustomRole(target).BeMurdered(source))
            {
                Logger.Info("KillCancel");
            }
            else
            {

                Murder(source.PlayerId, target.PlayerId, reason);
                MessageWriter writer = CustomRPC.SendRpc(Rpcs.CheckedMurderPlayer);
                writer.Write((int)source.PlayerId);
                writer.Write((int)target.PlayerId);
                writer.Write((int)reason);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }
    }
    public static class UnCheckedMurderPlayer
    {
        public static void Murder(int id1, int id2, DeathReason reason)
        {

                KillAnimationPatch.AnimCancel = true;
                Helper.GetPlayerById(id1).MurderPlayer(Helper.GetPlayerById(id2), MurderResultFlags.Succeeded);
                /*
                DataBase.AllPlayerDeathReasons.Add(id2, reason);
                if (id2 == PlayerControl.LocalPlayer.PlayerId)
                {
                    DataBase.buttons.Do(x => x.Death());

                    Logger.Info($"Death, reason:{reason}");
                }*/
                CheckedMurderPlayer.HappenedKill(id1, id2, reason);
        }
        public static void RpcMurder(PlayerControl source, PlayerControl target, DeathReason reason, bool DoCustomRpcMurder = true)
        {


            if (Helper.GetCustomRole(target).BeMurdered(source))
            {
                Logger.Info("KillCancel");
            }
            else
            {
                Murder(source.PlayerId, target.PlayerId, reason);
                MessageWriter writer = CustomRPC.SendRpc(Rpcs.UnCheckedMurderPlayer);
                writer.Write((int)source.PlayerId);
                writer.Write((int)target.PlayerId);
                writer.Write((int)reason);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
            }
        }
    }
}
