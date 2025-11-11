using Hazel;
using InnerNet;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class TheGlitch : TOUBaseRole
    {
        public bool Disguised;
        public float MimicCooldown = 10;
        public float MimicDuration = 10;
        public VanillaButtonManager Button;
        public float Timer;
        public override string roleDisplayName => "The Glitch";
        public override string roleDescription => "The Glitch\nMimic into others and kill.";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
            CanUseKillButton = true;
            CanVent = true;
            CanSabotage = true;
        }
        public override void OnRoleAdded()
        {
            Button = Utils.Create(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Test", TOUAssets.Mimic, new Action(CreateMenu));
            Timer = MimicCooldown;
        }
        public void Update()
        {
            if (Timer > 0)
            {
                Timer -= Time.deltaTime;
                if (Timer <= 0)
                {
                    if (Disguised && !CheckDisguise(null))
                    {
                        SendRpc(RpcCalls.RpcCheckRewind, new Action<MessageWriter>(delegate (MessageWriter writer)
                        {
                            writer.Write(false);
                        }));
                    }
                    Timer = 0;
                }
            }
            Button.CooldownText.text = Timer > 0 ? ((int)Timer).ToString() : "";
            Button.CooldownText.color = Disguised ? Palette.Purple : Color.white;
        }
        public override void OnMeetingCalled()
        {
            if (Disguised)
            {
                Disguise(null);
            }
        }
        public void CreateMenu()
        {
            List<PlayerPickOption> options = new List<PlayerPickOption>();
            foreach (GameData.PlayerInfo target in GameData.Instance.AllPlayers)
            {
                if (!target.Disconnected || target != PlayerControl.LocalPlayer.Data && !target.IsDead)
                {
                    string name = target.PlayerName;
                    if (target.IsDead)
                    {
                        name += " (Dead)";
                    }
                    options.Add(new PlayerPickOption(name, Color.white, (byte)target.ColorId, new Action(delegate
                    {
                        if (!CheckDisguise(target))
                        {
                            SendRpc(RpcCalls.RpcCheckRewind, new Action<MessageWriter>(delegate (MessageWriter writer) 
                            {
                                writer.Write(true);
                                writer.WriteNetObject(target.Object);
                            }));
                        }
                    })));
                }
            }
            PlayerPickMenu.Create(options);
        }
        public void Disguise(GameData.PlayerInfo playerInfo)
        {
            Player.RawSetOutfit(playerInfo == null ? Player.Data : playerInfo);
            Disguised = playerInfo != null;
            Timer = Disguised ? MimicDuration : MimicCooldown;
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcDisguise:
                    PlayerControl player = reader.ReadNetObject<PlayerControl>();
                    bool disguised = reader.ReadBoolean();
                    if (!disguised)
                    {
                        PlayerControl target = reader.ReadNetObject<PlayerControl>();
                        Disguise(target.Data);
                    }
                    break;
                case RpcCalls.RpcCheckDisguise:
                    bool flag = reader.ReadBoolean();
                    CheckDisguise(flag ? reader.ReadNetObject<PlayerControl>().Data : null);
                    break;
            }
        }
        public bool CheckDisguise(GameData.PlayerInfo playerInfo)
        {
            if (AmongUsClient.Instance.AmHost)
            {
                SendRpc(RpcCalls.RpcDisguise, new Action<MessageWriter>(delegate (MessageWriter writer)
                {
                    writer.WriteNetObject(Player);
                    writer.Write(Disguised);
                    if (!Disguised)
                    {
                        writer.WriteNetObject(playerInfo.Object);
                    }
                }));
                Disguise(playerInfo);
                return true;
            }
            return false;
        }
    }
}
