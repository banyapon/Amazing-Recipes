using UnityEngine;

namespace KitchenChaos.Interactions.Visual
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        [SerializeField] CuttingCounter _cuttingCounter;

        Animator _animator;
        const string CUT = "Cut";

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            _cuttingCounter.OnCut += _cuttingCounter_OnCut;
        }

        void _cuttingCounter_OnCut()
        {
            _animator.SetTrigger(CUT);
        }

        void OnDestroy()
        {
            _cuttingCounter.OnCut -= _cuttingCounter_OnCut;
        }
    }
}
