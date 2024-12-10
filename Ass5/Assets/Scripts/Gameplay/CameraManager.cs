using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private PhotonView photonView;

    private float upThreshhold = 5;
    private float downThreshhold = -2;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (GameManager.Instance.isPvP)
            Setup(NetworkGameplayManager.Instance.players[PhotonNetwork.LocalPlayer.ActorNumber - 1].transform);
        else
            Setup(GameplayManager.Instance.player.transform);    
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

    public void Setup(Transform player)
    {
        if (GameManager.Instance.isPvP && !photonView.IsMine) return;
        Transform playerTrans = player;

        _virtualCamera.LookAt = playerTrans;
        _virtualCamera.Follow = playerTrans;
    }

    
}
