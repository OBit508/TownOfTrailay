using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Assets
{
    internal static class TOUAssets
    {
        public static Sprite Clock;
        public static Sprite Mimic;
        public static Sprite ClutchKill;
        public static Sprite Bite;
        public static Sprite Poison;
        public static void LoadAssets()
        {
            Clock = Utils.LoadSprite("TownOfTrailay.Assets.clock.png", 75);
            Mimic = Utils.LoadSprite("TownOfTrailay.Assets.mimic.png", 125);
            ClutchKill = Utils.LoadSprite("TownOfTrailay.Assets.clutchKill.png", 100);
            Bite = Utils.LoadSprite("TownOfTrailay.Assets.bite.png", 100);
            Poison = Utils.LoadSprite("TownOfTrailay.Assets.poison.png", 100);
        }
    }
}
