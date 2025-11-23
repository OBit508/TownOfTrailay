using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Features;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Roles;
using UnityEngine;
using static TownOfTrailay.Roles.TimeMasterRole;

namespace TownOfTrailay.Helpers
{
    public class HelperManager : MonoBehaviour
    {
        public static HelperManager Instance;
        private static List<Helper> Helpers = new List<Helper>() { new TimeMasterHelper(), new ButtonSpriteHelper(), new LoadHelper(), new TOTRoleManager.NameHelper() };
        public static bool CanUpdate => AmongUsClient.Instance.IsGameStarted || AmongUsClient.Instance.GameMode == GameModes.FreePlay;
        public void Awake()
        {
            Instance = this;
            foreach (Helper helper in Helpers)
            {
                helper.Awake();
            }
        }
        public void Update()
        {
            foreach (Helper helper in Helpers)
            {
                helper.Update();
            }
        }
        public class Helper
        {
            public virtual void Update()
            {
            }
            public virtual void Awake()
            {
            }
        }
    }
}
