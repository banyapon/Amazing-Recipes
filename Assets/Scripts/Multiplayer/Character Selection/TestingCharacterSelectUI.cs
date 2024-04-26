using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Multiplayer.UI
{
    public class TestingCharacterSelectUI : MonoBehaviour
    {
        [SerializeField] Button _readyButton;
        [SerializeField] CharacterSelectReady _characterSelectReady;

        void Awake()
        {
            _readyButton.onClick.AddListener(() =>
            {
                _characterSelectReady.SetPlayerReady();
            });
        }
    }
}
