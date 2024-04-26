using UnityEngine;
using KitchenChaos.Control;
using KitchenChaos.Interactions;
using KitchenChaos.Core.UI;

namespace KitchenChaos.DataReset
{
    public class ResetStaticDataManager : MonoBehaviour
    {
        private void Awake()
        {
            PlayerController.ResetStaticData();
            PlayerInteractions.ResetStaticData();
            BaseCounter.ResetStaticData();
            CuttingCounter.ResetStaticData();
            StoveCounter.ResetStaticData();
            TrashCounter.ResetStaticData();
            StartCountdownUI.ResetStaticData();
        }
    }
}
