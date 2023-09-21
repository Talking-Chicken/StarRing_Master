using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiniGame
{
    namespace Oden
    {
        public class RotateButton : MonoBehaviour
        {
            public ChopMove chopMove;
            public float angle;
            // Start is called before the first frame update
            void Start()
            {

            }

            // Update is called once per frame
            void Update()
            {

            }
            private void OnMouseOver()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    chopMove.Rotate(angle);
                }
            }
        }

    }
}