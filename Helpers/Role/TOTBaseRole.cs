using Hazel;
using System;
using System.Collections.Generic;
using System.Text;

namespace TownOfTrailay.Helpers.Role
{
    public class TOTBaseRole : RoleBehaviour
    {
        private void Start()
        {
            if (Player.AmOwner)
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
