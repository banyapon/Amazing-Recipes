using Unity.Netcode.Components;

namespace KitchenChaos.Multiplayer
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
