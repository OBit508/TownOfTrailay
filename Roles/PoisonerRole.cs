using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class PoisonerRole : VampireRole
    {
        public override Color RoleColor => new Color32(197, 0, 255, byte.MaxValue);
        public override string roleDisplayName => "Poisoner";
        public override string roleDescription => "You can poison others";
        public float PoisonCooldown = 45;
        public new float KillDelay = 15;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanSabotage = true;
            CanVent = true;
        }
        public override void OnRoleAdded()
        {
            BiteCooldown = PoisonCooldown;
            base.KillDelay = KillDelay;
            Timer = BiteCooldown;
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Poison", TOTAssets.Poison, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = BiteCooldown;
                    Kills.Add((CurrentTarget, new ChangeableValue<float>(KillDelay)));
                }
            }));
        }
    }
}
