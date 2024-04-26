using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Interactions.UI
{
    public class IconSingleUI : MonoBehaviour
    {
        [SerializeField] Image _image;

        public void SetKitchenObjectSO(SO_KitchenObject kitchenObjectSO)
        {
            _image.sprite = kitchenObjectSO.Sprite;
        }
    }
}
