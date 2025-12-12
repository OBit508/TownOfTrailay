using Hazel;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Roles
{
    public class JanitorRole : TOTBaseRole
    {
        public override Color RoleColor { get; } = Palette.ImpostorRed;
        public override string roleDisplayName => "Janitor";
        public override string roleDescription => "Clears the body.";
        public DeadBody CurrentTarget;
        public VanillaButtonManager Button;
        public float CleanCooldown = 15;
        public float Timer;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanUseKillButton = true;
            CanVent = true;
            CanSabotage = true;
        }
        public override void OnRoleAdded()
        {
            Button = Utils.CreateButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Clean", TOTAssets.Clean, new Action(delegate
            {
                if (CurrentTarget != null && Timer <= 0)
                {
                    Timer = CleanCooldown;
                    RpcClean(CurrentTarget);
                }
            }));
        }
        public void Update()
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
            SetTarget(PlayerControl.LocalPlayer.GetClosestBody(0.5f));
        }
        public void RpcClean(DeadBody deadBody)
        {
            SendRpc(RpcCalls.RpcClean, new Action<MessageWriter>(delegate (MessageWriter messageWriter)
            {
                messageWriter.Write(deadBody.ParentId);
            }));
            deadBody.StartCoroutine(CoCleanDeadBody(deadBody));
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcClean:
                    byte id = reader.ReadByte();
                    DeadBody body = GameObject.FindObjectsOfType<DeadBody>().FirstOrDefault((DeadBody deadBody) => deadBody.ParentId == id);
                    if (body != null)
                    {
                        body.StartCoroutine(CoCleanDeadBody(body));
                    }
                    break;
            }
        }
        public System.Collections.IEnumerator CoCleanDeadBody(DeadBody deadBody)
        {
            deadBody.MyRend.color = Color.white;
            while (deadBody.MyRend.color.a > 0)
            {
                yield return new WaitForSeconds(0.1f);
                Color color = deadBody.MyRend.color;
                color.a -= 0.1f;
                deadBody.MyRend.color = color;
                yield return null;
            }
            GameObject.Destroy(deadBody.gameObject);
        }
        public void SetTarget(DeadBody target)
        {
            CurrentTarget = target;
            if (CurrentTarget)
            {
                Button.spriteRender.color = Palette.EnabledColor;
                Button.spriteRender.material.SetFloat("_Desat", 0f);
                return;
            }
            Button.spriteRender.color = Palette.DisabledColor;
            Button.spriteRender.material.SetFloat("_Desat", 1f);
        }
    }
}
