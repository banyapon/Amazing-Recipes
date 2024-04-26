using System;
using UnityEngine;
using Unity.Netcode;
using KitchenChaos.Multiplayer;

namespace KitchenChaos.Interactions
{
    public class StoveCounter : BaseCounter, IHasProgress
    {
        public static event Action<State, StoveCounter> OnStateChanged;
        public event Action<float> OnProgressChanged;

        public enum State
        {
            Idle,
            Frying,
            Fried,
            Burned,
        }

        SO_FryingRecipe _currentFryingRecipe;
        SO_BurningRecipe _currentBurningRecipe;
        
        NetworkVariable<State> _state = new NetworkVariable<State>(State.Idle);
        NetworkVariable<float> _fryingTimer = new NetworkVariable<float>(0f);
        NetworkVariable<float> _burningTimer = new NetworkVariable<float>(0f);

        public override void OnNetworkSpawn()
        {
            _fryingTimer.OnValueChanged += FryingTimer_OnValueChanged;
            _burningTimer.OnValueChanged += BurningTimer_OnValueChanged;
            _state.OnValueChanged += State_OnValueChanged;
        }

        void FryingTimer_OnValueChanged(float previousValue, float newValue)
        {
            float fryingTimerMax = _currentFryingRecipe != null ? _currentFryingRecipe.FryingTimerMax : 1f;

            OnProgressChanged?.Invoke(GetProgressNormalized(_fryingTimer.Value, fryingTimerMax));
        }

        void BurningTimer_OnValueChanged(float previousValue, float newValue)
        {
            float burningTimerMax = _currentBurningRecipe != null ? _currentBurningRecipe.BurningTimerMax : 1f;

            OnProgressChanged?.Invoke(GetProgressNormalized(_burningTimer.Value, burningTimerMax));
        }

        void State_OnValueChanged(State previousState, State newState)
        {
            OnStateChanged?.Invoke(_state.Value, this);

            if (_state.Value == State.Idle || _state.Value == State.Burned)
                OnProgressChanged?.Invoke(0);
        }

        void Update()
        {
            if (!IsServer) return;
            if (!HasKitchenObject()) return;

            switch (_state.Value)
            {
                case State.Idle:
                    break;
                case State.Frying:
                    ExecuteFrying();
                    break;
                case State.Fried:
                    ExecuteBurning();
                    break;
                case State.Burned:
                    break;
            }
        }

        void ExecuteBurning()
        {
            _burningTimer.Value += Time.deltaTime;

            if (_burningTimer.Value > GetBurningTimerMax())
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());

                KitchenObject.SpawnKitchenObject(_currentBurningRecipe.Output, this);
                _currentBurningRecipe = null;

                _state.Value = State.Burned;
            }
        }

        void ExecuteFrying()
        {
            _fryingTimer.Value += Time.deltaTime;
            
            if (_fryingTimer.Value > GetFryingTimerMax())
            {
                KitchenObject.DestroyKitchenObject(GetKitchenObject());

                KitchenObject.SpawnKitchenObject(_currentFryingRecipe.Output, this);
                _currentFryingRecipe = null;

                _state.Value = State.Fried;
                
                SetBurningRecipeSOClientRpc(
                    GameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().KitchenObjectSO));
                
                _burningTimer.Value = 0f;
            }
        }

        public override void Interact(PlayerInteractions player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                {
                    KitchenObject playerKitchenObject = player.GetKitchenObject();
                    if (CanBeFried(playerKitchenObject))
                    {
                        playerKitchenObject.SetKitchenObjectHolder(this);

                        PlaceObjectOnCounterServerRpc(
                            GameMultiplayer.Instance.GetKitchenObjectSOIndex(playerKitchenObject.KitchenObjectSO));                      
                    }
                }
            }
            else
            {
                if (!player.HasKitchenObject())
                {
                    GetKitchenObject().SetKitchenObjectHolder(player);
                    CleanStoveServerRpc();
                }
                else
                {
                    if (AttemptTransferFromCounterToPlate(player))
                        CleanStoveServerRpc();
                }
            }
        }

        #region MULTIPLAYER

        [ServerRpc(RequireOwnership = false)]
        void SetStateIdleServerRpc()
        {
            _state.Value = State.Idle;
        }

        [ServerRpc(RequireOwnership = false)]
        void PlaceObjectOnCounterServerRpc(int kitchenObjectSOIndex)
        {
            //moved from ClientRpc because only server can write to the NetworkVariable
            _fryingTimer.Value = 0f;
            _state.Value = State.Frying;

            SetFryingRecipeSOClientRpc(kitchenObjectSOIndex);
        }

        [ClientRpc]
        void SetFryingRecipeSOClientRpc(int kitchenObjectSOIndex)
        {
            // if error - serialize recipes here!
            SO_KitchenObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
            _currentFryingRecipe = kitchenObjectSO.GetFryingRecipe();
        }

        [ClientRpc]
        void SetBurningRecipeSOClientRpc(int kitchenObjectSOIndex)
        {
            // if error - serialize recipes here!
            SO_KitchenObject kitchenObjectSO = GameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
            _currentBurningRecipe = kitchenObjectSO.GetBurningRecipe();
        }

        [ServerRpc(RequireOwnership = false)]
        void CleanStoveServerRpc()
        {
            CleanStoveClientRpc();
        }

        [ClientRpc]
        void CleanStoveClientRpc()
        {
            CleanStove();
        }
        #endregion

        bool CanBeFried(KitchenObject kitchenObject)
        {
            return kitchenObject.KitchenObjectSO.GetFryingRecipe() != null;
        }

        public bool IsFried()
        {
            return _state.Value == State.Fried;
        }

        float GetFryingTimerMax()
        {
            return _currentFryingRecipe.FryingTimerMax;
        }

        float GetBurningTimerMax()
        {
            return _currentBurningRecipe.BurningTimerMax;
        }

        void CleanStove()
        {
            _currentFryingRecipe = null;
            _currentBurningRecipe = null;
            SetStateIdleServerRpc();
        }

        float GetProgressNormalized(float timer, float max)
        {
            return timer / max;
        }

        new public static void ResetStaticData()
        {
            OnStateChanged = null;
        }
    }
}
