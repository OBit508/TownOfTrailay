using Assets.CoreScripts;
using PowerTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using TownOfTrailay.Assets;
using TownOfTrailay.Components;
using TownOfTrailay.Helpers.Role;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfTrailay.Helpers.Utilities
{
    public static class Utils
    {
        internal static MethodInfo ShipStatus_AllVents_Set = typeof(ShipStatus).GetProperty("AllVents").GetSetMethod(true);
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
        public static void ReportDeadBody(this PlayerControl player, GameData.PlayerInfo target)
        {
            typeof(PlayerControl).GetMethod("ReportDeadBody", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(player, new object[] { target });
        }
        public static Vent CreateVent(Vector2 position, Vent left = null, Vent center = null, Vent right = null)
        {
            Vent vent = GameObject.Instantiate<Vent>(ShipStatus.Instance.AllVents[0]);
            vent.Id = ShipStatus.Instance.AllVents.Count();
            ShipStatus_AllVents_Set.Invoke(ShipStatus.Instance, new object[] { ShipStatus.Instance.AllVents.Concat(new Vent[] { vent }).ToArray() });
            vent.Right = right;
            vent.Center = center;
            vent.Left = left;
            vent.transform.position = new Vector3(position.x, position.y, position.y / 1000 + 0.001f);
            return vent;
        }
        public static void PetrifyPlayer(this PlayerControl player, PlayerControl target)
        {
            MedusaStatue statue = new GameObject(target.name + " - Statue").AddComponent<MedusaStatue>();
            statue.transform.position = target.transform.position;
            statue.transform.localScale = target.transform.localScale;
            statue.colorId = target.Data.ColorId;
            statue.BodyRender = new GameObject("BodyRend") 
            { 
                transform =
                {
                    parent = statue.transform,
                    position = target.MyPhysics.CurBody.transform.position,
                    localScale = target.MyPhysics.CurBody.transform.localScale
                }
            }.AddComponent<SpriteRenderer>();
            statue.BodyRender.sprite = target.MyPhysics.CurBody.Rend.sprite;
            statue.BodyRender.material = target.MyPhysics.CurBody.Rend.material;
            statue.BodyRender.flipX = target.MyPhysics.CurBody.Rend.flipX;
            Transform cosmeticParent = new GameObject("CosmeticParent") 
            {
                transform = 
                { 
                    parent = statue.transform,
                    localPosition = Vector3.zero,
                    localScale = Vector3.one * 0.5f
                }
            }.transform;
            SpriteRenderer skinRend = new GameObject("SkinRend")
            {
                transform =
                {
                    parent = cosmeticParent,
                    position = target.MyPhysics.Skin.transform.position,
                    localScale = target.MyPhysics.Skin.transform.localScale
                }
            }.AddComponent<SpriteRenderer>();
            skinRend.sprite = target.MyPhysics.Skin.layer.sprite;
            skinRend.flipX = statue.BodyRender.flipX;
            SpriteRenderer hatRend = new GameObject("HatRend")
            {
                transform =
                {
                    parent = cosmeticParent,
                    position = target.HatRenderer.transform.position,
                    localScale = target.HatRenderer.transform.localScale
                }
            }.AddComponent<SpriteRenderer>();
            hatRend.sprite = target.HatRenderer.sprite;
            hatRend.flipX = statue.BodyRender.flipX;
            TextMeshPro nameText = GameObject.Instantiate<TextMeshPro>(target.transform.GetChild(2).GetComponent<TextMeshPro>(), statue.transform);
            nameText.transform.position = target.transform.GetChild(2).position;
            GameObject.Destroy(nameText.GetComponent<TextRenderer>());
            nameText.text = target.Data.PlayerName + " - Statue";
            player.CustomMurderPlayer(target, true, false, false, false, false);
        }
        public static void CustomMurderPlayer(this PlayerControl player, PlayerControl target, bool resetKillTimer = true, bool createDeadBody = true, bool teleportMurderer = true, bool showKillAnim = true, bool playKillSound = true)
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
                if (Constants.ShouldPlaySfx() && playKillSound)
                {
                    SoundManager.Instance.PlaySound(PlayerControl.LocalPlayer.KillSfx, false, 0.8f, false);
                }
            }
            if (player.Data.myRole && resetKillTimer)
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
                if (showKillAnim)
                {
                    DestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(player.Data, data);
                }
                DestroyableSingleton<HudManager>.Instance.ShadowQuad.gameObject.SetActive(false);
                target.nameText.GetComponent<MeshRenderer>().material.SetInt("_Mask", 0);
                target.RpcSetScanner(false);
            }
            player.MyPhysics.StartCoroutine(CoPerformKill(player.KillAnimations.Random<KillAnimation>(), player, target, resetKillTimer, createDeadBody, teleportMurderer, showKillAnim, playKillSound));
            RoleBehaviour myRole = player.Data.myRole;
            if (myRole == null)
            {
                return;
            }
            myRole.OnMurder(target);
        }

        public static System.Collections.IEnumerator CoPerformKill(KillAnimation killAnimation, PlayerControl source, PlayerControl target, bool resetKillTimer = true, bool createDeadBody = true, bool teleportMurderer = true, bool showKillAnim = true, bool playKillSound = true)
        {
            bool isParticipant = PlayerControl.LocalPlayer == source || PlayerControl.LocalPlayer == target;
            PlayerPhysics sourcePhys = source.MyPhysics;
            if (teleportMurderer)
            {
                KillAnimation.SetMovement(source, false);
                KillAnimation.SetMovement(target, false);
            }
            if (isParticipant)
            {
                Camera.main.GetComponent<FollowerCamera>().Locked = true;
            }
            target.Die(DeathReason.Kill, source);
            SpriteAnim sourceAnim = source.myRend.GetComponent<SpriteAnim>();
            yield return new WaitForAnimationFinish(sourceAnim, killAnimation.BlurAnim);
            if (teleportMurderer)
            {
                source.NetTransform.SnapTo(target.transform.position);
            }
            sourceAnim.Play(sourcePhys.IdleAnim, 1f);
            if (resetKillTimer)
            {
                KillAnimation.SetMovement(source, true);
            }
            if (createDeadBody)
            {
                DeadBody deadBody = global::UnityEngine.Object.Instantiate<DeadBody>(killAnimation.bodyPrefab);
                Vector3 vector = target.transform.position + killAnimation.BodyOffset;
                vector.z = vector.y / 1000f;
                deadBody.transform.position = vector;
                deadBody.ParentId = target.PlayerId;
                target.SetPlayerMaterialColors(deadBody.MyRend);
            }
            if (teleportMurderer)
            {
                KillAnimation.SetMovement(target, true);
            }
            if (isParticipant)
            {
                Camera.main.GetComponent<FollowerCamera>().Locked = false;
            }
            yield break;
        }
    }
}
