using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{ 
    [CreateAssetMenu()]
    public class SO_KitchenObject : ScriptableObject
    {
        [SerializeField] Transform _prefab;
        public Transform KitchenObjectPrefab => _prefab;

        [SerializeField] Sprite _sprite;
        public Sprite Sprite => _sprite;

        [SerializeField] string _objectName;

        [SerializeField] SO_Recipe[] _recipes;

        public SO_CuttingRecipe GetCuttingRecipe()
        {
            foreach (var recipe in _recipes)
            {
                if (recipe is SO_CuttingRecipe)
                    return (SO_CuttingRecipe)recipe;
            }
            return null;
        }

        public SO_FryingRecipe GetFryingRecipe()
        {
            foreach (var recipe in _recipes)
            {
                if (recipe is SO_FryingRecipe)
                    return (SO_FryingRecipe)recipe;
            }
            return null;
        }

        public SO_BurningRecipe GetBurningRecipe()
        {
            foreach (var recipe in _recipes)
            {
                if (recipe is SO_BurningRecipe)
                    return (SO_BurningRecipe)recipe;
            }
            return null;
        }
    }
}
