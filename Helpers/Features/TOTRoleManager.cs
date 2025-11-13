using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using TownOfTrailay.Helpers.Role;
using UnityEngine;

namespace TownOfTrailay.Helpers.Features
{
    internal static class TOTRoleManager
    {
        public static System.Collections.IEnumerator LoadRoles(TextMeshPro text, string baseStr)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> roles = assembly.GetTypes().ToList().FindAll(t => t.IsAssignableFrom(typeof(RoleBehaviour)) && t != typeof(TOTBaseRole));
            int originalCount = roles.Count;
            while (roles.Count > 0)
            {
                try
                {
                    AddRole(roles[0]);
                }
                catch { }
                text.text = baseStr + ((originalCount - roles.Count) * 100 / originalCount).ToString() + "%";
                roles.RemoveAt(0);
                yield return null;
            }
        }
        public static void AddRole(Type type)
        {
            string name = type.Name;
            if (RoleManager.Instance.allRoles.Any(r => r.GetType() == type))
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
            RoleBehaviour val = (RoleBehaviour)obj.AddComponent(type);
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
