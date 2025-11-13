using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfTrailay.Helpers.Role
{
    public class TOTBaseRole : RoleBehaviour
    {
        public bool LocalPlayer => Player != null && Player.AmOwner;
        private void Start()
        {
            if (LocalPlayer)
            {
                OnRoleAdded();
            }
        }
        public void SendRpc(RpcCalls rpcId, Action<MessageWriter> rpc = null)
        {
            MessageWriter writer = Player.StartRoleRpc((int)rpcId);
            rpc?.Invoke(writer);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
        }
        public virtual void OnRoleAdded()
        {

        }

    }
}
