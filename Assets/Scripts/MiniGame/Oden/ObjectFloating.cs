using Cinemachine.Utility;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MiniGame
{
    namespace Oden
    {
        public class ObjectFloating : MonoBehaviour
        {
            //public Transform waterSurface;
            public OdenWater water;

            [MinMaxSlider(0f, 10f)]
            public Vector2 depthRange;
            [Range(1, 50f)]
            public float floatForce = 5f;
            const float maxCompareDepth = 5f;
            
            private float depth;

            private Rigidbody rb;
            // Start is called before the first frame update
            void Start()
            {
                depth = Random.Range(depthRange[0], depthRange[1]);
                rb = GetComponent<Rigidbody>();
            }


            private void Update()
            {
                
            }
            // Update is called once per frame
            private void FixedUpdate()
            {
                if (water != null)
                {
                    float distance = water.waterSurface.transform.position.y - transform.position.y - depth;
                    float rate = Mathf.Clamp(distance / maxCompareDepth, -1, 1);
                    rb.AddForce(rate * floatForce * Vector2.up);
                }
            }
            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent<OdenWater>(out var water))
                {
                    rb.useGravity = false;
                    if (rb.velocity.y < 0)
                    {
                        rb.AddForce(-1 * water.viscosity * rb.velocity);
                    }
                    this.water=water;
                }
            }
            private void OnTriggerExit(Collider other)
            {
                if(other.TryGetComponent<OdenWater>(out var water) && water == this.water)
                {
                    rb.useGravity = true;
                    this.water = null;
                }
            }
        }
    }
}