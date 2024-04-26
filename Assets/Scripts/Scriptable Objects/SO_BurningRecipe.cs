using UnityEngine;

namespace KitchenChaos.Interactions
{
    [CreateAssetMenu()]
    public class SO_BurningRecipe : SO_Recipe
    {
        [SerializeField] float _burningTimerMax;
        public float BurningTimerMax => _burningTimerMax;
    }
}

