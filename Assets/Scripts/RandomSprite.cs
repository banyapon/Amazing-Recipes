using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos
{
    public class RandomSprite : MonoBehaviour
    {
        Image img;
        public int number = 1;
        void Start()
        {
            number = Random.Range(1, 10);
            img = this.gameObject.GetComponent<Image>();
            Sprite mySprite = Resources.Load<Sprite>("customer"+number);
            img.sprite = mySprite;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
