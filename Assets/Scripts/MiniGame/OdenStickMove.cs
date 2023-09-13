using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace MiniGame
{
    namespace Oden
    {
        public class OdenStickMove : MonoBehaviour
        {
            bool isPressed;
            public float rotateSpeed = 10;
            public float range = 3.5f;

            public Transform head;

            private float radius;
            private Vector3 originDirection;


            public Vector2 StickAxis
            {
                get; private set;
            }
            // Start is called before the first frame update
            void Start()
            {
                originDirection = head.transform.position - transform.position;
                radius = originDirection.magnitude;
                originDirection = originDirection.normalized;
            }

            // Update is called once per frame
            void Update()
            {
                if (!Input.GetMouseButton(0))
                {
                    Quaternion _lookRotation = Quaternion.LookRotation(originDirection);

                    //rotate us over time according to speed until we are in the required rotation
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotateSpeed);
                }
                Vector3 axis = head.position - transform.position;
                StickAxis = new Vector2(axis.x, axis.y).normalized;
            }

            private void OnMouseDrag()
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z - 2));

                //find the vector pointing from our position to the target

                Vector3 _direction = (pos - transform.position);
                Vector2 surface = new(_direction.x, _direction.y);
                if (surface.magnitude > range)
                {
                    surface = Vector2.ClampMagnitude(surface, range);
                }
                _direction.x = surface.x;
                _direction.y = surface.y;
                //Debug.Log(_direction);
                _direction.z = Mathf.Sign(_direction.z) * Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(surface.magnitude, 2));
                //range square - distance(x,y) square = z square

                //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(_direction.normalized);

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotateSpeed);
            }
        }
    }
}

