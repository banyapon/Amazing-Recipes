using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KitchenChaos.Interactions;
using TMPro;
using UnityEngine.UI;

namespace KitchenChaos.Core.UI
{
    public class RecipeUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _recipeName;
        [SerializeField] Transform _iconContainer;
        [SerializeField] Transform _iconTemplate;

        void Awake()
        {
            foreach (Transform child in _iconContainer)
                Destroy(child.gameObject);
        }

        //prototype only
        public void SetRecipeSO(SO_FinalRecipe recipe)
        {
            _recipeName.text = recipe.RecipeName;

            foreach (Transform child in _iconContainer)
                Destroy(child.gameObject);

            foreach (var kitchenObjectSO in recipe.KitchenObjectSOList)
            {
                Transform iconTransform = Instantiate(_iconTemplate, _iconContainer);
                iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.Sprite;
            }
        }
    }
}
