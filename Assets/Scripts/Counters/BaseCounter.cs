using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public class BaseCounter : NetworkBehaviour, IKitchenObjectHolder
    {
        public static event Action<BaseCounter> OnAnyObjectPlaced;

        [SerializeField] Transform _spawnPoint;

        KitchenObject _kitchenObject;

        public virtual void Interact(PlayerInteractions player)
        {
            Debug.Log("BaseCounter.Interact");
        }

        public void ClearKitchenObject()
        {
            _kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return (_kitchenObject != null);
        }

        public Transform GetKitchenObjectSpawnPoint()
        {
            return _spawnPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            _kitchenObject = kitchenObject;

            if (_kitchenObject != null)
                OnAnyObjectPlaced?.Invoke(this);
        }

        public KitchenObject GetKitchenObject()
        {
            return _kitchenObject;
        }

        public virtual void InteractAlt(PlayerInteractions player)
        {
            Debug.Log("BaseCounter.InteractAlt");
        }

        public bool AttemptTransferFromCounterToPlate(PlayerInteractions player)
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
            {
                plate.TryAddIngredient(GetKitchenObject().KitchenObjectSO);
                KitchenObject.DestroyKitchenObject(GetKitchenObject());

                return true;
            }
            return false;
        }

        public void AttemptTransferFromPlayerToPlate(PlayerInteractions player)
        {
            if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
            {
                KitchenObject playerKitchenObject = player.GetKitchenObject();

                if (plate.TryAddIngredient(playerKitchenObject.KitchenObjectSO))
                    KitchenObject.DestroyKitchenObject(playerKitchenObject);
            }
        }

        public static void ResetStaticData()
        {
            OnAnyObjectPlaced = null;
        }

        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    }
}
