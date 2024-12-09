using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
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
        LogAllPlayersInRoom();
        //We're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        StartCoroutine(DelayedPlayerInstantiation());
    }
    public void LogAllPlayersInRoom()
    {


        Debug.Log("Listing all players in the room:");
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log($"Player: {player.NickName}");
        }


    }
    private IEnumerator DelayedPlayerInstantiation()
    {
        yield return new WaitForSeconds(5.0f);
        PhotonNetwork.LoadLevel("PVPGameplay");
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