using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using TMPro;

public class PhotonNetworkManager : MonoBehaviourPunCallbacks
{
    public static PhotonNetworkManager instance; // instance of the network manager
    public GameObject Nickname;
    public TMP_Text pingText;
    [SerializeField] private playerInfo playerInfo_;
    [SerializeField] private GameObject playerInfoParent;

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
        PhotonNetwork.AutomaticallySyncScene = true;
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.Multiplayer);
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log($"Connected to {PhotonNetwork.ServerAddress}");
        Debug.Log($"Btw your IP address is {PhotonNetwork.IsMasterClient}");
    }

    public void OnCreateRoomButtonClicked()
    {
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.RoomList);
        PhotonNetwork.AutomaticallySyncScene = true ;
    }

    public void OnPublicButtonClicked()
    {
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.WaitingArea);
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    

    public void CreateRoom(string name, RoomOptions roomOptions)
    {
        PhotonNetwork.CreateRoom(name, roomOptions);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("Room is created");

        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.WaitingArea);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.RoomList);
        PhotonNetwork.AutomaticallySyncScene = true;

        string playerNickname = PhotonNetwork.NickName;
        if (!string.IsNullOrEmpty(playerNickname) && playerInfo_ != null)
        {
            playerInfo_.playerName.text = playerNickname;

            Instantiate(playerInfo_, playerInfoParent.transform);


            foreach (var i in PhotonNetwork.CurrentRoom.Players)
            {
                var _player = Instantiate(playerInfo_, playerInfoParent.transform);
                _player.playerName.text = i.Value.NickName;
            }

        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.NickName);

        var _playerInfo = Instantiate(playerInfo_, playerInfoParent.transform);
        _playerInfo.playerName.text = newPlayer.NickName;


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
        StartCoroutine(TryReconnect());
    }

    private IEnumerator TryReconnect()
    {
        int maxAttempts = 5;
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            attempt++;
            Debug.Log($"Reconnection attempt {attempt}/{maxAttempts}...");

            if (PhotonNetwork.Reconnect())
            {
                Debug.Log("You were disconnected! Welcome back");
                GameManagerMultiplayer.Instance.OpenMenu(EMenuName.Menu);
                PhotonNetwork.AutomaticallySyncScene = true;
                yield break;

            }

            yield return new WaitForSeconds(2.0f);
        }

        Debug.Log("Failed to reconnect after multiple attempts.");
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.Menu);

    }

}
