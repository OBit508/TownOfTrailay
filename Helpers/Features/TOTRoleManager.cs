using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using TownOfTrailay.Roles;
using UnityEngine;

namespace TownOfTrailay.Helpers.Features
{
    internal static class TOTRoleManager
    {
        public static System.Collections.IEnumerator LoadRoles(TextMeshPro text, string baseStr)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            List<Type> roles = new List<Type>() { typeof(BaitRole), typeof(ClutchRole), typeof(CrewpostorRole), typeof(HunterRole), typeof(PoisonerRole), typeof(SabotagerRole), typeof(SerialKillerRole), typeof(TheGlitchRole), typeof(TimeMasterRole), typeof(UncertainRole), typeof(VampireRole) };
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
        public static Color GetColor(this RoleBehaviour role)
        {
            if (role is TOTBaseRole r)
            {
                return r.RoleColor;
            }
            return role.RoleTeamType == RoleTeamTypes.Crewmate ? Color.cyan : role.TeamColor;
        }
        public class NameHelper : HelperManager.Helper
        {
            public override void Update()
            {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    if (player.AmOwner || player.Data.myRole.RoleTeamType == RoleTeamTypes.Impostor && PlayerControl.LocalPlayer.Data.myRole.RoleTeamType == RoleTeamTypes.Impostor)
                    {
                        string timeMasterText = (player.Data.myRole is TimeMasterRole ? "Time points: " + TimeMasterHelper.GlobalPoints.Count.ToString() + " (" + (TimeMasterHelper.RewindActive ? "<color=#0000ff>Active</color>" : TimeMasterHelper.GlobalPoints.Count < TimeMasterHelper.MaxPoints ? "<color=#ff0000>Loading</color>" : "<color=#28ba00>Loaded</color>") + ")\n" : "");
                        player.PrivateSetName("<size=2><color=#" + player.Data.myRole.GetColor().ToHex() + ">" + player.Data.myRole.roleDisplayName + "</color></size>\n" + timeMasterText + "<color=#ffffff>" + player.name + "</color>");
                        player.nameText.transform.SetY(0.65f + (player.Data.myRole is TimeMasterRole ? 0.3f : 0.15f));
                    }
                }
            }
        }
    }
}
