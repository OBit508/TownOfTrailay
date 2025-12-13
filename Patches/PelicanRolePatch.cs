using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(PelicanRole), "KillAbilityImageName", MethodType.Getter)]
    internal static class PelicanRolePatch
    {
        public static bool Prefix(ref string __result)
        {
            __result = "pelicanEat";
            return false;
        }
    }
}
