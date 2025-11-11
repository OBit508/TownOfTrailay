[RegisterRole]
public class Uncertain : RoleBehaviour
{
    public override string roleDescription
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Você é Incerto. Você pode ajudar outros impostores ou ser um Traidor e fazê-los perder.";
            if (lang == "Polish") return "Jesteś Niepewny. Możesz pomóc innym impostorom, lub być Zdrajcą i sprawić, że przegrają.";
            return "You are Uncertain. You can either help other impostors or be Traitor and make them lose.";
        }
    }

    public override string roleDisplayName
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Incerto";
            if (lang == "Portugu�s") return "Niepewny";
            return "Uncertain";
        }
    }

    public override void ConfigureRole()
    {
        RoleTeamType = RoleTeamTypes.Impostor;
        enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor, RoleTeamTypes.Neutral };
        CanUseKillButton = false;
        CanVent = true;
        CanSabotage = false;
    }
}
