[RegisterRole]
public class Sabotager : RoleBehaviour
{
    public override string roleDisplayName
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portuguese") return "Sabotador";
            if (lang == "Polish") return "Sabotażysta";
            return "Sabotager";
        }
    }

    public override string roleDescription
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portuguese") return "Você é um Sabotador. Sabote e mate.";
            if (lang == "Polish") return "Jestes Sabotażysta. Sabotuj i zabijaj.";
            return "You are Sabotager. Sabotage and kill.";
        }
    }

    public override void ConfigureRole()
    {
        RoleTeamType = RoleTeamTypes.Impostor;
        enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
        CanUseKillButton = true;
        CanVent = false;
        CanSabotage = true;
    }
}
