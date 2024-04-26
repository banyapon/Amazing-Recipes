using KitchenChaos.Multiplayer;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.DataReset
{
    public class MainMenuCleanup : MonoBehaviour
    {
        void Awake()
        {
            if (NetworkManager.Singleton != null)
                Destroy(NetworkManager.Singleton.gameObject);

            if (GameMultiplayer.Instance != null)
                Destroy(GameMultiplayer.Instance.gameObject);

            //NetworkManager[] networkManager_GameObjectArray = FindObjectsOfType<NetworkManager>();
            //if (networkManager_GameObjectArray.Length > 1)
            //    Destroy(networkManager_GameObjectArray[1]);

            //GameMultiplayer[] gameMultiplayer_GameObjectArray = FindObjectsOfType<GameMultiplayer>();
            //if (gameMultiplayer_GameObjectArray.Length > 1)
            //    Destroy(gameMultiplayer_GameObjectArray[1]);

            if (KitchenGameLobby.Instance != null)
                Destroy(KitchenGameLobby.Instance.gameObject);
        }

    }
}
