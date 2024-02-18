using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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
            public float rotateTime = .5f;
            // Start is called before the first frame update
            void Start()
            {
                rb = GetComponent<Rigidbody>();
            }

            // Update is called once per frame
            void Update()
            {
                //rb.AddForce(input.StickAxis * speed, ForceMode.Impulse);
                rb.velocity = input.StickAxis * speed;
            }
            public void Rotate(float angle)
            {
                StartCoroutine(RotateTo(angle));
            }
            private IEnumerator RotateTo(float angle)
            {
                for (float timer = 0; timer < rotateTime; timer += Time.deltaTime)
                {
                    transform.Rotate(0, 0, angle / rotateTime * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}
