using Hazel;
using InnerNet;
using System;
using System.Collections.Generic;
using System.Linq;
using TownOfTrailay.Assets;
using TownOfTrailay.Helpers.Role;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace TownOfTrailay.Roles
{
    public class UncertainRole : TOTBaseRole
    {
        public override Color RoleColor => new Color32(174, 3, 81, byte.MaxValue);
        public override string roleDescription => "You are Uncertain. You can either help other impostors or be Traitor and make them lose.";
        public override string roleDisplayName => "Uncertain";
        public PlayerPickMenu Menu;
        public LimitedUsesButtonManager Button;
        public static float SwapCooldown = 10;
        public bool UsedAbility;
        public float Timer;
        public override void ConfigureRole()
        {
            RoleTeamType = RoleTeamTypes.Impostor;
            enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Crewmate, RoleTeamTypes.Neutral };
            CanVent = true;
            CanSabotage = true;
        }
        public override void OnRoleAdded()
        {
            Button = Utils.CreateUsesButton(HudManager.Instance.transform.Find("Buttons/BottomRight").transform, this, "Choose", TOTAssets.Choose, new Action(CreateMenu));
            Button.UsesText.text = "1";
            Timer = SwapCooldown;
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
                Button.spriteRender.color = UsedAbility ? Palette.DisabledColor : Color.white;
            }
        }
        public override void HandleRpc(MessageReader reader, int rpc)
        {
            switch ((RpcCalls)rpc)
            {
                case RpcCalls.RpcSwapTeam:
                    RoleTeamType = RoleTeamTypes.Crewmate;
                    enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Impostor, RoleTeamTypes.Neutral };
                    break;
            }
        }
        public void CreateMenu()
        {
            if (Menu == null && Timer <= 0 && !UsedAbility && HudManager.Instance != null)
            {
                Menu = GameObject.Instantiate(CachedMaterials.Instance.PPM);
                Menu.transform.SetParent(Camera.main.transform, worldPositionStays: false);
                Menu.transform.localPosition = new Vector3(0f, 0f, -50f);
                Menu.Begin(null);
                CreateCard(Menu, new Vector3(-3.8f, 0, -10), Vector3.one * 1.4f, TOTAssets.TraitorCard, delegate
                {
                    UsedAbility = true;
                    Button.UsesText.text = "0";
                    RoleTeamType = RoleTeamTypes.Crewmate;
                    enemyTeams = new RoleTeamTypes[] { RoleTeamTypes.Impostor, RoleTeamTypes.Neutral };
                    SendRpc(RpcCalls.RpcSwapTeam);
                    Menu.Close();
                });
                CreateCard(Menu, new Vector3(0.3f, 0, -10), Vector3.one * 1.4f, TOTAssets.HelperCard, delegate
                {
                    UsedAbility = true;
                    Button.UsesText.text = "0";
                    Menu.Close();
                });
            }
        }
        public void CreateCard(PlayerPickMenu playerPickMenu, Vector3 pos, Vector3 scale, Sprite card, Action onClick)
        {
            PPMButton button = GameObject.Instantiate<PPMButton>(playerPickMenu.Prefab, playerPickMenu.transform);
            SpriteRenderer rend = button.transform.GetChild(4).GetComponent<SpriteRenderer>();
            rend.material = new Material(Shader.Find("Sprites/Outline"));
            rend.sprite = card;
            ButtonRolloverHandler handler = rend.GetComponent<ButtonRolloverHandler>();
            CustomButtonRolloverHandler custom = rend.gameObject.AddComponent<CustomButtonRolloverHandler>();
            custom.Target = rend;
            custom.HoverSound = handler.HoverSound;
            custom.OverColor = Color.red;
            custom.OutColor = Color.clear;
            GameObject.Destroy(handler);
            rend.GetComponent<BoxCollider2D>().size = new Vector2(2, 0.55f);
            button.OnClick = onClick;
            for (int i = 0; i < button.transform.childCount; i++)
            {
                button.transform.GetChild(i).gameObject.SetActive(false);
            }
            rend.gameObject.SetActive(true);
            button.transform.localPosition = pos;
            rend.transform.localScale = new Vector3(1, 4.5f, 1);
            button.transform.localScale = scale;
        }
    }
}