using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    [CreateAssetMenu()]
    public class SO_RecipeList : ScriptableObject
    {
        [SerializeField] List<SO_FinalRecipe> _finalRecipesList;
        public List<SO_FinalRecipe> FinalRecipesList => _finalRecipesList;
    }
}
