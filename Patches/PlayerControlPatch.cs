using HarmonyLib;
using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(PlayerControl))]
    internal static class PlayerControlPatch
    {
        [HarmonyPatch("HandleRpc")]
        [HarmonyPrefix]
        public static bool HandleRpcPrefix(PlayerControl __instance, [HarmonyArgument(0)] byte callId, [HarmonyArgument(1)] MessageReader reader)
        {
            if (callId == byte.MaxValue)
            {
                TOTRpcManager.HandleRpc(__instance, callId, reader);
                return false;
            }
            return true;
        }
        [HarmonyPatch("CheckMurder")]
        [HarmonyPrefix]
        public static bool CheckMurderPrefix(PlayerControl __instance, [HarmonyArgument(0)] PlayerControl t)
        {
            if (AmongUsClient.Instance.IsGameOver || !AmongUsClient.Instance.AmHost)
            {
                return false;
            }
            if (!t || __instance.Data.IsDead || __instance.Data.Disconnected)
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (__instance.Data.myRole.PreKillCheck && !__instance.Data.myRole.CheckMurder(t))
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (!__instance.Data.myRole || !t.Data.myRole)
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (t.inVent || t.Data.IsDead)
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (MeetingHud.Instance)
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (!__instance.Data.myRole.CanUseKillButton)
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (DateTime.UtcNow.Subtract(__instance.Data.LastMurder.UtcDateTime).TotalSeconds < (double)(__instance.Data.myRole.KillCooldown - 1.5f))
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            if (!__instance.Data.myRole.CheckMurder(t))
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.FailedError);
                return false;
            }
            __instance.Data.LastMurder = DateTime.UtcNow;
            if (__instance.Data.myRole is TOTBaseRole role)
            {
                __instance.RpcCustomMurder(t, MurderResultFlags.Succeeded, true, role.CreateDeadBodyWheenKill);
            }
            else
            {
                __instance.RpcMurderPlayer(t, MurderResultFlags.Succeeded);
            }
            return false;
        }
    }
}
