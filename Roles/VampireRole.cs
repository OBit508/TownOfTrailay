using Hazel;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class VampireRole : TOTBaseRole
    {
        public List<(PlayerControl player, ChangeableValue<float> timer)> Kills = new List<(PlayerControl player, ChangeableValue<float> timer)>();
        public override string roleDisplayName => "Vampire";
        public override string roleDescription => "You can bite others";
        public VanillaButtonManager Button;
        public float BiteCooldown = 10;
        public float KillDelay = 20;
        public float Timer;
        public PlayerControl CurrentTarget;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Impostor, RoleTeamTypes.Crewmate };
        }
        public override void OnRoleAdded()
        {
            Timer = BiteCooldown;
            Button = Utils.Create(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "", TOUAssets.Bite, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = BiteCooldown;
                    Kills.Add((CurrentTarget, new ChangeableValue<float>(KillDelay)));
                }
            }));
        }
        public void Update()
        {
            if (Player.AmOwner)
            {
                if (Timer > 0)
                {
                    Timer -= Time.deltaTime;
                    if (Timer < 0)
                    {
                        Timer = 0;
                    }
                }
                Button.CooldownText.text = Timer > 0 ? ((int)Timer).ToString() : "";
                SetTarget(Player.FindClosestTarget());
                for (int i = Kills.Count - 1; i >= 0; i--)
                {
                    (PlayerControl player, ChangeableValue<float> timer) pair = Kills[i];
                    pair.timer.Value -= Time.deltaTime;
                    if (pair.timer.Value <= 0)
                    {
                        SendRpc(RpcCalls.RpcPoison, new Action<MessageWriter>(delegate (MessageWriter writer) { writer.WriteNetObject(CurrentTarget); }));
                        Player.CustomMurderPlayer(pair.player);
                        Kills.Remove(pair);
                    }
                }
            }
        }
        public void SetTarget(PlayerControl target)
        {
            if (CurrentTarget && CurrentTarget != target)
            {
                CurrentTarget.myRend.material.SetFloat("_Outline", 0f);
            }
            CurrentTarget = target;
            if (CurrentTarget)
            {
                SpriteRenderer myRend = CurrentTarget.myRend;
                myRend.material.SetFloat("_Outline", 1f);
                myRend.material.SetColor("_OutlineColor", PlayerControl.LocalPlayer.Data.myRole.TeamColor);
                Button.spriteRender.color = Palette.EnabledColor;
                Button.spriteRender.material.SetFloat("_Desat", 0f);
                return;
            }
            Button.spriteRender.color = Palette.DisabledColor;
            Button.spriteRender.material.SetFloat("_Desat", 1f);
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcPoison:
                    Player.CustomMurderPlayer(reader.ReadNetObject<PlayerControl>());
                    break;
            }
        }
    }
}
