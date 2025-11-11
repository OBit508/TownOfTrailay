using System.Linq;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class SerialKillerImpostorRole : RoleBehaviour
    {
        public override string roleDisplayName => "Serial killer";
        public override string roleDescription => "You are Serial killer. kill everyone to win";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
            CanUseKillButton = true;
        }
    }
}
