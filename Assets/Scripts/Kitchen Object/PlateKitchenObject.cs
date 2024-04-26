using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using KitchenChaos.Multiplayer;

namespace KitchenChaos.Interactions
{
    public class PlateKitchenObject : KitchenObject
    {
        public event Action<SO_KitchenObject> OnIngredientAdded;

        [SerializeField] List<SO_KitchenObject> _acceptableKitchenObjects;

        List<SO_KitchenObject> _kitchenObjects = new List<SO_KitchenObject>();
        public List<SO_KitchenObject> KitchenObjectsList => _kitchenObjects;

        public bool TryAddIngredient(SO_KitchenObject kitchenObjectSO)
        {
            if (!_acceptableKitchenObjects.Contains(kitchenObjectSO))
                return false;
            else
            {
                AddIngredientServerRpc(GameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSO));
                return true;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void AddIngredientServerRpc(int kitchenObjectSOIndex)
        {
            AddIngredientClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        void AddIngredientClientRpc(int kitchenObjectSOIndex)
        {
            SO_KitchenObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

            _kitchenObjects.Add(kitchenObjectSO);
            OnIngredientAdded?.Invoke(kitchenObjectSO);
        }
    }
}
