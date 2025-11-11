using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Roles;
using UnityEngine;
using static TownOfTrailay.Roles.TimeMasterRole;

namespace TownOfTrailay.Helpers
{
    internal class RoleHelper : MonoBehaviour
    {
        public static SpriteRenderer Background;
        public static List<TimePoint> GlobalPoints = new List<TimePoint>();
        public static bool RewindActive;
        public static bool LastShipCheck;
        public static int MaxPoints = 720;
        public static Sprite KillButton;
        public void Update()
        {
            TimeMasterUpdate();
        }
        public void UpdateButtonsSprite()
        {
            if (AmongUsClient.Instance.IsGameStarted && HudManager.Instance != null && PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data != null)
            {
                if (KillButton == null)
                {
                    KillButton = HudManager.Instance.KillButton.renderer.sprite;
                }
                HudManager.Instance.KillButton.renderer.sprite = PlayerControl.LocalPlayer.Data.myRole is ClutchRole ? TOUAssets.ClutchKill : KillButton;
            }
        }
        public void TimeMasterUpdate()
        {
            if (!LastShipCheck && ShipStatus.Instance != null && PlayerControl.LocalPlayer != null && PlayerControl.AllPlayerControls.Count > 0)
            {
                GlobalPoints.Clear();
            }
            LastShipCheck = ShipStatus.Instance != null;
            if (AmongUsClient.Instance.IsGameStarted)
            {
                if (!RewindActive)
                {
                    GlobalPoints.Add(new TimePoint());
                    if (GlobalPoints.Count >= MaxPoints + 1)
                    {
                        GlobalPoints.RemoveAt(0);
                    }
                }
                else
                {
                    if (GlobalPoints.Count > 0)
                    {
                        TimePoint point = GlobalPoints[GlobalPoints.Count - 1];
                        point.FixedUpdate();
                        GlobalPoints.Remove(point);
                    }
                    else
                    {
                        RewindActive = false;
                    }
                }
                if (Background == null && HudManager.Instance != null && HudManager.Instance.FullScreen != null)
                {
                    Background = Instantiate(HudManager.Instance.FullScreen, HudManager.Instance.FullScreen.transform.parent);
                    Background.transform.localPosition = new Vector3(0, 0, -751);
                    Background.color = new Color(0, 0, 1, 0.5f);
                }
                if (Background != null)
                {
                    Background.enabled = RewindActive;
                }
            }
        }
    }
}
