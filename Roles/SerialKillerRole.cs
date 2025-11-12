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
    public class SerialKillerRole : TOTBaseRole
    {
        public override string roleDisplayName => "Serial killer";
        public override string roleDescription => "You are Serial killer. kill everyone to win";
        public VanillaButtonManager Button;
        public float Timer;
        public PlayerControl CurrentTarget;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
            CanUseKillButton = true;
        }
        public override void OnRoleAdded()
        {
            Timer = KillCooldown + 15;
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, HudManager.Instance.KillButton.ButtonText.text, HudManager.Instance.KillButton.renderer.sprite, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = KillCooldown + 15;
                    Player.RpcMurderPlayer(CurrentTarget, MurderResultFlags.Succeeded);
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
    }
}
