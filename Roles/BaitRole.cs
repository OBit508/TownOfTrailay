using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class BaitRole : RoleBehaviour
    {
        public override string roleDisplayName => "Bait";
        public override string roleDescription => "You are Bait. If someone kills you, he will automaticlly self-report";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Crewmate;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Neutral, RoleTeamTypes.Impostor };
        }
        public override void OnMurdered(DeathReason reason, PlayerControl killer = null)
        {
            if (reason == DeathReason.Kill && killer != null)
            {
                killer.ReportClosest();
            }
        }
    }
}