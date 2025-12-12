using Hazel;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class MedusaRole : TOTBaseRole
    {
        public override Color RoleColor { get; } = new Color32(0, 99, 13, byte.MaxValue);
        public override string roleDisplayName => "Medusa";
        public override string roleDescription => "You can petrify others.";
        public VanillaButtonManager Button;
        public float PetrifyCooldown = 25;
        public float Timer;
        public PlayerControl CurrentTarget;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Neutral;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Impostor, RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
        }
        public override void OnRoleAdded()
        {
            Timer = PetrifyCooldown;
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Petrify", HudManager.Instance.KillButton.renderer.sprite, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = PetrifyCooldown;
                    RpcPetrify(CurrentTarget);
                }
            }));
        }
        public void Update()
        {
            if (LocalPlayer)
            {
                if (Timer > 0)
                {
                    Timer -= Time.deltaTime;
                    if (Timer < 0)
                    {
                        Timer = 0;
                    }
                }
                Button.CooldownText.text = Timer > 0 ? ((int)Timer).ToString() : "";
                SetTarget(Player.FindClosestTarget());
            }
        }
        public void RpcPetrify(PlayerControl target)
        {
            Player.PetrifyPlayer(target);
            SendRpc(RpcCalls.RpcPetrify, new Action<MessageWriter>(delegate (MessageWriter messageWriter)
            {
                messageWriter.WriteNetObject(target);
            }));
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcPetrify:
                    PlayerControl target = reader.ReadNetObject<PlayerControl>();
                    Player.PetrifyPlayer(target);
                    break;
            }
        }
        public void SetTarget(PlayerControl target)
        {
            if (CurrentTarget && CurrentTarget != target)
            {
                CurrentTarget.myRend.material.SetFloat("_Outline", 0f);
            }
            CurrentTarget = target;
            if (CurrentTarget)
            {
                SpriteRenderer myRend = CurrentTarget.myRend;
                myRend.material.SetFloat("_Outline", 1f);
                myRend.material.SetColor("_OutlineColor", RoleColor);
                Button.spriteRender.color = Palette.EnabledColor;
                Button.spriteRender.material.SetFloat("_Desat", 0f);
                return;
            }
            Button.spriteRender.color = Palette.DisabledColor;
            Button.spriteRender.material.SetFloat("_Desat", 1f);
        }
    }
}
