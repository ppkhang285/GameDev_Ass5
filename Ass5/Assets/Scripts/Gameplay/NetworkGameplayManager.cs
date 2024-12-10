using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class NetworkGameplayManager : MonoBehaviour
{
    public static NetworkGameplayManager Instance { get; private set; }

    public string playerPrefabName = "Prefabs/Characters/Knight"; // Path to the player prefab in Resources folder
    public Quaternion spawnRotation = Quaternion.identity; // Default rotation for players

    public CameraManager cameraManager;
    public HUDManager hudManager;

    [SerializeField]
    private List<GameObject> spawnLocations;

    public List<GameObject> players = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        for (int i = 0; i < 2; i++)
        {
            players.Add(null);
        }

        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}, Player Count: {PhotonNetwork.CurrentRoom.PlayerCount}");
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            SpawnNetworkPlayer();
        }
        else
        {
            Debug.LogError("Not connected to Photon or not in a room.");
        }
    }

    void SpawnNetworkPlayer()
    {
        // Get the local player's actor number to decide spawn position
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;

        // Set spawn position based on the player's actor number (1 or 2)
        Vector3 spawnPosition = spawnLocations[playerIndex - 1].transform.position;

        // Instantiate the player prefab using PhotonNetwork.Instantiate
        GameObject networkPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, spawnRotation);
        players[playerIndex - 1] = networkPlayer;
        if (networkPlayer.GetComponent<PhotonView>().IsMine)
        {
            //cameraManager.Setup(networkPlayer.transform);
            //hudManager.Setup(networkPlayer.GetComponent<Character>());
            
        }
    }
}
