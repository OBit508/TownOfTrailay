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
    [HarmonyPatch(typeof(KillButtonManager))]
    internal static class KillButtonManagerPatch
    {
        [HarmonyPatch("Refresh")]
        [HarmonyPrefix]
        public static bool RefreshPrefix(KillButtonManager __instance)
        {
            PlayerControl local = GameFast.Local;
            if (local == null || local.Data.myRole == null)
            {
                __instance.ButtonText.text = "KILL";
                return false;
            }
            RoleBehaviour role = local.Data.myRole;
            __instance.ButtonText.text = role.KillAbilityName;
            __instance.renderer.sprite = role is ClutchRole ? TOTAssets.ClutchKill : (role is SheriffRole || role is HunterRole ? TOTAssets.YellowKill : role is JuggernautRole ? TOTAssets.JuggerKill : role is PelicanRole ? TOTAssets.Eat : RoleManager.Instance.allSprites["killSprite"]);
            return false;
        }
        [HarmonyPatch("SetTarget")]
        [HarmonyPrefix]
        public static bool SetTargetPrefix(KillButtonManager __instance, [HarmonyArgument(0)] PlayerControl target)
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
