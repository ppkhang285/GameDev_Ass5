using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{
    string playerName = "1";
    string gameVersion = "1";
    string sceneName = "RoomScene";
    int MAX_PLAYER = 2;
    Vector2 roomListScroll = Vector2.zero;
    int Max_PLAYER_ROOM = 2;
    List<RoomInfo> createdRooms = new List<RoomInfo>();
    string roomName = "New Room";
    bool isJoining = false;

    // Aspect ratio settings
    float targetAspect = 16f / 16f;
    float scale;

    void Start()
    {
        // Calculate the scaling factor for 16:10 aspect ratio
        scale = (Screen.width / (float)Screen.height) / (2.5f);

        string randomNickname = "Player_" + Random.Range(1000, 9999);
        PhotonNetwork.NickName = randomNickname;
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = gameVersion;
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "eu"; // Europe
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void OnGUI()
    {
        // Adjust window dimensions to fit 16:10 aspect ratio
        float width = 1920 * scale;
        float height = 1080 * scale *1.25f;
        //how to add font size
        GUI.skin.label.fontSize = 36;
        GUI.skin.textField.fontSize = 36;
        GUI.skin.button.fontSize = 36;

        Rect lobbyPanel = new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height);
        GUI.Window(0, lobbyPanel, LobbyWindow, "Lobby");
    }

    void LobbyWindow(int index)
    {
        // Scale GUI elements
        float labelWidth = 170 * scale;
        float textFieldWidth = 500 * scale;
        float buttonWidth = 300 * scale;
        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Trạng thái " + PhotonNetwork.NetworkClientState);

        if (isJoining || !PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GUI.enabled = false;
        }
        
        GUILayout.FlexibleSpace();

        roomName = GUILayout.TextField(roomName, GUILayout.Width(textFieldWidth));
        if (GUILayout.Button("Tạo phòng", GUILayout.Width(buttonWidth)))
        {
            if (roomName != "")
            {
                isJoining = true;
                RoomOptions option = new RoomOptions
                {
                    IsOpen = true,
                    IsVisible = true,
                    MaxPlayers = MAX_PLAYER
                };
                PhotonNetwork.JoinOrCreateRoom(roomName, option, TypedLobby.Default);
            }
        }
        GUILayout.EndHorizontal();

        roomListScroll = GUILayout.BeginScrollView(roomListScroll, GUILayout.Width(600 * scale), GUILayout.Height(800 * scale));
        // Room List
        if (createdRooms.Count == 0)
        {
            GUILayout.Label("Chưa có phòng nào");
        }
        else
        {
            for (int i = 0; i < createdRooms.Count; i++)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Label(createdRooms[i].Name, GUILayout.Width(400 * scale));
                GUILayout.Label(createdRooms[i].PlayerCount + "/" + createdRooms[i].MaxPlayers);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Tham gia", GUILayout.Width(buttonWidth)))
                {
                    isJoining = true;
                    PhotonNetwork.NickName = playerName;
                    PhotonNetwork.JoinRoom(createdRooms[i].Name);
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndScrollView();
        GUILayout.BeginVertical();
        GUILayout.Space(200);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Player Name", GUILayout.Width(labelWidth + 100));
        playerName = GUILayout.TextField(playerName, GUILayout.Width(textFieldWidth));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndVertical();

        if (isJoining)
        {
            GUI.enabled = true;
            GUI.Label(new Rect((900 * scale) / 2 - 50, (400 * scale) / 2 - 10, 100, 20), "Connecting...");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Mất kết nối: " + cause);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Đã kết nối");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        isJoining = false;
        Debug.Log(returnCode + message);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Joined");
        PhotonNetwork.NickName = playerName;
        PhotonNetwork.LoadLevel(sceneName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have received the Room list");
        createdRooms = roomList;
    }
}
