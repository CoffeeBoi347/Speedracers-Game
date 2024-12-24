using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class RoomName : MonoBehaviour
{
    public TMP_Text roomName;
    public TMP_Text playersJoined;
    public Image roomStatusImage;
    public Button roomJoin;

    public void JoinRoom()
    {
        PhotonNetworkManager.instance.JoinRoom(roomName.text);
    }
}
