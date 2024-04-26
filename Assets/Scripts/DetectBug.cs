using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace KitchenChaos
{
    public class DetectBug : NetworkBehaviour
    {
        public GameObject current;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (!IsServer)
            {
                current = GameObject.Find("Player-0");
                if (current != null)
                {
                    //Destroy(current);
                    StartCoroutine(ChangePositionStone());
                }
                else
                {
                    return;
                }
            }

        }
        IEnumerator ChangePositionStone()
        {
            yield return new WaitForSeconds(0.5f);
            current.transform.position = new Vector3(0f, 0f, 12f);
        }

    }
}
