public class Bait : RoleBehaviour
{
    public override string roleDisplayName
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Bait";
            if (lang == "Polish") return "Bait";
            return "Zasadzka (Bait)";
        }
    }

    public override string roleDescription
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Você pode matar.";
            if (lang == "Polish") return "Jesteś Zaszadzką (Biat). Gdy Cię ktoś zabiję, to automatycznie zrobi self-report";
            return "You are Bait. If someone kills you, he will automaticlly self-report";
        }
    }

    public override void ConfigureRole()
    {
        RoleTeamType = RoleTeamTypes.Crewmate;
        enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Neutral, RoleTeamTypes.Impostor };
        CanUseKillButton = false;
        CanVent = false;
        CanSabotage = false;
    }
}
