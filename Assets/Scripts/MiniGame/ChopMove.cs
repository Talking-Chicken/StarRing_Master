using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGame
{
    namespace Oden
    {
        public class ChopMove : MonoBehaviour
        {
            public OdenStickMove input;
            Rigidbody rb;
            [Range(0, 100)]
            public float speed = 10;
            // Start is called before the first frame update
            void Start()
            {
                rb=GetComponent<Rigidbody>();    
            }

            // Update is called once per frame
            void Update()
            {
                //rb.AddForce(input.StickAxis * speed, ForceMode.Impulse);
                rb.velocity= input.StickAxis * speed;
            }
        }
    }
}
