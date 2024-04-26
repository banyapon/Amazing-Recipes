using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KitchenChaos.Interactions.Multiplayer;
using KitchenChaos.Core;

namespace KitchenChaos.Multiplayer
{
    public class TestingLobbyUI : MonoBehaviour
    {
        [SerializeField] Button _createGameButton;
        [SerializeField] Button _joinGameButton;

        private void Awake()
        {
            _createGameButton.onClick.AddListener(() =>
           {
               GameMultiplayer.Instance.StartHost();
               Loader.LoadSceneNetwork(Loader.Scene.CharacterSelectScene);
           });

            _joinGameButton.onClick.AddListener(() =>
            {
                GameMultiplayer.Instance.StartClient();
            });
        }
    }
}
