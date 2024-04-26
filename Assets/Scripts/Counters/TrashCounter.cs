using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public class TrashCounter : BaseCounter
    {
        public static Action<TrashCounter> OnAnyObjectTrashed;

        //prototype code - implement pooling
        public override void Interact(PlayerInteractions player)
        {
            if (player.HasKitchenObject())
            {
                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());

                InteractLogicServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void InteractLogicServerRpc()
        {
            InteractLogicClientRpc();
        }

        [ClientRpc]
        void InteractLogicClientRpc()
        {
            OnAnyObjectTrashed?.Invoke(this);
        }


        new public static void ResetStaticData()
        {
            OnAnyObjectTrashed = null;
        }
    }
}
