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
    [SerializeField] private RoomName roomListPrefab;
    [SerializeField] private Transform roomListParent;
    public static PhotonNetworkManager instance; // instance of the network manager
    public GameObject Nickname;
    public TMP_Text pingText;
    [SerializeField] private playerInfo playerInfo_;
    [SerializeField] private GameObject playerInfoParent;
    [SerializeField] private Button playerStatus;
    private string roomName;

    private string time;

    ExitGames.Client.Photon.Hashtable testing;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DontDestroyOnLoad(instance); // this is a keyword used to refer over objects
    }
    private void Update()
    {

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
        }
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName}");

    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined Lobby ");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Photon is Connected");
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.AutomaticallySyncScene = true;
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.Multiplayer);
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log($"Connected to {PhotonNetwork.ServerAddress}");
        Debug.Log($"Btw your IP address is {PhotonNetwork.IsMasterClient}");
        PublicRoomRandom();
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
    }


    public void CreateRoom(string name, RoomOptions roomOptions)
    {
        PhotonNetwork.CreateRoom(name, roomOptions);
        Debug.Log($"Creating Room: {name}");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log($"Room has been successfully created -> OnCreatedRoom {roomName}");
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName}");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.WaitingArea);
        PhotonNetwork.AutomaticallySyncScene = true;
        StartCoroutine(AddBots(3, 3));
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log($"Current Nickname: {PhotonNetwork.NickName} -> OnPlayerEnteredRoom()");

        Debug.Log(newPlayer.NickName);
        Instantiate(playerInfo_, playerInfoParent.transform);

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

        playerInfo_.playerName.text = PhotonNetwork.NickName;
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

    private IEnumerator AddBots(int botCount, float delay)
    {
        for (int i = 0; i < botCount; i++)
        {
            yield return new WaitForSeconds(delay);
            string botName = "Bot" + UnityEngine.Random.Range(1000, 9999);
            var botInfo = Instantiate(playerInfo_, playerInfoParent.transform);
            botInfo.playerName.text = botName;
        }
    }

    private void PublicRoomRandom()
    {
        string randomRoomName = "Room_" + UnityEngine.Random.Range(1000, 9999);
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true
        };

        Debug.Log($"Creating Room: {randomRoomName}");
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        for (int i = 0; i >= roomListParent.childCount; i++)
        {
            Destroy(roomListParent.GetChild(i).gameObject);
        }

        foreach (var i in roomList)
        {
            RoomName rName = Instantiate(roomListPrefab, roomListParent);
            rName.roomName.text = i.Name.ToString();
            rName.playersJoined.text = (i.PlayerCount + "/" + i.MaxPlayers).ToString();

            if (i.IsOpen)
            {
                if (i.PlayerCount >= i.MaxPlayers / 2)
                {
                    rName.roomStatusImage.color = Color.red;
                }
                else
                {
                    rName.roomStatusImage.color = Color.green;
                }
            }
            else
            {
                rName.roomStatusImage.color = Color.red;
                rName.roomJoin.interactable = false;
            }
        }
    }
}