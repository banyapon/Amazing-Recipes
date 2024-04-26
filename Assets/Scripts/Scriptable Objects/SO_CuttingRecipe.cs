using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    [CreateAssetMenu()]
    public class SO_CuttingRecipe : SO_Recipe
    {        
        [SerializeField] int _cuttingProgressMax;
        public int CuttingProgressMax => _cuttingProgressMax;
    }


}
