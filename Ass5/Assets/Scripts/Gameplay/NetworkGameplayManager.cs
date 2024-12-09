using Photon.Pun;
using UnityEngine;

public class NetworkGameplayManager : MonoBehaviour
{
    public string playerPrefabName = "Prefabs/Characters/Knight"; // Path to the player prefab in Resources folder
    public Vector3 spawnPoint1 = new Vector3(5, 0, 0); // Position for Player 1
    public Vector3 spawnPoint2 = new Vector3(-5, 0, 0); // Opposite position for Player 2
    public Quaternion spawnRotation = Quaternion.identity; // Default rotation for players

    public Camera mainCamera; // Reference to the main camera
    public Vector3 cameraOffset = new Vector3(0, 3, -10); // Camera offset to follow the player

    void Start()
    {
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
        Vector3 spawnPosition = (playerIndex == 1) ? spawnPoint1 : spawnPoint2;

        // Instantiate the player prefab using PhotonNetwork.Instantiate
        GameObject networkPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, spawnRotation);

        Debug.Log($"Network player {playerIndex} spawned at position {spawnPosition}");

        // If this is the local player's character, move the camera to follow it
        if (networkPlayer.GetComponent<PhotonView>().IsMine)
        {
            SetCameraFollow(networkPlayer.transform);
        }
    }

    void SetCameraFollow(Transform playerTransform)
    {
        if (mainCamera == null) return;

        // Position the camera based on the player's position and offset
        mainCamera.transform.position = playerTransform.position + cameraOffset;

        // Make the camera look at the player
        mainCamera.transform.LookAt(playerTransform);
    }
}
