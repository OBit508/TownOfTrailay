using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Roles;
using UnityEngine;
using static UnityEngine.PlayerLoop.PreUpdate;

namespace TownOfTrailay.Helpers.Role
{
    public class ButtonSpriteHelper : HelperManager.Helper
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
                RoleBehaviour role = PlayerControl.LocalPlayer.Data.myRole;
                HudManager.Instance.KillButton.renderer.sprite = role is ClutchRole ? TOTAssets.ClutchKill : (role is SheriffRole || role is HunterRole ? TOTAssets.YellowKill : role is JuggernautRole  ? TOTAssets.JuggerKill : role is PelicanRole ? TOTAssets.Eat : KillButton);
                if (role is DraggerRole dragger)
                {
                    dragger.dragButton.spriteRender.sprite = TOTAssets.Drag;
                }
            }
        }
    }
}
