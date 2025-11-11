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
        public static void LoadAssets()
        {
            Clock = Utils.LoadSprite("NewRoles.Assets.clock.png", 75);
            Mimic = Utils.LoadSprite("NewRoles.Assets.mimic.png", 100);
        }
    }
}
