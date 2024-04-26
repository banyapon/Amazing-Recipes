using UnityEngine;
using Unity.Netcode;
using KitchenChaos.Interactions.Multiplayer;
using KitchenChaos.Multiplayer;

namespace KitchenChaos.Interactions
{
    public class KitchenObject : NetworkBehaviour
    {
        [SerializeField] SO_KitchenObject _kitchenObjectSO;
        public SO_KitchenObject KitchenObjectSO => _kitchenObjectSO;

        IKitchenObjectHolder _kitchenObjectHolder;
        public IKitchenObjectHolder KitchenObjectHolder => _kitchenObjectHolder;

        int _cuttingTracker = 0;
        public int CuttingTracker
        {
            get { return _cuttingTracker; }
            set { _cuttingTracker = value; }
        }

        FollowTransform _followTransform;

        void Awake()
        {
            _followTransform = GetComponent<FollowTransform>();
        }

        public void SetKitchenObjectHolder(IKitchenObjectHolder kitchenObjectHolder)
        {
            SetKitchenObjectHolderServerRpc(kitchenObjectHolder.GetNetworkObject());
        }

        //this will only run on the server
        [ServerRpc(RequireOwnership = false)]
        void SetKitchenObjectHolderServerRpc(NetworkObjectReference kitchenObjectHolder_NetworkObjRef)
        {
            SetKitchenObjectHolderClientRpc(kitchenObjectHolder_NetworkObjRef);
        }

        [ClientRpc]
        void SetKitchenObjectHolderClientRpc(NetworkObjectReference kitchenObjectHolder_NetworkObjRef)
        {
            kitchenObjectHolder_NetworkObjRef.TryGet(out NetworkObject kitchenObjectHolder_NetworkObject);
            IKitchenObjectHolder kitchenObjectHolder = kitchenObjectHolder_NetworkObject.GetComponent<IKitchenObjectHolder>();

            if (_kitchenObjectHolder != null)
                _kitchenObjectHolder.ClearKitchenObject();

            _kitchenObjectHolder = kitchenObjectHolder;

            if (_kitchenObjectHolder.HasKitchenObject())
                Debug.LogError("IKitchenObjectHolder already has a KitchenObject!");

            _kitchenObjectHolder.SetKitchenObject(this);

            _followTransform.SetTargetTransform(_kitchenObjectHolder.GetKitchenObjectSpawnPoint());
        }

        public bool TryGetPlate(out PlateKitchenObject plate)
        {
            if (this is PlateKitchenObject)
            {
                plate = this as PlateKitchenObject;
                return true;
            }
            else
            {
                plate = null;
                return false;
            }
        }

        //for prototyping only - implement pooling
        public void DestroySelf()
        {
            Destroy(gameObject); 
        }
        
        //refactored DestroySelf() to run this logic on client
        public void ClearKitchenObjectHolder()
        {
            _kitchenObjectHolder.ClearKitchenObject();
        }

        public static void SpawnKitchenObject(SO_KitchenObject kitchenObjectSO, IKitchenObjectHolder kitchenObjectHolder)
        {
            GameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectHolder);
        }

        public static void DestroyKitchenObject(KitchenObject kitchenObject)
        {
            GameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
        }

    }
}
