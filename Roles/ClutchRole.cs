using System.Linq;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class ClutchRole : TOTBaseRole
    {
        public int alivePlayerCount = 0;
        private bool wasBelowThreshold = false;
        public override Color RoleColor => new Color32(118, 0, 0, byte.MaxValue);
        public override string roleDisplayName => "Clutch";
        public override string roleDescription => "You are Clutch. You can kill and vent only if there are 7 or fewer people alive.";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanUseKillButton = true;
            CanVent = true;
            CanSabotage = true;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            int previousCount = alivePlayerCount;
            alivePlayerCount = GameData.Instance.AllPlayers.Count(p => !p.IsDead);
            bool nowBelow = alivePlayerCount > 8;
            if (nowBelow != wasBelowThreshold)
            {
                wasBelowThreshold = nowBelow;
                if (nowBelow)
                {
                    CanUseKillButton = false;
                    CanVent = false;
                }
                else
                {
                    CanUseKillButton = true;
                    CanVent = true;
                }
                HudManager.Instance.SetHudActive(true);
            }
        }
    }
}