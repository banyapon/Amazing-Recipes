using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public class SO_Recipe : ScriptableObject
    {
        [SerializeField] SO_KitchenObject _input;
        public SO_KitchenObject Input => _input;

        [SerializeField] SO_KitchenObject _output;
        public SO_KitchenObject Output => _output;
    }
}
