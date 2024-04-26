using System;
using System.Collections.Generic;
using UnityEngine;
using KitchenChaos.Interactions;
using Unity.Netcode;

namespace KitchenChaos.Core
{
    public class DeliveryManager : NetworkBehaviour
    {
        public static DeliveryManager Instance { get; private set; }

        public event Action OnNewRecipeGenerated;
        public event Action OnRecipeDelivered;
        public event Action OnRecipeFailed;

        [SerializeField] SO_RecipeList _recipeListSO;
        [Tooltip("Sets the waiting time for the very first recipe")]
        [SerializeField] float _spawnRecipeTimer = 4f;
        [Tooltip("Sets the waiting time between recipes")]
        [SerializeField] float _spawnRecipeTimerMax = 4f;
        [SerializeField] int _pendingRecipesMax = 4;

        List<SO_FinalRecipe> _finalRecipesList;
        List<SO_FinalRecipe> _pendingRecipesList = new List<SO_FinalRecipe>();
        public List<SO_FinalRecipe> PendingRecipesList => _pendingRecipesList;

        int _successfulRecipesDelivered;
        public int SuccessfulRecipesDelivered => _successfulRecipesDelivered;

        int _deliveredRecipesScore;
        public int DeliveredRecipesScore => _deliveredRecipesScore;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _finalRecipesList = _recipeListSO.FinalRecipesList;
        }

        void Update()
        {
            if (!IsServer) return;
            if (!GameManager.Instance.IsGamePlaying()) return;

            _spawnRecipeTimer -= Time.deltaTime;

            GenerateNewRecipe();
        }

        void GenerateNewRecipe()
        {
            if (_spawnRecipeTimer <= 0f)
            {
                _spawnRecipeTimer = _spawnRecipeTimerMax;

                if (_pendingRecipesList.Count < _pendingRecipesMax)
                {
                    int pendingRecipeIndex = UnityEngine.Random.Range(0, _finalRecipesList.Count); 

                    GenerateNewRecipeClientRpc(pendingRecipeIndex);
                }
            }
        }

        public void DeliverRecipe(PlateKitchenObject plate)
        {
            foreach (var recipe in _pendingRecipesList)
            {
                if (DoIngredientsMatch(recipe, plate))
                {
                    int correctRecipeIndex = _pendingRecipesList.IndexOf(recipe);
                    DeliverCorrectRecipeServerRpc(correctRecipeIndex);

                    return;
                }
            }

            DeliverIncorrectRecipeServerRpc();
        }

        bool DoIngredientsMatch(SO_FinalRecipe recipe, PlateKitchenObject plate)
        {
            if (recipe.KitchenObjectSOList.Count != plate.KitchenObjectsList.Count)
                return false;

            foreach (var recipeIngredient in recipe.KitchenObjectSOList)
            {
                if (!plate.KitchenObjectsList.Contains(recipeIngredient))
                    return false;
            }

            return true;
        }

        [ClientRpc]
        void GenerateNewRecipeClientRpc(int pendingRecipeIndex)
        {
            SO_FinalRecipe recipe = _finalRecipesList[pendingRecipeIndex];
            _pendingRecipesList.Add(recipe);

            OnNewRecipeGenerated?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        void DeliverCorrectRecipeServerRpc(int correctRecipeIndex)
        {
            DeliverCorrectRecipeClientRpc(correctRecipeIndex);
        }

        [ClientRpc]
        void DeliverCorrectRecipeClientRpc(int correctRecipeIndex)
        {
            SO_FinalRecipe correctRecipe = _pendingRecipesList[correctRecipeIndex];

            _successfulRecipesDelivered++;
            _deliveredRecipesScore += correctRecipe.DeliveryPoints;

            _pendingRecipesList.RemoveAt(correctRecipeIndex);
            OnRecipeDelivered?.Invoke();
        }

        [ServerRpc(RequireOwnership = false)]
        void DeliverIncorrectRecipeServerRpc()
        {
            DeliverIncorrectRecipeClientRpc();
        }

        [ClientRpc]
        void DeliverIncorrectRecipeClientRpc()
        {
            OnRecipeFailed?.Invoke();
        }
    }
}
