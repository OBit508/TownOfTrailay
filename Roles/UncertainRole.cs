using System.Linq;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class UncertainRole : RoleBehaviour
    {
        public override string roleDescription => "You are Uncertain. You can either help other impostors or be Traitor and make them lose.";
        public override string roleDisplayName => "Uncertain";
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor, RoleTeamTypes.Neutral };
            CanVent = true;
        }
    }

}