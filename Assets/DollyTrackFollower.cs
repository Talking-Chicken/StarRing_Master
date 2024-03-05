using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class DollyTrackFollower : MonoBehaviour
{

    [SerializeField] PlayerManager player; // 玩家对象
    [SerializeField] float followSpeed = 5f; // Dolly Track跟随速度\
    
    Vector3 targetPosition;
    Vector3 playerPosition;
    Vector3 initialDistance;
    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerManager>();
            playerPosition = player.transform.position;
            initialDistance= transform.position - player.transform.position;
        }
        else
        {


            playerPosition = player.transform.position;
            targetPosition = new Vector3(playerPosition.x + initialDistance.x,transform.position.y,transform.position.z);


         //   Debug.Log("initialDistance"+ playerPosition);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
        }
    }
}
