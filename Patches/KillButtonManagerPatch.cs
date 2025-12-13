using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Features;
using TownOfTrailay.Roles;
using UnityEngine;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(KillButtonManager), "SetTarget")]
    internal static class KillButtonManagerPatch
    {
        public static bool Prefix(KillButtonManager __instance, [HarmonyArgument(0)] PlayerControl target)
        {
            if (__instance.CurrentTarget && __instance.CurrentTarget != target)
            {
                __instance.CurrentTarget.myRend.material.SetFloat("_Outline", 0f);
            }
            __instance.CurrentTarget = target;
            if (__instance.CurrentTarget)
            {
                SpriteRenderer myRend = __instance.CurrentTarget.myRend;
                myRend.material.SetFloat("_Outline", 1f);
                myRend.material.SetColor("_OutlineColor", PlayerControl.LocalPlayer.Data.myRole.GetColor());
                __instance.renderer.color = Palette.EnabledColor;
                __instance.renderer.material.SetFloat("_Desat", 0f);
                return false;
            }
            __instance.renderer.color = Palette.DisabledColor;
            __instance.renderer.material.SetFloat("_Desat", 1f);
            return false;
        }
    }
}
