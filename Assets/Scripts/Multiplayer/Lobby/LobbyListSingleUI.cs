using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace KitchenChaos.Multiplayer
{
    public class LobbyListSingleUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _lobbyNameText;

        Lobby _lobby;

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                //public lobbies cannot be joined by code!
                KitchenGameLobby.Instance.JoinWithId(_lobby.Id);
            });      
        }

        public void SetLobby(Lobby lobby)
        {
            _lobby = lobby;
            _lobbyNameText.text = lobby.Name;
        }


    }
}
