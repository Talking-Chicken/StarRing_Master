using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiniGame
{
    namespace Oden
    {
        public class OdenFishMove : MonoBehaviour
        {
            ObjectFloating floating;
            Rigidbody rb;
            public float moveSpeed;
            // Start is called before the first frame update
            void Start()
            {
                floating = GetComponent<ObjectFloating>();
                rb = GetComponent<Rigidbody>();
            }

            // Update is called once per frame
            void Update()
            {
                //move in water
                if (floating.water != null)
                {
                    rb.AddForce(moveSpeed * Time.deltaTime * transform.right, ForceMode.Acceleration);
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
                }
                else
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
                }

                //add torque to make z rotation lerp to zero or 180


            }
        }

    }
}
