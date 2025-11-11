using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfTrailay.Helpers.Utilities
{
    public static class Utils
    {
        public static void PrivateSetName(this PlayerControl player, string name)
        {
            player.nameText.Text = name;
            player.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 4);
        }
        public static Sprite LoadSprite(string path, float pixelsPerUnit = 1f)
        {
            Sprite sprite = null;
            try
            {
                Texture2D texture = LoadTextureFromResources(path);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
                sprite.hideFlags |= HideFlags.HideAndDontSave | HideFlags.DontSaveInEditor;
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading sprite: " + e.Message);
            }
            return sprite;
        }

        public static Texture2D LoadTextureFromResources(string path)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            texture.LoadImage(ms.ToArray(), false);
            return texture;
        }
        public static T DontDestroy<T>(this T obj) where T : UnityEngine.Object
        {
            obj.hideFlags |= HideFlags.HideAndDontSave;
            UnityEngine.Object.DontDestroyOnLoad(obj);
            return obj;
        }
        public static VanillaButtonManager Create(Transform parent, RoleBehaviour role, string abilityName, Sprite abilitySprite, Action onClick)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(DestroyableSingleton<CachedMaterials>.Instance.abilityButton, parent).gameObject;
            AbilityButtonManager component = gameObject.GetComponent<AbilityButtonManager>();
            TextMeshPro abilityText = component.AbilityText;
            TextMeshPro cooldownText = component.CooldownText;
            SpriteRenderer spriteRender = component.spriteRender;
            UnityEngine.Object.Destroy(component);
            VanillaButtonManager vanillaButtonManager = gameObject.AddComponent<VanillaButtonManager>();
            vanillaButtonManager.spriteRender = spriteRender;
            vanillaButtonManager.CooldownText = cooldownText;
            vanillaButtonManager.CooldownText.gameObject.SetActive(true);
            vanillaButtonManager.AbilityText = abilityText;
            vanillaButtonManager.TargetRole = role;
            vanillaButtonManager.spriteRender.sprite = abilitySprite;
            vanillaButtonManager.AbilityName = abilityName;
            vanillaButtonManager.onClick = onClick;
            vanillaButtonManager.Refresh();
            vanillaButtonManager.GetComponent<PassiveButton>().OnClick.RemoveAllListeners();
            vanillaButtonManager.GetComponent<PassiveButton>().OnClick.AddListener(new UnityAction(vanillaButtonManager.DoClick));
            return vanillaButtonManager;
        }
    }
}
