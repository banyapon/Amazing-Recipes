using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenChaos.PlayerInput
{
    public partial class GameInput : MonoBehaviour
    {
        public event Action OnInteractAction;
        public event Action OnInteractAltAction;
        public event Action OnPauseAction;
        public event Action OnBindingRebind;

        PlayerInputActions _playerInputActions;
        const string PLAYER_PREFS_BINDINGS = "InputBindings";

        void Awake()
        {
            _playerInputActions = new PlayerInputActions();

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
                _playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));

            _playerInputActions.Player.Enable();

            _playerInputActions.Player.Interact.performed += Interact_performed;
            _playerInputActions.Player.InteractAlt.performed += InteractAlt_performed;
            _playerInputActions.Player.Pause.performed += Pause_performed;
        }

        private void Pause_performed(InputAction.CallbackContext obj)
        {
            OnPauseAction?.Invoke();
        }

        private void InteractAlt_performed(InputAction.CallbackContext obj)
        {
            OnInteractAltAction?.Invoke();
        }

        void Interact_performed(InputAction.CallbackContext obj)
        {
            OnInteractAction?.Invoke();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            Vector2 inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();

            inputVector = inputVector.normalized;

            return inputVector;
        }

        //check how Samyam does this :-/
        public string GetBindingText(Binding binding)
        {
            switch (binding)
            {
                default:
                case Binding.Move_Up:
                    return _playerInputActions.Player.Move.bindings[1].ToDisplayString();
                case Binding.Move_Down:
                    return _playerInputActions.Player.Move.bindings[2].ToDisplayString();
                case Binding.Move_Left:
                    return _playerInputActions.Player.Move.bindings[3].ToDisplayString();
                case Binding.Move_Right:
                    return _playerInputActions.Player.Move.bindings[4].ToDisplayString();
                case Binding.Interact:
                    return _playerInputActions.Player.Interact.bindings[0].ToDisplayString();
                case Binding.InteractAlt:
                    return _playerInputActions.Player.InteractAlt.bindings[0].ToDisplayString();
                case Binding.Pause:
                    return _playerInputActions.Player.Pause.bindings[0].ToDisplayString();
                case Binding.Gamepad_Interact:
                    return _playerInputActions.Player.Interact.bindings[1].ToDisplayString();
                case Binding.Gamepad_InteractAlt:
                    return _playerInputActions.Player.InteractAlt.bindings[1].ToDisplayString();
                case Binding.Gamepad_Pause:
                    return _playerInputActions.Player.Pause.bindings[1].ToDisplayString();
            }
        }

        public void RebindBinding(Binding binding, Action onActionRebound)
        {
            _playerInputActions.Player.Disable();

            InputAction inputAction;
            int bindingIndex;

            switch (binding)
            {
                default:
                case Binding.Move_Up:
                    inputAction = _playerInputActions.Player.Move;
                    bindingIndex = 1;
                    break;
                case Binding.Move_Down:
                    inputAction = _playerInputActions.Player.Move;
                    bindingIndex = 2;
                    break;
                case Binding.Move_Left:
                    inputAction = _playerInputActions.Player.Move;
                    bindingIndex = 3;
                    break;
                case Binding.Move_Right:
                    inputAction = _playerInputActions.Player.Move;
                    bindingIndex = 4;
                    break;
                case Binding.Interact:
                    inputAction = _playerInputActions.Player.Interact;
                    bindingIndex = 0;
                    break;
                case Binding.InteractAlt:
                    inputAction = _playerInputActions.Player.InteractAlt;
                    bindingIndex = 0;
                    break;
                case Binding.Pause:
                    inputAction = _playerInputActions.Player.Pause;
                    bindingIndex = 0;
                    break;
                case Binding.Gamepad_Interact:
                    inputAction = _playerInputActions.Player.Interact;
                    bindingIndex = 1;
                    break;
                case Binding.Gamepad_InteractAlt:
                    inputAction = _playerInputActions.Player.InteractAlt;
                    bindingIndex = 1;
                    break;
                case Binding.Gamepad_Pause:
                    inputAction = _playerInputActions.Player.Pause;
                    bindingIndex = 1;
                    break;
            }

            inputAction.PerformInteractiveRebinding(bindingIndex)
                .OnComplete(callback =>
                {
                    callback.Dispose();
                    _playerInputActions.Player.Enable();
                    onActionRebound();

                    SaveRebind();
                    OnBindingRebind?.Invoke();
                })
                .Start();
        }

        void SaveRebind()
        {
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, _playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }

        void OnDisable()
        {
            _playerInputActions.Player.Interact.performed -= Interact_performed;
            _playerInputActions.Player.InteractAlt.performed -= InteractAlt_performed;
            _playerInputActions.Player.Pause.performed -= Pause_performed;

            _playerInputActions.Dispose();
        }
    }
}
