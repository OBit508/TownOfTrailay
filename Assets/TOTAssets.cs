using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Assets
{
    internal static class TOTAssets
    {
        public static Sprite Clock;
        public static Sprite Mimic;
        public static Sprite ClutchKill;
        public static Sprite Bite;
        public static Sprite Poison;
        public static Sprite UsesCounter;
        public static Sprite Choose;
        public static Sprite HelperCard;
        public static Sprite TraitorCard;
        public static Sprite Drag;
        public static Sprite Eat;
        public static Sprite YellowKill;
        public static Sprite JuggerKill;
        public static void LoadAssets()
        {
            Clock = Utils.LoadSprite("TownOfTrailay.Assets.clock.png", 75);
            Mimic = Utils.LoadSprite("TownOfTrailay.Assets.mimic.png", 110);
            ClutchKill = Utils.LoadSprite("TownOfTrailay.Assets.clutchKill.png", 100);
            Bite = Utils.LoadSprite("TownOfTrailay.Assets.bite.png", 100);
            Poison = Utils.LoadSprite("TownOfTrailay.Assets.poison.png", 100);
            UsesCounter = Utils.LoadSprite("TownOfTrailay.Assets.usesCounter.png", 100);
            Choose = Utils.LoadSprite("TownOfTrailay.Assets.choose.png", 100);
            HelperCard = Utils.LoadSprite("TownOfTrailay.Assets.helperCard.png", 100);
            TraitorCard = Utils.LoadSprite("TownOfTrailay.Assets.traitorCard.png", 100);
            Drag = Utils.LoadSprite("TownOfTrailay.Assets.drag.png", 100);
            Eat = Utils.LoadSprite("TownOfTrailay.Assets.eat.png", 100);
            YellowKill = Utils.LoadSprite("TownOfTrailay.Assets.yellowKill.png", 100);
            JuggerKill = Utils.LoadSprite("TownOfTrailay.Assets.juggerKill.png", 100);
        }
    }
}
