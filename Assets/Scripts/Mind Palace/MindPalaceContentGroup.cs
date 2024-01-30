using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindPalaceContentGroup : MonoBehaviour
{
    public bool Shorten { get; private set; }
    public Transform[] nonShortenContent;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetShorten(bool toShorten)
    {
        Shorten = toShorten;
        foreach (Transform t in nonShortenContent)
        {
            if (t != null)
            {
                t.gameObject.SetActive(!toShorten);
            }
        }
    }
}
