using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Roles;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreUpdate;

namespace TownOfTrailay.Helpers.Role
{
    public class ClutchHelper : HelperManager.Helper
    {
        public static Sprite KillButton;
        public override void Update()
        {
            if (HelperManager.CanUpdate && HudManager.Instance != null && PlayerControl.LocalPlayer != null && PlayerControl.LocalPlayer.Data != null)
            {
                if (KillButton == null)
                {
                    KillButton = HudManager.Instance.KillButton.renderer.sprite;
                }
                HudManager.Instance.KillButton.renderer.sprite = PlayerControl.LocalPlayer.Data.myRole is ClutchRole ? TOTAssets.ClutchKill : KillButton;
            }
        }
    }
}
