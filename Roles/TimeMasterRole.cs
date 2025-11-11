using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TownOfTrailay.Helpers;
using TownOfTrailay.Helpers.Utilities;
using TownOfTrailay.Assets;

namespace TownOfTrailay.Roles
{
    public class TimeMasterRole : TOUBaseRole
    {
        public override string roleDisplayName => "Time Master";
        public override string roleDescription => "You can rewind the time";
        public VanillaButtonManager Button;
        public float RewindCooldown = 10;
        public float AfterRewindCooldown = 20;
        public float Timer;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Crewmate;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Impostor, RoleTeamTypes.Neutral };
        }
        public override void OnRoleAdded()
        {
            Timer = RewindCooldown;
            Button = Utils.Create(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Rewind", TOUAssets.Clock, delegate
            {
                if (!RoleHelper.RewindActive && Timer <= 0 && RoleHelper.GlobalPoints.Count >= RoleHelper.MaxPoints)
                {
                    Timer = AfterRewindCooldown;
                    if (!CheckRewind())
                    {
                        SendRpc(RpcCalls.RpcCheckRewind);
                    }
                }
            });
        }
        public void Update()
        {
            if (Player.AmOwner)
            {
                if (Timer > 0 && !RoleHelper.RewindActive)
                {
                    Timer -= Time.deltaTime;
                    if (Timer < 0)
                    {
                        Timer = 0;
                    }
                }
                Button.CooldownText.text = Timer > 0 && !RoleHelper.RewindActive ? ((int)Timer).ToString() : "";
                Button.spriteRender.color = RoleHelper.RewindActive ? Palette.DisabledGrey : Color.white;
                Player.PrivateSetName("Time points: " + RoleHelper.GlobalPoints.Count.ToString() + " (" + (RoleHelper.RewindActive ? "<color=#0000ff>Active</color>" : RoleHelper.GlobalPoints.Count < RoleHelper.MaxPoints ? "<color=#ff0000>Loading</color>" : "<color=#28ba00>Loaded</color>") + ")\n" + Player.name);
            }
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcRewind:
                    RoleHelper.RewindActive = true;
                    break;
                case RpcCalls.RpcCheckRewind:
                    CheckRewind();
                    break;
            }
        }
        public bool CheckRewind()
        {
            if (AmongUsClient.Instance.AmHost)
            {
                if (RoleHelper.GlobalPoints.Count >= RoleHelper.MaxPoints)
                {
                    RoleHelper.RewindActive = true;
                    SendRpc(RpcCalls.RpcRewind);
                }
                return true;
            }
            return false;
        }
        public class TimePoint 
        {
            public List<(PlayerControl, bool, Vector3, Vector3)> Saves = new List<(PlayerControl, bool, Vector3, Vector3)>();
            public TimePoint()
            {
                foreach (PlayerControl player in PlayerControl.AllPlayerControls)
                {
                    Saves.Add((player, player.Data.IsDead, player.AmOwner && player.inVent ? player.closestVent.transform.position : player.transform.position, player.GetComponent<Rigidbody2D>().velocity));
                }
            }
            public void FixedUpdate()
            {
                foreach ((PlayerControl, bool, Vector3, Vector3) save in Saves)
                {
                    if (save.Item1.Data.IsDead && !save.Item2)
                    {
                        save.Item1.Revive();
                    }
                    save.Item1.transform.position = save.Item3;
                    save.Item1.GetComponent<Rigidbody2D>().velocity = save.Item4;
                }
            }
        }
    }
}
