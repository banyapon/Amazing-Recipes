using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KitchenChaos.PlayerInput;
using System;

namespace KitchenChaos.Core.UI
{
    public class ControlsMenuUI : MonoBehaviour
    {
        [SerializeField] GameInput _gameInput;
        [SerializeField] GameObject _rebindScreen;
        
        [Header("Keyboard Buttons")]
        [SerializeField] Button _moveUpButton;
        [SerializeField] Button _moveDownButton;
        [SerializeField] Button _moveLeftButton;
        [SerializeField] Button _moveRightButton;
        [SerializeField] Button _interactButton;
        [SerializeField] Button _interactAltButton;
        [SerializeField] Button _pauseButton;
        
        [Header("Keyboard Button Texts")]
        [SerializeField] TextMeshProUGUI _moveUpText;
        [SerializeField] TextMeshProUGUI _moveDownText;
        [SerializeField] TextMeshProUGUI _moveLeftText;
        [SerializeField] TextMeshProUGUI _moveRightText;
        [SerializeField] TextMeshProUGUI _interactText;
        [SerializeField] TextMeshProUGUI _interactAltText;
        [SerializeField] TextMeshProUGUI _pauseText;

        [Header("Gamepad Buttons")]
        [SerializeField] Button _gamepadInteractButton;
        [SerializeField] Button _gamepadInteractAltButton;
        [SerializeField] Button _gamepadPauseButton;

        [Header("Gamepad Button Texts")]
        [SerializeField] TextMeshProUGUI _gamepadInteractText;
        [SerializeField] TextMeshProUGUI _gamepadInteractAltText;
        [SerializeField] TextMeshProUGUI _gamepadPauseText;


        void OnEnable()
        {
            HideRebindScreen();
        }

        void Awake()
        {
            SetupAllButtonsListeners();
        }

        void Start()
        {
            UpdateAllButtonsText();
            gameObject.SetActive(false);
        }

        void SetupAllButtonsListeners()
        {
            SetupKeyboardButtonListeners();
            SetupGamepadButtonListeners();
        }

        void SetupKeyboardButtonListeners()
        {
            _moveUpButton.onClick.AddListener(() => { RebindBinding(Binding.Move_Up); });
            _moveDownButton.onClick.AddListener(() => { RebindBinding(Binding.Move_Down); });
            _moveLeftButton.onClick.AddListener(() => { RebindBinding(Binding.Move_Left); });
            _moveRightButton.onClick.AddListener(() => { RebindBinding(Binding.Move_Right); });
            _interactButton.onClick.AddListener(() => { RebindBinding(Binding.Interact); });
            _interactAltButton.onClick.AddListener(() => { RebindBinding(Binding.InteractAlt); });
            _pauseButton.onClick.AddListener(() => { RebindBinding(Binding.Pause); });
        }

        void SetupGamepadButtonListeners()
        {
            _gamepadInteractButton.onClick.AddListener(() => { RebindBinding(Binding.Gamepad_Interact); });
            _gamepadInteractAltButton.onClick.AddListener(() => { RebindBinding(Binding.Gamepad_InteractAlt); });
            _gamepadPauseButton.onClick.AddListener(() => { RebindBinding(Binding.Gamepad_Pause); });
        }

        void RebindBinding(Binding binding)
        {
            _rebindScreen.SetActive(true);
            _gameInput.RebindBinding(binding, () =>
            {
                HideRebindScreen();
                UpdateAllButtonsText();
            });
        }

        void HideRebindScreen()
        {
            _rebindScreen.SetActive(false);
        }

        void UpdateAllButtonsText()
        {
            UpdateKeyboardButtonText();
            UpdateGamepadButtonText();
        }

        void UpdateGamepadButtonText()
        {
            _gamepadInteractText.text = _gameInput.GetBindingText(Binding.Gamepad_Interact);
            _gamepadInteractAltText.text = _gameInput.GetBindingText(Binding.Gamepad_InteractAlt);
            _gamepadPauseText.text = _gameInput.GetBindingText(Binding.Gamepad_Pause);
        }
        
        void UpdateKeyboardButtonText()
        {
            _moveUpText.text = _gameInput.GetBindingText(Binding.Move_Up);
            _moveDownText.text = _gameInput.GetBindingText(Binding.Move_Down);
            _moveLeftText.text = _gameInput.GetBindingText(Binding.Move_Left);
            _moveRightText.text = _gameInput.GetBindingText(Binding.Move_Right);
            _interactText.text = _gameInput.GetBindingText(Binding.Interact);
            _interactAltText.text = _gameInput.GetBindingText(Binding.InteractAlt);
            _pauseText.text = _gameInput.GetBindingText(Binding.Pause);
        }
    }
}
