using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Multiplayer.UI
{
    public class CharacterColorSelectSingleUI : MonoBehaviour
    {
        [SerializeField] int _colorIndex;
        [SerializeField] Image _image;
        [SerializeField] GameObject _selectedGameObject;

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                GameMultiplayer.Instance.ChangePlayerColor(_colorIndex);
            });
        }

        void Start()
        {
            GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameMultiplayer_OnPlayerDataNetworkListChanged;
            _image.color = GameMultiplayer.Instance.GetPlayerColor(_colorIndex);

            UpdateIsSelected();
        }

        void GameMultiplayer_OnPlayerDataNetworkListChanged()
        {
            UpdateIsSelected(); 
        }

        void UpdateIsSelected()
        {
            if (GameMultiplayer.Instance.GetPlayerData().ColorId == _colorIndex)
                _selectedGameObject.SetActive(true);
            else
                _selectedGameObject.SetActive(false);
        }

        void OnDestroy()
        {
            GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayer_OnPlayerDataNetworkListChanged;
        }
    }
}
