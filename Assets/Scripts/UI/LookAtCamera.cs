using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Control
{
    public class LookAtCamera : MonoBehaviour
    {
        Transform _cameraTransform;
        enum Mode
        {
            LookAt,
            LookAtInverted,
            CameraFwd,
            CameraFwdInverted,
        }

        [SerializeField] Mode _mode;
        Vector3 _fixedInvertedPosition;

        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            
            //also in LateUpdate? Will this be used while moving?
            _fixedInvertedPosition = transform.position + (transform.position - _cameraTransform.position); 
        }

        void LateUpdate()
        {
            switch (_mode)
            {
                case Mode.LookAt:
                    transform.LookAt(_cameraTransform);
                    break;
                case Mode.LookAtInverted:
                    transform.LookAt(_fixedInvertedPosition);
                    break;
                case Mode.CameraFwd:
                    transform.forward = _cameraTransform.forward;
                    break;
                case Mode.CameraFwdInverted:
                    transform.forward = -_cameraTransform.forward;
                    break;

            }
        }

    }
}
