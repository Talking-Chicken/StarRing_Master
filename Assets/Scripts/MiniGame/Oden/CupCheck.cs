using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    namespace Oden
    {
        public class CupCheck : MonoBehaviour
        {
            public OdenGameManager gameManager;
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }
            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent<OdenFood>(out var food))
                {
                    gameManager.foodInCup.Add(food);
                }
            }
            private void OnTriggerExit(Collider other)
            {
                if (other.TryGetComponent<OdenFood>(out var food))
                {
                    gameManager.foodInCup.Remove(food);
                }
            }
        }
    }
}