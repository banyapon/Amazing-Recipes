using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public interface IKitchenObjectHolder 
    {
        Transform GetKitchenObjectSpawnPoint();
        void SetKitchenObject(KitchenObject kitchenObject);
        KitchenObject GetKitchenObject();
        void ClearKitchenObject();
        bool HasKitchenObject();
        NetworkObject GetNetworkObject();
    }
}
