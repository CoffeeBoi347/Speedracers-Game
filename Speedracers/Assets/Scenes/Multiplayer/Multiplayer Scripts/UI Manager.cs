using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Realtime;

public class UIManager : Singleton<UIManager>
{
    [Header("Footer")]

    [SerializeField] private TMP_Text playerNameTxt;

    [Header("Nickname")]

    [SerializeField] private Button playButton;
    [SerializeField] private TMP_InputField playerNameField;

    [Header("RoomName")]

    [SerializeField] private Slider timeOfGame;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private Slider maxPlayersVal;
    [SerializeField] private Button viewRoomButton;
    [SerializeField] private Button joinRoomButton;
    [SerializeField] private Button quitRoomButton;

    [Header("Create Room")]
    public TMP_InputField roomNameField;


    [SerializeField] private Button createRoom;

    private void Awake()
    {
        if(playerNameField == null)
        {
            return;
        }
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerNameField.text = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            playerNameField.text = "Player_" + Random.Range(1000, 9999);
        }
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
        createRoom.onClick.AddListener(OnCreateRoomPressed);
        viewRoomButton.onClick.AddListener(OnRoomListButtonPressed);
    }

    void OnPlayButtonPressed()
    {
        if (!PhotonNetwork.IsConnected)
        {
            playerNameTxt.text = playerNameField.text;
            Debug.Log("Connecting...");
            PhotonNetworkManager.instance.ConnectToServer(playerNameField.text);
        }
        else
        {
            Debug.Log("Connected To Photon!");
        }
    }

    void OnWaitingArea()
    {
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.WaitingArea);
        Debug.Log("Waitin  for players...");
    }

    private void OnRoomListButtonPressed()
    {
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.ViewAllRooms);
        Debug.Log("Room List Pressed");
    }

    void OnCreateRoomPressed()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (int)maxPlayersVal.value;
        roomOptions.EmptyRoomTtl = 45000;
        PhotonNetworkManager.instance.CreateRoom(roomName.text, roomOptions);
        Debug.Log("Room created! UI Manager!");
        OnWaitingArea();

    }

    public void OnQuitRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(this);
            GameManagerMultiplayer.Instance.OpenMenu(EMenuName.ViewAllRooms);
        }
    }
}
