using KitchenChaos.Core;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public class PlatesCounter : BaseCounter
    {
        public event Action OnPlateSpawned;
        public event Action OnPlateRemoved;

        [SerializeField] SO_KitchenObject _plateSO;
        [SerializeField] float _spawnPlateTimerMax = 4f;
        [SerializeField] int _platesSpawnedAmountMax = 4;

        float spawnPlateTimer;
        int _platesSpawnedAmount;

        private void Update()
        {
            if (!IsServer) return;
            if (!GameManager.Instance.IsGamePlaying()) return;

            spawnPlateTimer += Time.deltaTime;

            if (spawnPlateTimer > _spawnPlateTimerMax)
            {
                spawnPlateTimer = 0f;

                if (_platesSpawnedAmount < _platesSpawnedAmountMax)
                {
                    SpawnPlateServerRpc();
                }
            }
        }

        [ServerRpc]
        void SpawnPlateServerRpc()
        {
            SpawnPlateClientRpc();
        }

        [ClientRpc]
        void SpawnPlateClientRpc()
        {
            _platesSpawnedAmount++;
            OnPlateSpawned?.Invoke();
        }


        public override void Interact(PlayerInteractions player)
        {
            if (!player.HasKitchenObject())
            {
                if (_platesSpawnedAmount > 0)
                {
                    KitchenObject.SpawnKitchenObject(_plateSO, player);
                    InteractLogicServerRpc();
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
            _platesSpawnedAmount--;
            OnPlateRemoved?.Invoke();
        }
    }
}
