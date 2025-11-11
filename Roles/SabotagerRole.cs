using System.Linq;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class SabotagerRole : RoleBehaviour
    {
        public override string roleDisplayName => "Sabotager";
        public override string roleDescription => "You are Sabotager. Sabotage and kill.";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanUseKillButton = true;
            CanSabotage = true;
        }
    }

}