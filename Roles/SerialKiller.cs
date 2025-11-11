[RegisterRole]
public class SerialKillerImpostorRole : RoleBehaviour
{
    public override string roleDisplayName
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Serial killer";
            if (lang == "Polish") return "Seryjny Morderca";
            return "Serial killer";
        }
    }

    public override string roleDescription
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Você pode matar.";
            if (lang == "Polish") return "Jesteś Seryjnym Zabójcą. Zabij wszystkich aby wygrać";
            return "You are Serial killer. kill everyone to win";
        }
    }

    public override void ConfigureRole()
    {
        RoleTeamType = RoleTeamTypes.Neutral;
        enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
        CanUseKillButton = true;
        CanVent = false;
        CanSabotage = false;
    }
}
