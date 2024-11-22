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
    [Header("Nickname")]

    [SerializeField] private Button playButton;
    [SerializeField] private TMP_Text playerName;

    [Header("RoomName")]

    [SerializeField] private Slider timeOfGame;
    [SerializeField] private TMP_Text roomName;
    [SerializeField] private Slider maxPlayersVal;
    [SerializeField] private Button createRoomButton;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            playerName.text = "Player_" + Random.Range(1000, 9999);
        }
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonPressed);
    }

    void OnPlayButtonPressed()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting...");
            PhotonNetworkManager.instance.ConnectToServer(playerName.text);
        }
        else
        {
            Debug.Log("Connected To Photon!");
        }
    }

    private void OnRoomListButtonPressed()
    {
        GameManagerMultiplayer.Instance.OpenMenu(EMenuName.RoomList);
        Debug.Log("Room List Pressed");
    }

    void OnCreateRoomPressed()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (int)maxPlayersVal.value;
        roomOptions.EmptyRoomTtl = 45000;

    }
}
