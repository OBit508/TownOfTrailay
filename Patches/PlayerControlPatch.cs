using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Utilities;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(PlayerControl), "HandleRpc")]
    internal static class PlayerControlPatch
    {
        public static bool Prefix(PlayerControl __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            if (callId == byte.MaxValue)
            {
                TOTRpcManager.HandleRpc(__instance, callId, reader);
                return false;
            }
            return true;
        }
    }
}
