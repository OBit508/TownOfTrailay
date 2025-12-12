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
        public static bool TestBuild = true;
        public static Harmony Harmony = new Harmony("townoftrailay.gg");
        public static void Load()
        {
            TOTAssets.LoadAssets();
            RoleManager roleManager = RoleManager.Instance;
            roleManager.allSprites.Remove("reviveSprite");
            roleManager.allSprites.Add("reviveSprite", TOTAssets.Drag);
            new GameObject("HelperManager").AddComponent<HelperManager>().DontDestroy();
            Harmony.PatchAll();
        }
    }
}
