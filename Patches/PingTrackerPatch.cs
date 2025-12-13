using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(PingTracker), "Update")]
    internal static class PingTrackerPatch
    {
        public static bool Prefix(PingTracker __instance)
        {
            if (AmongUsClient.Instance)
            {
                string extraText = TownOfTrailayPlugin.TestBuild ? "\n<color=#00E3C9>TownOfTrailay</color>" : "";
                string text = "\n<size=95%><color=red>Modding</color> <color=blue>Us</color>" + extraText;
                string mapCredits = CreditState.MapCredits ?? "";
                if (!string.IsNullOrEmpty(mapCredits))
                {
                    text += "\n" + mapCredits;
                }
                __instance.text.Text = string.Format("{0}: {1} ms", CachedMaterials.CachedPing, AmongUsClient.Instance.Ping) + text;
            }
            return false;
        }
    }
}
