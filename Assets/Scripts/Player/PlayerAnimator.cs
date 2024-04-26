using UnityEngine;
using Unity.Netcode;

namespace KitchenChaos.Control
{
    public class PlayerAnimator : NetworkBehaviour
    {
        [SerializeField] PlayerController _player;
        
        const string IS_WALKING = "IsWalking";
        Animator _animator;

        void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (!IsOwner) return;

            _animator.SetBool(IS_WALKING, _player.IsWalking());
        }
    }
}
