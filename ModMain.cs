using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Features;
using TownOfTrailay.Helpers.Role;
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
            AddRole<BaitRole>();
            AddRole<ClutchRole>();
            AddRole<CrewpostorRole>();
            AddRole<HunterRole>();
            AddRole<SabotagerRole>();
            AddRole<SerialKillerRole>();
            AddRole<TheGlitchRole>();
            AddRole<TimeMasterRole>();
            AddRole<UncertainRole>();
            AddRole<VampireRole>();
            AddRole<PoisonerRole>();
            new GameObject("RoleHelper").AddComponent<RoleHelper>().DontDestroy();
            CosmeticLoaderManager.LoadCosmetics();
            MapLoaderManager.LoadMaps();
            Harmony.PatchAll();
        }
        public static void AddRole<T>() where T : RoleBehaviour
        {
            string name = typeof(T).Name;
            if (RoleManager.Instance.allRoles.Any(r => r.GetType() == typeof(T)))
            {
                Debug.LogError("Role of type " + name + " already exists");
                return;
            }
            FieldInfo roleHolderField = typeof(RoleManager).GetField("RoleHolder", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo lastRoleIdField = typeof(RoleManager).GetField("lastRoleId", BindingFlags.Instance | BindingFlags.NonPublic);
            Transform holder = (Transform)roleHolderField.GetValue(RoleManager.Instance);
            if (holder == null)
            {
                holder = new GameObject("RoleHolder").transform;
                UnityEngine.Object.DontDestroyOnLoad(holder.gameObject);
                roleHolderField.SetValue(RoleManager.Instance, holder);
            }
            GameObject obj = new GameObject(name);
            obj.transform.parent = holder;
            T val = obj.AddComponent<T>();
            val.roleCodeName = name;
            ushort lastId = (ushort)lastRoleIdField.GetValue(RoleManager.Instance);
            val.roleId = lastId;
            lastRoleIdField.SetValue(RoleManager.Instance, (ushort)(lastId + 1));
            try
            {
                val.ConfigureRole();
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to configure role: " + ex);
            }
            RoleManager.Instance.allRoles.Add(val);
        }
    }
}
