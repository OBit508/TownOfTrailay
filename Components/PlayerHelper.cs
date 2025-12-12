using System;
using System.Collections.Generic;
using System.Text;
using TownOfTrailay.Helpers.Utilities;
using UnityEngine;

namespace TownOfTrailay.Components
{
    public class PlayerHelper : MonoBehaviour
    {
        public PlayerControl Player;
        public RoleBehaviour Role => Player.Data.myRole;
        private List<(PlayerControl, ChangeableValue<float>, bool)> DelayedMurderers = new List<(PlayerControl, ChangeableValue<float>, bool)>();
        public void Start()
        {
            Player = GetComponent<PlayerControl>();
        }
        public void Update()
        {
            for (int i = DelayedMurderers.Count - 1; i >= 0; i--)
            {
                (PlayerControl, ChangeableValue<float>, bool) pair = DelayedMurderers[i];
                pair.Item2.Value -= Time.deltaTime;
                if (pair.Item2.Value <= 0)
                {
                    if (pair.Item1 && !Player.Data.IsDead && !Player.Data.Disconnected)
                    {
                        Player.RpcCustomMurder(pair.Item1, MurderResultFlags.Succeeded, true, true, pair.Item3);
                    }
                }
            }
        }
        public void AddDelayedKill(PlayerControl target, float delay, bool teleportWheenKill)
        {
            DelayedMurderers.Add((target, new ChangeableValue<float>(delay), teleportWheenKill));
        }
    }
}
