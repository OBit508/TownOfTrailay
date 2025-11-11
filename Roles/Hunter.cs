//[RegisterRole]
//public class Hunter : RoleBehaviour
//{
//    private int killCount = 0;

//    public override string roleDescription
//    {
//        get
//        {
//            string lang = TranslationController.Instance.CurrentLanguage.langName;
//            if (lang == "Portugu�s") return "Você é caçador.";
//            if (lang == "Polish") return "Jesteś Łowcą.";
//            return "You are Hunter.";
//        }
//    }

//    public override string roleDisplayName
//    {
//        get
//        {
//            string lang = TranslationController.Instance.CurrentLanguage.langName;
//            if (lang == "Portugu�s") return "Caçador";
//            if (lang == "Portugu�s") return "Łowca";
//            return "Hunter";
//        }
//    }

//    public override void ConfigureRole()
//    {
//        RoleTeamType = RoleTeamTypes.Neutral;
//        enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Impostor };
//        CanUseKillButton = true;
//        CanVent = true;
//        CanSabotage = false 
//    }

//    public override void OnKill(PlayerControl target)
//    {
//        base.OnKill(target);

//        killCount++;

//        if (killCount >= 3)
//        {
//            EndGame;
//        }
//    }
//}
