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
        playerPrefabName="Prefabs/Characters/" + GameManager.Instance.CharacterType;
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
        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber;
        Vector3 spawnPosition = spawnLocations[playerIndex - 1].transform.position;
        GameObject networkPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, spawnRotation);
        players[playerIndex - 1] = networkPlayer;

        // If the player instance belongs to the local player, set up the camera and HUD
        if (networkPlayer.GetComponent<PhotonView>().IsMine)
        {
            // Setup camera and HUD for the local player
            //cameraManager.Setup(networkPlayer.transform);
            //hudManager.Setup(networkPlayer.GetComponent<Character>());
        }
    }
}
