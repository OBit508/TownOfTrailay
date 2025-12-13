using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfTrailay.Patches
{
    [HarmonyPatch(typeof(DraggerRole), "CreateDragButton")]
    internal static class DraggerRolePatch
    {
        public static bool Prefix(DraggerRole __instance, [HarmonyArgument(0)] Transform parent, ref VanillaButtonManager __result)
        {
            GameObject gameObject = global::UnityEngine.Object.Instantiate<AbilityButtonManager>(DestroyableSingleton<CachedMaterials>.Instance.abilityButton, parent).gameObject;
            AbilityButtonManager component = gameObject.GetComponent<AbilityButtonManager>();
            TextMeshPro abilityText = component.AbilityText;
            TextMeshPro cooldownText = component.CooldownText;
            SpriteRenderer spriteRender = component.spriteRender;
            GameObject.Destroy(component);
            VanillaButtonManager vanillaButtonManager = gameObject.AddComponent<VanillaButtonManager>();
            vanillaButtonManager.spriteRender = spriteRender;
            vanillaButtonManager.CooldownText = cooldownText;
            vanillaButtonManager.AbilityText = abilityText;
            vanillaButtonManager.TargetRole = __instance;
            vanillaButtonManager.SpriteName = "draggerSprite";
            vanillaButtonManager.AbilityName = Translator.GetString("DragButton");
            vanillaButtonManager.onClick = new Action(__instance.CheckDrag);
            vanillaButtonManager.Refresh();
            vanillaButtonManager.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
            vanillaButtonManager.GetComponent<PassiveButton>().OnClick.AddListener(new UnityAction(vanillaButtonManager.DoClick));
            __result = vanillaButtonManager;
            return false;
        }
    }
}
