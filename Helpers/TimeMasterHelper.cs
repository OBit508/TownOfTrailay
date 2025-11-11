using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static TownOfTrailay.Roles.TimeMaster;

namespace TownOfTrailay.Helpers
{
    internal class TimeMasterHelper : MonoBehaviour
    {
        public static SpriteRenderer Background;
        public static List<TimePoint> GlobalPoints = new List<TimePoint>();
        public static bool RewindActive;
        public static bool LastShipCheck;
        public static int MaxPoints = 720;
        public void Start()
        {
            GlobalPoints.Clear();
        }
        public void Update()
        {
            if (!LastShipCheck && ShipStatus.Instance != null && PlayerControl.LocalPlayer != null && PlayerControl.AllPlayerControls.Count > 0)
            {
                GlobalPoints.Clear();
            }
            LastShipCheck = ShipStatus.Instance != null;
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
