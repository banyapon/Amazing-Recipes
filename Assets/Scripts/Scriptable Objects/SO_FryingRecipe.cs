using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    [CreateAssetMenu()]
    public class SO_FryingRecipe : SO_Recipe
    {        
        [SerializeField] float _fryingTimerMax;
        public float FryingTimerMax => _fryingTimerMax;
    }


}
