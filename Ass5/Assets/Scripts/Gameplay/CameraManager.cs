using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private float upThreshhold = 5;
    private float downThreshhold = -2;

    void Start()
    {
        Setup();    
    }

    private void Update()
    {
        MoveVerticalCamera();
    }

    private void MoveVerticalCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        CinemachineComposer transposer = _virtualCamera.GetCinemachineComponent<CinemachineComposer>();

        Vector3 newOffset = new Vector3(transposer.m_TrackedObjectOffset.x
            , transposer.m_TrackedObjectOffset.y += mouseY * 0.4f
            , transposer.m_TrackedObjectOffset.z);
        transposer.m_TrackedObjectOffset = newOffset;

    }
    private void Setup()
    {
        Transform playerTrans = GameplayManager.Instance.player.transform;

        _virtualCamera.LookAt = playerTrans;
        _virtualCamera.Follow = playerTrans;
    }

    
}
