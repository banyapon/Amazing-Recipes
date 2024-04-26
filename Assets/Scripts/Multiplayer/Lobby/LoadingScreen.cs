using UnityEngine;

namespace KitchenChaos.Multiplayer
{
    public class LoadingScreen : MonoBehaviour
    {
        void Awake()
        {
            if (GameMultiplayer.PlayMultiplayer)
                Hide();
            else
                Show();
        }

        void Show()
        {
            gameObject.SetActive(true);
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
