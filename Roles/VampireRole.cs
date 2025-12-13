using Hazel;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class VampireRole : TOTBaseRole
    {
        public List<(PlayerControl player, ChangeableValue<float> timer)> Kills = new List<(PlayerControl player, ChangeableValue<float> timer)>();
        public override Color RoleColor { get; } = new Color32(86, 30, 27, byte.MaxValue);
        public override string roleDisplayName => "Vampire";
        public override string roleDescription => "You can bite others";
        public override bool NeutralKiller => true;
        public VanillaButtonManager Button;
        public float BiteCooldown = 30;
        public float KillDelay = 10;
        public float Timer;
        public PlayerControl CurrentTarget;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Impostor, RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
        }
        public override void OnRoleAdded()
        {
            Timer = BiteCooldown;
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "", TOTAssets.Bite, new Action(delegate
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
            if (LocalPlayer)
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
                    if (pair.player == null || pair.player.Data.IsDead)
                    {
                        Kills.Remove(pair);
                        break;
                    }
                    pair.timer.Value -= Time.deltaTime;
                    if (pair.timer.Value <= 0)
                    {
                        Kills.Remove(pair);
                        Player.RpcCustomMurder(pair.player, MurderResultFlags.Succeeded, true, true, false);
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
                myRend.material.SetColor("_OutlineColor", RoleColor);
                Button.spriteRender.color = Palette.EnabledColor;
                Button.spriteRender.material.SetFloat("_Desat", 0f);
                return;
            }
            Button.spriteRender.color = Palette.DisabledColor;
            Button.spriteRender.material.SetFloat("_Desat", 1f);
        }
    }
}
