using System;
using Unity.Netcode;
using UnityEngine;
using KitchenChaos.Multiplayer;

namespace KitchenChaos.Interactions
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event Action<CuttingCounter> OnAnyCut;
        public event Action<float> OnProgressChanged;
        public event Action OnCut;

        SO_CuttingRecipe _currentCuttingRecipe;
        
        // TRY getting the kitchenobject SO from index (GameMultiplayer) instead of this
        // and getting the recipe from that in SetRecipe etc.
        [SerializeField] SO_CuttingRecipe[] _cuttingRecipes;

        public override void Interact(PlayerInteractions player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectHolder(this);

                    SetRecipeServerRpc(
                        GameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.KitchenObjectSO));
                }
            }
            else
            {
                if (!player.HasKitchenObject())
                {
                    if (CanBeCut())
                        ClearCounterServerRpc();

                    GetKitchenObject().SetKitchenObjectHolder(player);
                }
                else
                {
                    if (!AttemptTransferFromCounterToPlate(player))
                        AttemptTransferFromPlayerToPlate(player);
                }
            }
        }

        #region MULTIPLAYER_INTERACT_LOGIC

        [ServerRpc(RequireOwnership = false)]
        void SetRecipeServerRpc(int kitchenObjectSOIndex)
        {
            SetRecipeClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        void SetRecipeClientRpc(int kitchenObjectSOIndex)
        {
            if (CanBeCut())
            {
                //_currentCuttingRecipe = GetCurrentCuttingRecipe(GetKitchenObject().KitchenObjectSO);
                SO_KitchenObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
                _currentCuttingRecipe = kitchenObjectSO.GetCuttingRecipe();

                if (GetKitchenObject().CuttingTracker != 0)
                    OnProgressChanged?.Invoke(GetProgressNormalized());
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void ClearCounterServerRpc()
        {
            ClearCounterClientRpc();
        }

        [ClientRpc]
        void ClearCounterClientRpc()
        {
            OnProgressChanged?.Invoke(0);
            _currentCuttingRecipe = null;
        }

        SO_CuttingRecipe GetCurrentCuttingRecipe(SO_KitchenObject kitchenObject)
        {
            foreach (var recipe in _cuttingRecipes)
            {
                if (recipe.Input == kitchenObject)
                    return recipe;
            }
            return null;
        }

        #endregion

        public override void InteractAlt(PlayerInteractions player)
        {

            if (!HasKitchenObject() || player.HasKitchenObject() || !CanBeCut()) return;

            CutObjectServerRpc();
            TestCuttingProgressDoneServerRpc();
        }

        #region MULTIPLAYER_INTERACTALT_LOGIC

        [ServerRpc(RequireOwnership = false)]
        void CutObjectServerRpc()
        {
            if (HasKitchenObject() && CanBeCut()) 
                CutObjectClientRpc();
        }

        [ClientRpc]
        void CutObjectClientRpc()
        {
            float cuttingProgress = AddCuts();
            OnCut?.Invoke();
            OnAnyCut?.Invoke(this);
            OnProgressChanged?.Invoke(cuttingProgress);
        }

        [ServerRpc(RequireOwnership = false)]
        void TestCuttingProgressDoneServerRpc()
        {
            if (!HasKitchenObject() && !CanBeCut()) return;

            if (FinishedCutting())
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
                KitchenObject.SpawnKitchenObject(GetCuttingOutput(), this);
                _currentCuttingRecipe = null;
            }
        }

        #endregion

        bool FinishedCutting()
        {
            return GetKitchenObject().CuttingTracker >= GetCuttingProgressMax();
        }

        SO_KitchenObject GetCuttingOutput()
        {
            return _currentCuttingRecipe.Output;
        }

        bool CanBeCut()
        {
            //return GetKitchenObject().KitchenObjectSO.GetCuttingRecipe() != null;
            return GetCurrentCuttingRecipe(GetKitchenObject().KitchenObjectSO) != null;
        }

        int GetCuttingProgressMax()
        {
            return _currentCuttingRecipe.CuttingProgressMax;
        }

        float AddCuts()
        {
            GetKitchenObject().CuttingTracker++;
            return GetProgressNormalized();
        }

        float GetProgressNormalized()
        {
            return (float)GetKitchenObject().CuttingTracker / _currentCuttingRecipe.CuttingProgressMax;
        }

        new public static void ResetStaticData()
        {
            OnAnyCut = null;
        }
    }
}
