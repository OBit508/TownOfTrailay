public class CrewpostorRole : RoleBehaviour
{
    public override string roleDisplayName => "Crewpostor";

    public override string roleDescription
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Sabote a galera e mate geral.";
            if (lang == "Polish") return "Rób zadania albo zabijaj.";
            return "Do your tasks or kill.";
        }
    }

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
