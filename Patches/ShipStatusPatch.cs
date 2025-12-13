using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TownOfTrailay.Helpers.Features;
using TownOfTrailay.Helpers.Utilities;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(ShipStatus), "GetPlayerCounts")]
    internal static class ShipStatusPatch
    {
        public static bool Prefix(ShipStatus __instance, ref ValueTuple<int, int, int, int, int> __result)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            for (int i = 0; i < GameData.Instance.PlayerCount; i++)
            {
                GameData.PlayerInfo playerInfo = GameData.Instance.AllPlayers[i];
                if (playerInfo != null && !playerInfo.Disconnected && !(playerInfo.myRole == null))
                {
                    if (playerInfo.myRole.RoleTeamType == RoleTeamTypes.Impostor)
                    {
                        num3++;
                    }
                    if (playerInfo.myRole.RoleTeamType == RoleTeamTypes.Crewmate)
                    {
                        num5++;
                    }
                    if (!playerInfo.IsDead)
                    {
                        if (playerInfo.myRole.RoleTeamType == RoleTeamTypes.Impostor)
                        {
                            num2++;
                        }
                        else if (playerInfo.myRole.RoleTeamType == RoleTeamTypes.Crewmate)
                        {
                            num++;
                        }
                        else if (playerInfo.myRole.IsNeutralKiller())
                        {
                            num4++;
                        }
                    }
                }
            }
            __result = new ValueTuple<int, int, int, int, int>(num, num2, num3, num4, num5);
            return false;
        }
    }
}
