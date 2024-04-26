using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions.UI
{
    public class PlateIconsUI : MonoBehaviour
    {
        [SerializeField] PlateKitchenObject _plateKitchenObject;
        [SerializeField] Transform _iconTemplate;

        void Start()
        {
            _plateKitchenObject.OnIngredientAdded += _plateKitchenObject_OnIngredientAdded;

            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        void _plateKitchenObject_OnIngredientAdded(SO_KitchenObject kitchenObject)
        {
            UpdateVisual(kitchenObject);
        }

        //prototyping. implement pooling
        void UpdateVisual(SO_KitchenObject kitchenObject)
        {
            Transform iconTransform = Instantiate(_iconTemplate, transform);
            iconTransform.GetComponent<IconSingleUI>().SetKitchenObjectSO(kitchenObject);
        }

        void OnDestroy()
        {
            _plateKitchenObject.OnIngredientAdded -= _plateKitchenObject_OnIngredientAdded;
        }
    }
}
