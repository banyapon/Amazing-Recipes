using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Multiplayer
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] MeshRenderer _headMeshRenderer;
        [SerializeField] MeshRenderer _bodyMeshRenderer;

        Material _material;

        void Awake()
        {
            //cloning the material, otherwise all players will share one material
            _material = new Material(_headMeshRenderer.material);
            _headMeshRenderer.material = _material;
            _bodyMeshRenderer.material = _material;
        }

        public void SetPlayerColor(Color color)
        {
            _material.color = color;
        }
    }
}
