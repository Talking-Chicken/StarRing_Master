using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGame
{
    namespace Oden
    {
        public class OdenGameManager : MonoBehaviour
        {
            public enum GameState { Search, Drag, Drop }
            public GameState gameState;
            public OdenStickMove stick;

            public float lastAxisAngle;

            public List<OdenFood> foodInCup;
            // Start is called before the first frame update
            void Start()
            {
                foodInCup= new List<OdenFood>();
            }

            // Update is called once per frame
            void Update()
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                float angle = Angle(stick.StickAxis);


                //last line
                lastAxisAngle = angle;
            }
            public static float Angle(Vector2 vector2)
            {
                if (vector2.x < 0)
                {
                    return 360 - (Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg * -1);
                }
                else
                {
                    return Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
                }
            }
        }

    }
}