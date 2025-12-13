using Hazel;
using Rewired;
using System;
using System.Linq;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class JuggernautRole : TOTBaseRole
    {
        public float OriginalKillCooldown = 30;
        public float CurrentKillCooldown;
        public override Color RoleColor { get; } = new Color32(140, 0, 77, byte.MaxValue);
        public override string roleDisplayName => "Juggernaut";
        public override string roleDescription => "After every kill your cooldown lowers";
        public override bool NeutralKiller => true;
        public VanillaButtonManager Button;
        public float Timer;
        public PlayerControl CurrentTarget;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
            CanUseKillButton = false;
        }

        public override void OnMurder(PlayerControl target)
        {
            base.OnMurder(target);
            if (CurrentKillCooldown > 5)
            {
                CurrentKillCooldown -= 5;
            }
        }
        public override void OnRoleAdded()
        {
            CurrentKillCooldown = OriginalKillCooldown;
            Timer = CurrentKillCooldown;
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, HudManager.Instance.KillButton.ButtonText.text, TOTAssets.JuggerKill, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = CurrentKillCooldown;
                    Player.RpcMurderPlayer(CurrentTarget, MurderResultFlags.Succeeded);
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
