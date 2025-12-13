using Hazel;
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
    public class DiggerRole : TOTBaseRole
    {
        public override Color RoleColor { get; } = Palette.ImpostorRed;
        public override string roleDescription => "You can dig a vent.";
        public override string roleDisplayName => "Digger";
        public LimitedUsesButtonManager Button;
        public static float DigCooldown = 15;
        public int UsesCount = 5;
        public float Timer;
        public List<Vent> Vents;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanVent = true;
            CanSabotage = true;
            CanUseKillButton = true;
        }
        public override void OnRoleAdded()
        {
            Button = Utils.CreateUsesButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Dig Vent", TOTAssets.Dig, new Action(delegate
            {
                if (Timer <= 0 && UsesCount > 0)
                {
                    UsesCount--;
                    Button.UsesText.text = UsesCount.ToString();
                    Timer = DigCooldown;
                    RpcCreateVent(Player.transform.position);
                }
            }));
            Button.UsesText.text = UsesCount.ToString();
            Timer = DigCooldown;
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
                Button.spriteRender.color = UsesCount <= 0 ? Palette.DisabledColor : Color.white;
            }
        }
        public void RpcCreateVent(Vector2 position)
        {
            Vents.Add(Utils.CreateVent(position));
            UpdateConnections();
            SendRpc(RpcCalls.RpcCreateVent, new Action<MessageWriter>(delegate (MessageWriter messageWriter) { NetHelpers.WriteVector2(position, messageWriter); }));
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcCreateVent:
                    Vector2 position = NetHelpers.ReadVector2(reader);
                    Vents.Add(Utils.CreateVent(position));
                    UpdateConnections();
                    break;
            }
        }
        public void UpdateConnections()
        {
            if (Vents.Count <= 1)
            {
                return;
            }
            for (int i = 0; i < Vents.Count; i++)
            {
                Vents[i].Left = Vents[(i - 1 + Vents.Count) % Vents.Count];
                Vents[i].Right = Vents[(i + 1) % Vents.Count];
            }
        }
    }
}
