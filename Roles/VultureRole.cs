using Hazel;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class VultureRole : TOTBaseRole
    {
        public override Color RoleColor { get; } = new Color32(87, 46, 0, byte.MaxValue);
        public override string roleDisplayName => "Vulture";
        public override string roleDescription => "Eat 3 bodies.";
        public DeadBody CurrentTarget;
        public VanillaButtonManager Button;
        public float EatCooldown = 15;
        public float Timer;
        public int EatCount;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral, RoleTeamTypes.Neutral };
            CanVent = true;
        }
        public override void OnRoleAdded()
        {
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Clean", TOTAssets.Clean, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = EatCooldown;
                    CurrentTarget.RpcDestroy();
                    EatCount++;
                    CurrentTarget = null;
                    if (EatCount == 3)
                    {
                        ShipStatus.Instance.WinAlone(Player);
                    }
                }
            }));
        }
        public void Update()
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
            SetTarget(PlayerControl.LocalPlayer.GetClosestBody(0.5f));
        }
        public void SetTarget(DeadBody target)
        {
            CurrentTarget = target;
            if (CurrentTarget)
            {
                Button.spriteRender.color = Palette.EnabledColor;
                Button.spriteRender.material.SetFloat("_Desat", 0f);
                return;
            }
            Button.spriteRender.color = Palette.DisabledColor;
            Button.spriteRender.material.SetFloat("_Desat", 1f);
        }
    }
}
