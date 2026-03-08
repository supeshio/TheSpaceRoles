using AmongUs.GameOptions;
using HarmonyLib;
using Hazel;
using InnerNet;
using System;

namespace TSR
{
    [SmartPatch(typeof(InnerNetClient),nameof(InnerNetClient.HostGame))]
    public static class HostGamePatch
    {
        public static readonly Guid ModGuid =
            new Guid("7a284585-d3a9-4a0e-9cde-f6058f03b72a");

        public static bool Prefix(InnerNetClient __instance,[HarmonyArgument(0)]IGameOptions settings,[HarmonyArgument(1)] GameFilterOptions filterOpts)
        {
            // Standard HostGame method body
            MessageWriter msg = MessageWriter.Get(SendOption.Reliable);
            msg.StartMessage(25/*HostModdedGame*/);
            msg.WriteBytesAndSize(__instance.gameOptionsFactory.ToBytes(settings, AprilFoolsMode.IsAprilFoolsModeToggledOn));
            msg.Write(CrossplayMode.GetCrossplayFlags());
            filterOpts.Serialize(msg);
            msg.Write(ModGuid.ToByteArray());
            
            // Standard HostGame method
            msg.EndMessage();
            __instance.SendOrDisconnect(msg);
            msg.Recycle();
            return false;
        }
        
    }
}