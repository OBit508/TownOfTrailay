using System.Linq;

[RegisterRole]
public class Clutch : RoleBehaviour
{
    public int alivePlayerCount = 0;
    private bool wasBelowThreshold = false;

    public override string roleDisplayName
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portuguese") return "Clutch";
            if (lang == "Polish") return "Clutch";
            return "Clutch";
        }
    }

    public override string roleDescription
    {
        get
        {
            string lang = TranslationController.Instance.CurrentLanguage.langName;
            if (lang == "Portugu�s") return "Você é Clutch. Você pode matar e desabafar apenas se houver 7 ou menos pessoas vivas.";
            if (lang == "Polish") return "Jesteś Clutchem. Możesz zabijać i ventować gdy pozostanie 7 osób lub mniej.";
            return "You are Clutch. You can kill and vent only if there are 7 or fewer people alive.";
        }
    }

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