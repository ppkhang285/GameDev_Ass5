using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        Setup();    
    }

    private void Setup()
    {
        Transform playerTrans = GameplayManager.Instance.player.transform;

        _virtualCamera.LookAt = playerTrans;
        _virtualCamera.Follow = playerTrans;
    }

    
}
