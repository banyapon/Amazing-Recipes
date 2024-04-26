using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public interface IHasProgress 
    {
        public event Action<float> OnProgressChanged;
    }
}
