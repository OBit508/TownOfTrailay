using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;

namespace TownOfTrailay.Helpers.Features
{
    internal class LoadHelper : HelperManager.Helper
    {
        public override void Awake()
        {
            HelperManager.Instance.StartCoroutine(CoLoadFeatures());
        }
        public System.Collections.IEnumerator CoLoadFeatures()
        {
            ModsManager modsManager = ModsManager.Instance;
            TextMeshPro fillText = (TextMeshPro)typeof(ModsManager).GetField("FillText", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(modsManager);
            while (fillText.isActiveAndEnabled)
            {
                yield return null;
            }
            modsManager.Fill.SetActive(true);
            fillText.text = "Loading TownOfTrailay Roles 0%";
            yield return TOTRoleManager.LoadRoles(fillText, "Loading TownOfTrailay Roles ");
            fillText.text = "Loading TownOfTrailay Cosmetics 0%";
            yield return CosmeticLoaderManager.LoadCosmetics(fillText, "Loading TownOfTrailay Cosmetics ");
            modsManager.Fill.SetActive(false);
        }
    }
}
