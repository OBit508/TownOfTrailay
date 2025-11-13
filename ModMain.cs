using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers;
using TownOfTrailay.Helpers.Features;
using TownOfTrailay.Helpers.Utilities;
using TownOfTrailay.Roles;
using UnityEngine;

namespace TownOfTrailay
{
    public class ModMain
    {
        public static Harmony Harmony = new Harmony("rafael.newroles.com");
        public static void Load()
        {
            TOTAssets.LoadAssets();
            new GameObject("RoleHelper").AddComponent<HelperManager>().DontDestroy();
            Harmony.PatchAll();
        }
    }
}
