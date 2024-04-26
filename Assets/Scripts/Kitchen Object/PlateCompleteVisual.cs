using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions.Visual
{
    public class PlateCompleteVisual : MonoBehaviour
    {
        [SerializeField] PlateKitchenObject _plateKitchenObject;
        [SerializeField] List<KitchenObjectSO_GameObject> _kitchenObjectGameObjectList;

        [System.Serializable]
        struct KitchenObjectSO_GameObject
        {
            public SO_KitchenObject KitchenObjectSO;
            public GameObject GameObject;
        }

        void Start()
        {
            _plateKitchenObject.OnIngredientAdded += _plateKitchenObject_OnIngredientAdded;

            foreach (var kitchenObjGameObj in _kitchenObjectGameObjectList)
                kitchenObjGameObj.GameObject.SetActive(false);
        }

        void _plateKitchenObject_OnIngredientAdded(SO_KitchenObject kitchenObject)
        {
            foreach (var kitchenObjGameObj in _kitchenObjectGameObjectList)
            {
                if (kitchenObjGameObj.KitchenObjectSO == kitchenObject)
                    kitchenObjGameObj.GameObject.SetActive(true);
            }
        }

        void OnDestroy()
        {
            _plateKitchenObject.OnIngredientAdded -= _plateKitchenObject_OnIngredientAdded;
        }
    }
}
