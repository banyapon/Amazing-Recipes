using System;
using UnityEngine;
using Unity.Netcode;

namespace KitchenChaos.Interactions
{
    public class ContainerCounter : BaseCounter
    {
        public event Action OnPlayerGrabbedObject;

        [SerializeField] SO_KitchenObject _kitchenObjectSO;

        public override void Interact(PlayerInteractions player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                    player.GetKitchenObject().SetKitchenObjectHolder(this);
                else
                {
                    KitchenObject.SpawnKitchenObject(_kitchenObjectSO, player);
                    InteractLogicServerRpc();
                }
            }
            else
            {
                if (!player.HasKitchenObject())
                    GetKitchenObject().SetKitchenObjectHolder(player);
                else
                {
                    if (!AttemptTransferFromCounterToPlate(player))
                        AttemptTransferFromPlayerToPlate(player);
                }
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
            OnPlayerGrabbedObject?.Invoke();
        }
    }
}

