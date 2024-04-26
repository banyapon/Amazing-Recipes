using UnityEngine;

namespace KitchenChaos.Interactions.Multiplayer
{
    public class FollowTransform : MonoBehaviour
    {
        Transform _targetTransform;

        public void SetTargetTransform (Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }

        void LateUpdate()
        {
            if (_targetTransform == null) return;

            transform.position = _targetTransform.position;
            transform.rotation = _targetTransform.rotation;
        }
    }
}
