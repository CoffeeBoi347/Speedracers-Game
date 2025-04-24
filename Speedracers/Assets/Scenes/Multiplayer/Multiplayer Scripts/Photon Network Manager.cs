using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomListPrefab;
    [SerializeField] private Transform roomListParent;
    public static PhotonNetworkManager instance; // instance of the network manager
    public GameObject Nickname;
    public string playerNickname;
    public TMP_Text pingText;
    [SerializeField] private playerInfo playerInfo_;
    [SerializeField] private GameObject playerInfoParent;
    [SerializeField] private Button playerStatus;
    private string roomName;
    [SerializeField] private TextMeshProUGUI roomErrorMessage;
    [SerializeField] TMP_InputField roomInputField;
    private string time;
    [SerializeField] private TextMeshProUGUI roomNameTxt;

    ExitGames.Client.Photon.Hashtable testing;

    private string botName = "alex_the_goat_";

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DontDestroyOnLoad(instance); // this is a keyword used to refer over objects

        botName = botName + UnityEngine.Random.Range(1000, 9999).ToString();
    }
    private void Update()
    {
        roomNameTxt.text = "ROOM NAME: " + roomName;
        int ping = PhotonNetwork.GetPing();
        pingText.text = "Ping: " + ping.ToString();

    }
    public void ConnectToServer(string playerName)
    {
        PhotonNetwork.ConnectUsingSettings();
        if (!string.IsNullOrEmpty(playerName))
        {
            PlayerPrefs.SetString("PlayerName", playerName);
            PhotonNetwork.NickName = playerName;
            playerNickname = playerName;
        }
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName}");

    }



    public void CreateRoomBot()
    {
        RoomOptions roomOptions = new RoomOptions
        {
            IsOpen = true,  // Room is open for players to join
            IsVisible = true,  // Room is visible in the list
            MaxPlayers = 4 // Max number of players in the room
        };

        string room_name = "random_room" + UnityEngine.Random.Range(0000, 9999); // Random room name
        PhotonNetwork.CreateRoom(room_name, roomOptions); // Create room with random name
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon is Connected");
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }


    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby Successfully!");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.Multiplayer);
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log($"Connected to {PhotonNetwork.ServerAddress}");
    }

    public void OnCreateRoomButtonClicked()
    {
        Debug.Log("Room create button clicked!");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.RoomList);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnPublicButtonClicked()
    {
        Debug.Log("Public button clicked!");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.WaitingArea);
        PhotonNetwork.AutomaticallySyncScene = true;
        StartCoroutine(NewRoomCreated(5f));
    }


    public void CreateRoom(string name, RoomOptions roomOptions)
    {
        string roomName = roomInputField.text;
        name = roomName;

        if (!string.IsNullOrEmpty(roomName))
        {
            roomOptions.IsVisible = true;
            roomOptions.IsOpen = true;

            Debug.Log($"Creating Room: {roomName}");
            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }
        else
        {
            Debug.LogError("Room name is empty!");
        }
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log($"Room has been successfully created -> OnCreatedRoom {roomName}");
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName}");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.WaitingArea);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnJoinRoomButtonPressed()
    {
        string selectedRoomName = roomName;  
        PhotonNetwork.JoinRoom(selectedRoomName);
        Debug.Log($"Joining Room: {selectedRoomName}");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName} -> OnPlayerEnteredRoom()");
        UpdatePlayerList();
        Debug.Log(newPlayer.NickName);

        var _playerInfo = Instantiate(playerInfo_, playerInfoParent.transform);
        _playerInfo.playerName.text = newPlayer.NickName;
    }



    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        roomName = UIManager.Instance.roomNameField.text;
        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName}");
        Debug.Log($"OnJoinedRoom() Player Info -> {playerInfo_.playerName.text}");

        Debug.Log($"OnJoinedRoom() Current Player Count -> {PhotonNetwork.CurrentRoom.PlayerCount}");

        foreach (var i in PhotonNetwork.CurrentRoom.Players)
        {
            var _player = Instantiate(playerInfo_, playerInfoParent.transform);
            _player.playerName.text = i.Value.NickName;
        }

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable();
            time = DateTime.UtcNow.ToString();
            roomProperties.Add("Data", time);

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
            playerStatus.gameObject.SetActive(true);
            Debug.Log("Room Creation Time: " + time);
        }
        else
        {
            var val = PhotonNetwork.CurrentRoom.CustomProperties["Data"].ToString();
            time = val;
            Debug.Log("Room Creation Time: " + time);

            playerStatus.gameObject.SetActive(false);
        }
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        Debug.Log("Master client changed!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log($"Disconnected! {cause}");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.Disconnected);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.LogError($"Failed to create room. Code: {returnCode}, Message: {message}");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Left Room, Returning to Lobby...");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError($"Failed to join room. Code: {returnCode}, Message: {message}");
    }

    public void OpenScene()
    {
        PhotonNetwork.LoadLevel(6);
    }

    public void JoinRoom(string name)
    {
        Debug.Log($"Attempting to join room: {name}");
        PhotonNetwork.JoinRoom(name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        Debug.Log($"OnRoomListUpdate called. {roomList.Count} rooms found.");
        RefreshRoomList();

        foreach (var i in roomList)
        {
            if (i.RemovedFromList)
                continue;

            Debug.Log($"Room Found: {i.Name} | Players: {i.PlayerCount}/{i.MaxPlayers} | Open: {i.IsOpen}");

            GameObject roomInstance = Instantiate(roomListPrefab, roomListParent);
            RoomName roomListScriptComponent = roomInstance.GetComponent<RoomName>();

            if (roomListScriptComponent != null)
            {
                roomListScriptComponent.roomName.text = i.Name;
                roomListScriptComponent.playersJoined.text = $"{i.PlayerCount}/{i.MaxPlayers}";
                roomListScriptComponent.roomStatusImage.color = i.IsOpen ? Color.green : Color.red;
                roomListScriptComponent.roomJoin.interactable = i.IsOpen;

                // Set up a join button in the UI to manually join the room
                roomListScriptComponent.roomJoin.onClick.AddListener(() => JoinRoom(i.Name));
            }
        }
    }


    private void RefreshRoomList()
    {
        foreach (Transform child in roomListParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdatePlayerList()
    {
        foreach (Transform child in playerInfoParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            var playerInstance = Instantiate(playerInfo_, playerInfoParent.transform);
            playerInstance.playerName.text = player.NickName;
        }
    }

    private IEnumerator NewRoomCreated(float time)
    {
        yield return new WaitForSeconds(time);
        CreateRoomBot();
    }

}