using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Interactions
{
    public class ClearCounter : BaseCounter
    {
        public override void Interact(PlayerInteractions player)
        {
            if (!HasKitchenObject())
            {
                if (player.HasKitchenObject())
                    player.GetKitchenObject().SetKitchenObjectHolder(this);
            }
            else
            {
                if (!player.HasKitchenObject())
                     GetKitchenObject().SetKitchenObjectHolder(player);
                else
                {
                    if (!AttemptTransferFromCounterToPlate(player))
                        AttemptTransferFromPlayerToPlate(player);
                }
            }
        }
    }
}
