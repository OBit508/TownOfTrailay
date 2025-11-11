using System.Linq;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class HunterRole : RoleBehaviour
    {
        public static int RequiredKills = 3;
        private int killCount = 0;
        public override string roleDescription => "You are Hunter.";
        public override string roleDisplayName => "Hunter";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
            CanUseKillButton = true;
            CanVent = true;
        }
        public override void OnMurder(PlayerControl target)
        {
            base.OnMurder(target);
            killCount++;
            if (killCount >= RequiredKills)
            {
                ShipStatus.Instance.WinAlone(Player);
            }
        }
    }
}