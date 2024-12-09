using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class RoomController : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    string lobbyScene = "LobbyScene";
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            Debug.Log("Not in the room, returning back to Lobby");
            UnityEngine.SceneManagement.SceneManager.LoadScene(lobbyScene);
            return;
        }
        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        StartCoroutine(DelayedPlayerInstantiation());
    }
    private IEnumerator DelayedPlayerInstantiation()
    {
        yield return new WaitForSeconds(2.0f);
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length - 1)].position, spawnPoints[Random.Range(0, spawnPoints.Length - 1)].rotation, 0);
    }
    void OnGUI()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        // Button: Leave Room
        if (GUI.Button(new Rect(125, 50, 300, 80), "Leave room"))
        {
            PhotonNetwork.LeaveRoom();
        }

        // Room Name
        GUI.Label(new Rect(450, 75, 400, 80), PhotonNetwork.CurrentRoom.Name, GUI.skin.label);

        // Player List
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            string isMasterClient = PhotonNetwork.PlayerList[i].IsMasterClient ? " (MasterClient)" : "";
            GUI.Label(new Rect(125, 150 + 40 * i, 400, 80), PhotonNetwork.PlayerList[i].NickName + isMasterClient, GUI.skin.label);
        }
    }
    public override void OnLeftRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(lobbyScene);
    }
}