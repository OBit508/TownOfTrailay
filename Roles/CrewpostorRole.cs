using System.Linq;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class CrewpostorRole : TOTBaseRole
    {
        public override Color RoleColor => new Color32(164, 22, 48, byte.MaxValue);
        public override string roleDisplayName => "Crewpostor";
        public override string roleDescription => "Do your tasks or kill.";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Crewmate;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral, RoleTeamTypes.Impostor };
            CanUseKillButton = true;
            CanVent = true;
            CanSabotage = true;
        }

        public override void OnAssign(PlayerControl player)
        {
            base.OnAssign(player);
            HudManager.Instance.KillButton.ButtonText.fontMaterial = CachedMaterials.Instance.BrookMaterials[1];
        }
    }

}