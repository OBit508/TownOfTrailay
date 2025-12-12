using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Role;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class JesterRole : TOTBaseRole
    {
        public override Color RoleColor { get; } = new Color32(196, 0, 222, byte.MaxValue);
        public override string roleDescription => "Get ejected to win.";
        public override string roleDisplayName => "Jester";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral, RoleTeamTypes.Impostor };
        }
        public override void OnMurdered(DeathReason reason, PlayerControl killer = null)
        {
            base.OnMurdered(reason, killer);
            if (reason == DeathReason.Exile)
            {
                ShipStatus.Instance.WinAlone(Player);
            }
        }
    }
}
