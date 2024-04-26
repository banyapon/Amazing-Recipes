using UnityEngine;

namespace KitchenChaos.Interactions.Visual
{
    public class ContainerCounterVisual : MonoBehaviour
    {
        [SerializeField] ContainerCounter _containerCounter;

        Animator _animator;
        const string OPEN_CLOSE = "OpenClose";

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Start()
        {
            _containerCounter.OnPlayerGrabbedObject += _containerCounter_OnPlayerGrabbedObject;
        }

        void _containerCounter_OnPlayerGrabbedObject()
        {
            _animator.SetTrigger(OPEN_CLOSE);
        }

        void OnDestroy()
        {
            _containerCounter.OnPlayerGrabbedObject -= _containerCounter_OnPlayerGrabbedObject;
        }
    }
}
