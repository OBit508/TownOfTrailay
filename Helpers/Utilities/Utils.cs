using Assets.CoreScripts;
using PowerTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using TMPro;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
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
        public static VanillaButtonManager CreateButton(Transform parent, RoleBehaviour role, string abilityName, Sprite abilitySprite, Action onClick)
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
        public static LimitedUsesButtonManager CreateUsesButton(Transform parent, RoleBehaviour role, string abilityName, Sprite abilitySprite, Action onClick)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(DestroyableSingleton<CachedMaterials>.Instance.abilityButton, parent).gameObject;
            AbilityButtonManager component = gameObject.GetComponent<AbilityButtonManager>();
            TextMeshPro abilityText = component.AbilityText;
            TextMeshPro cooldownText = component.CooldownText;
            SpriteRenderer spriteRender = component.spriteRender;
            UnityEngine.Object.Destroy(component);
            LimitedUsesButtonManager vanillaButtonManager = gameObject.AddComponent<LimitedUsesButtonManager>();
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
            SpriteRenderer counter = new GameObject("Counter")
            {
                layer = vanillaButtonManager.gameObject.layer,
                transform =
                {
                    parent = vanillaButtonManager.transform,
                    localPosition = new Vector3(-0.4f, 0.2f, -1),
                    localScale = Vector3.one * 0.9f
                }
            }.AddComponent<SpriteRenderer>();
            counter.sprite = TOTAssets.UsesCounter;
            vanillaButtonManager.UsesText = GameObject.Instantiate<TextMeshPro>(vanillaButtonManager.CooldownText, counter.transform);
            vanillaButtonManager.UsesText.transform.localPosition = new Vector3(0, 0.05f, -1);
            vanillaButtonManager.UsesText.transform.localScale = Vector3.one * 0.8f;
            vanillaButtonManager.UsesText.text = "";
            return vanillaButtonManager;
        }
        public static void CustomMurderPlayer(this PlayerControl player, PlayerControl target)
        {
            if (AmongUsClient.Instance.IsGameOver)
            {
                return;
            }
            GameData.PlayerInfo data = target.Data;
            if (player.AmOwner)
            {
                StatsManager instance = StatsManager.Instance;
                uint num = instance.ImpostorKills;
                instance.ImpostorKills = num + 1U;
                if (Constants.ShouldPlaySfx())
                {
                    SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 0.8f, false);
                }
            }
            if (player.Data.myRole)
            {
                player.Data.myRole.SetKillTimer(player.Data.myRole.KillCooldown);
            }
            DestroyableSingleton<Telemetry>.Instance.WriteMurder();
            target.gameObject.layer = LayerMask.NameToLayer("Ghost");
            if (target.AmOwner)
            {
                StatsManager instance2 = StatsManager.Instance;
                uint num = instance2.TimesMurdered;
                instance2.TimesMurdered = num + 1U;
                if (Minigame.Instance)
                {
                    Minigame.Instance.Close();
                    Minigame.Instance.Close();
                }
                DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(player.Data, data);
                DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(false);
                target.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                target.RpcSetScanner(false);
            }
            player.MyPhysics.StartCoroutine(CoPerformKill(player.KillAnimations.Random<KillAnimation>(), player, target));
            RoleBehaviour myRole = player.Data.myRole;
            if (myRole == null)
            {
                return;
            }
            myRole.OnMurder(target);
        }
        private static IEnumerator CoPerformKill(KillAnimation anim, PlayerControl source, PlayerControl target)
        {
            bool isParticipant = PlayerControl.LocalPlayer == source || PlayerControl.LocalPlayer == target;
            PlayerPhysics sourcePhys = source.MyPhysics;
            target.Die(DeathReason.Kill, source);
            SpriteAnim sourceAnim = source.myRend.GetComponent<SpriteAnim>();
            yield return new WaitForAnimationFinish(sourceAnim, anim.BlurAnim);
            sourceAnim.Play(sourcePhys.IdleAnim, 1f);
            KillAnimation.SetMovement(source, true);
            DeadBody deadBody = global::UnityEngine.Object.Instantiate<DeadBody>(anim.bodyPrefab);
            Vector3 vector = target.transform.position + anim.BodyOffset;
            vector.z = vector.y / 1000f;
            deadBody.transform.position = vector;
            deadBody.ParentId = target.PlayerId;
            target.SetPlayerMaterialColors(deadBody.MyRend);
            yield break;
        }
    }
}
