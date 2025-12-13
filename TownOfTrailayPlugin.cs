using HarmonyLib;
using Pathfinding;
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
    public class TownOfTrailayPlugin : ModBehaviour
    {
        public static bool TestBuild = true;
        public Harmony Harmony = new Harmony("townoftrailay.gg");
        public override string ModName => "<size=65%>Town Of Trailay</size>";
        public override string ModDescription => "<size=70%>Town Of Trailay is a mod for modding us that add new roles.</size>";
        public override string ModVersion => "Beta 8";
        public override bool ClientOnly => false;
        public override Sprite ModImage => TOTAssets.ModLogo;
        public override void ApplyMod()
        {
            TOTAssets.LoadAssets();
            RoleManager roleManager = RoleManager.Instance;
            roleManager.allSprites.Add("draggerSprite", TOTAssets.Drag);
            roleManager.allSprites.Add("scavengerKill", TOTAssets.ScavengerKill);
            roleManager.allSprites.Add("clutchKill", TOTAssets.ClutchKill);
            roleManager.allSprites.Add("juggerKill", TOTAssets.JuggerKill);
            roleManager.allSprites.Add("pelicanEat", TOTAssets.Eat);
            roleManager.allSprites.Remove("shootSprite");
            roleManager.allSprites.Add("shootSprite", TOTAssets.YellowKill);
            new GameObject("HelperManager").AddComponent<HelperManager>().DontDestroy();
            Harmony.PatchAll();
        }
    }
}
