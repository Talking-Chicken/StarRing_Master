using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class NPCFace : MMSingleton<NPCFace>
{
    // Start is called before the first frame update
    PlayerManager _playerManager;
    GameObject VC_camera;
    public GameObject facingPosition;
    void Start()
    {
        _playerManager = FindObjectOfType<PlayerManager>();
        if (_playerManager == null) { Debug.Log("cannot find player manager"); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FaceEachOther ()
    {
        _playerManager.TargetNPC.transform.LookAt(_playerManager.gameObject.transform.position, transform.up);
        _playerManager.VirtualCamera.transform.LookAt(_playerManager.TargetNPC.transform.position, transform.up);
        Vector3 targetAngle = new Vector3(_playerManager.TargetNPC.transform.position.x - _playerManager.transform.position.x, 0,_playerManager.TargetNPC.transform.position.z - _playerManager.transform.position.z).normalized;
        _playerManager.PlayerFace(new Vector2(targetAngle.x,targetAngle.z));
    }
}
