using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos
{
    public class DetectBug : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            GameObject current = GameObject.Find("Player(Clone)");
            int i = 0;
            while (current != null)
            {
                i++;
                Debug.Log("Player(Clone)" + i);
                current = GameObject.Find("Player(Clone)");
                if (i == 0)
                {
                    Destroy(current);
                }
            }
        }
    }
}
