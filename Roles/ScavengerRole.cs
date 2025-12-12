using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Role;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class ScavengerRole : TOTBaseRole
    {
        public override Color RoleColor { get; } = Palette.ImpostorRed;
        public override string roleDisplayName => "Scavenger";
        public override string roleDescription => "You dont leave bodies.";
        public override bool CreateDeadBodyWheenKill => false;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanUseKillButton = true;
            CanVent = true;
            CanSabotage = true;
        }

    }
}
