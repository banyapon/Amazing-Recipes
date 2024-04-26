using Unity.Netcode.Components;
using UnityEngine;

namespace KitchenChaos.Multiplayer
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
