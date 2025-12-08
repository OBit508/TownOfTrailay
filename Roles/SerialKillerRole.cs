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
        public override Color RoleColor => new Color32(3, 90, 13, byte.MaxValue);
        public override string roleDisplayName => "Serial killer";
        public override string roleDescription => "You are Serial killer. kill everyone to win";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor, RoleTeamTypes.Neutral };
            CanUseKillButton = true;
        }
    }
}
