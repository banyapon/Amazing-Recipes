using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Core.UI
{
    public class DeliveryManagerUI : MonoBehaviour
    {
        [SerializeField] Transform _container;
        [SerializeField] Transform _recipeTemplate;

        void Awake()
        {
            foreach (Transform child in _container)
                Destroy(child.gameObject);
        }

        void Start()
        {
            DeliveryManager.Instance.OnNewRecipeGenerated += DeliveryManager_OnNewRecipeGenerated;
            DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;
        }

        void DeliveryManager_OnRecipeDelivered()
        {
            UpdateVisual();
        }

        void DeliveryManager_OnNewRecipeGenerated()
        {
            UpdateVisual();
        }

        //event should take in a recipe parameter, store in list here, only remove and instantiate changes!
        void UpdateVisual()
        {
            foreach (Transform child in _container)
                Destroy(child.gameObject);

            foreach (var recipe in DeliveryManager.Instance.PendingRecipesList)
            {
                Transform recipeTransform = Instantiate(_recipeTemplate, _container);
                recipeTransform.GetComponent<RecipeUI>().SetRecipeSO(recipe);
            }
        }

        void OnDestroy()
        {
            DeliveryManager.Instance.OnNewRecipeGenerated -= DeliveryManager_OnNewRecipeGenerated;
            DeliveryManager.Instance.OnRecipeDelivered -= DeliveryManager_OnRecipeDelivered;
        }
    }
}
