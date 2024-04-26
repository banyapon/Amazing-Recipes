using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;

namespace KitchenChaos.Multiplayer
{
    public class CharacterSelectPlayer : MonoBehaviour
    {
        [SerializeField] int _playerIndex;
        [SerializeField] GameObject _readyGameObject;
        [SerializeField] PlayerVisual _playerVisual;
        [SerializeField] Button _kickButton;
        [SerializeField] TextMeshPro _playerNameText;
        
        CharacterSelectReady _characterSelectReady;

        void Awake()
        {
            _characterSelectReady = FindObjectOfType<CharacterSelectReady>();

            _kickButton.onClick.AddListener(() =>
            {
                PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
                KitchenGameLobby.Instance.KickPlayer(playerData.PlayerId.ToString());
                GameMultiplayer.Instance.KickPlayer(playerData.ClientId);
            });
        }

        void Start()
        {
            GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameMultiplayer_OnPlayerDataNetworkListChanged;
            _characterSelectReady.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

            _kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

            UpdatePlayer();
        }

        void CharacterSelectReady_OnReadyChanged()
        {
            UpdatePlayer();
        }

        void GameMultiplayer_OnPlayerDataNetworkListChanged()
        {
            UpdatePlayer();
        }

        void UpdatePlayer()
        {
            if (GameMultiplayer.Instance.IsPlayerIndexConnected(_playerIndex))
            {
                Show();

                PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
                _readyGameObject.SetActive(_characterSelectReady.IsPlayerReady(playerData.ClientId));

                _playerNameText.text = playerData.PlayerName.ToString();

                _playerVisual.SetPlayerColor(GameMultiplayer.Instance.GetPlayerColor(playerData.ColorId));
            }
            else
            {
                Hide();
            }
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayer_OnPlayerDataNetworkListChanged;
        }
    }
}
