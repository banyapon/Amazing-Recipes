using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    /// <summary>
    /// For the purposes of multiplayer: SOs can't be serialized,
    /// so we need to pass in an integer (a list index) instead.
    /// </summary>
    [CreateAssetMenu()]
    public class SO_KitchenObjectList : ScriptableObject
    {
        [SerializeField] List<SO_KitchenObject> _kitchenObjectList;
        public List<SO_KitchenObject> KitchenObjectList => _kitchenObjectList;
    }
}
