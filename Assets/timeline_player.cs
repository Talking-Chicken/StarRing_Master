using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class timeline_player : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayableDirector timeline;
    public TimelineAsset timeline_asset;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            timeline_asset.GetOutputTrack(0).muted = false;
            Debug.Log("subgroup" + timeline_asset.GetRootTrack(0).name);
            timeline.Play();
            
        }
    }
}
