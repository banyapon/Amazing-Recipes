using UnityEngine;
using TMPro;
using KitchenChaos.Core;

namespace KitchenChaos.PlayerInput.UI
{
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] GameInput _gameInput;

        [Header("Keyboard Input Text")]
        [SerializeField] TextMeshProUGUI _keyMoveUpText;
        [SerializeField] TextMeshProUGUI _keyMoveDownText;
        [SerializeField] TextMeshProUGUI _keyMoveLeftText;
        [SerializeField] TextMeshProUGUI _keyMoveRightText;
        [SerializeField] TextMeshProUGUI _keyInteractText;
        [SerializeField] TextMeshProUGUI _keyInteractAltText;
        [SerializeField] TextMeshProUGUI _keyPauseText;

        [Header("Gamepad Input Text")]
        [SerializeField] TextMeshProUGUI _gamepadInteractText;
        [SerializeField] TextMeshProUGUI _gamepadInteractAltText;
        [SerializeField] TextMeshProUGUI _gamepadPauseText;

        void Start()
        {
            _gameInput.OnBindingRebind += GameInput_OnBindingRebind;
            GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
            UpdateVisual();
            
        }

        void GameManager_OnLocalPlayerReadyChanged()
        {
            // CONSIDER implementing the ability to "unready" the player
            // otherwise remove the bool check
            if (GameManager.Instance.IsLocalPlayerReady)
                gameObject.SetActive(false);
        }

        void GameInput_OnBindingRebind()
        {
            UpdateVisual();
        }

        void UpdateVisual()
        {
            UpdateKeyboardInputText();
            UpdateGamepadInputText();
        }

        void UpdateKeyboardInputText()
        {
            _keyMoveUpText.text = _gameInput.GetBindingText(Binding.Move_Up);
            _keyMoveDownText.text = _gameInput.GetBindingText(Binding.Move_Down);
            _keyMoveLeftText.text = _gameInput.GetBindingText(Binding.Move_Left);
            _keyMoveRightText.text = _gameInput.GetBindingText(Binding.Move_Right);
            _keyInteractText.text = _gameInput.GetBindingText(Binding.Interact);
            _keyInteractAltText.text = _gameInput.GetBindingText(Binding.InteractAlt);
            _keyPauseText.text = _gameInput.GetBindingText(Binding.Pause);
        }

        void UpdateGamepadInputText()
        {
            _gamepadInteractText.text = _gameInput.GetBindingText(Binding.Gamepad_Interact);
            _gamepadInteractAltText.text = _gameInput.GetBindingText(Binding.Gamepad_InteractAlt);
            _gamepadPauseText.text = _gameInput.GetBindingText(Binding.Gamepad_Pause);
        }

        void OnDestroy()
        {
            _gameInput.OnBindingRebind -= GameInput_OnBindingRebind;
            GameManager.Instance.OnStateChanged -= GameManager_OnLocalPlayerReadyChanged;
        }
    }
}
