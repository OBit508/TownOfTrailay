using Hazel;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfTrailay.Helpers.Utilities
{
    public static class TOTRpcManager
    {
        public static void SendRpc(Action<MessageWriter> messageWriter, uint targetNetId, byte callId, SendOption option, int targetClientId = -1)
        {
            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(targetNetId, callId, option, targetClientId);
            messageWriter?.Invoke(writer);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        public static void RpcCustomMurder(this PlayerControl source, PlayerControl target, MurderResultFlags status, bool resetKillTimer = true, bool createDeadBody = true, bool teleportMurderer = true, bool showKillAnim = true, bool playKillSound = true)
        {
            if (status == MurderResultFlags.NULL)
            {
                DebugLogger.Log("I wonder how this even happened");
                return;
            }
            if (status != MurderResultFlags.FailedError)
            {
                if (AmongUsClient.Instance.AmClient)
                {
                    source.CustomMurderPlayer(target, resetKillTimer, createDeadBody, teleportMurderer, showKillAnim, playKillSound);
                }
                SendRpc(delegate (MessageWriter messageWriter)
                {
                    messageWriter.Write((int)RpcCalls.RpcCustomMurder);
                    messageWriter.WriteNetObject(target);
                    messageWriter.Write(resetKillTimer);
                    messageWriter.Write(createDeadBody);
                    messageWriter.Write(teleportMurderer);
                    messageWriter.Write(showKillAnim);
                    messageWriter.Write(playKillSound);
                }, PlayerControl.LocalPlayer.NetId, byte.MaxValue, SendOption.Reliable);
                return;
            }
        }
        public static void HandleRpc(InnerNetObject innerNetObject, byte callId, MessageReader messageReader)
        {
            RpcCalls rpcCalls = (RpcCalls)messageReader.ReadInt32();
            if (innerNetObject is PlayerControl player && rpcCalls == RpcCalls.RpcCustomMurder)
            {
                PlayerControl target = messageReader.ReadNetObject<PlayerControl>();
                bool resetKillTimer = messageReader.ReadBoolean();
                bool createDeadBody = messageReader.ReadBoolean();
                bool teleportMurderer = messageReader.ReadBoolean();
                bool showKillAnim = messageReader.ReadBoolean();
                bool playKillSound = messageReader.ReadBoolean();
                player.CustomMurderPlayer(target, resetKillTimer, createDeadBody, teleportMurderer, showKillAnim, playKillSound);
            }
        }
    }
}
