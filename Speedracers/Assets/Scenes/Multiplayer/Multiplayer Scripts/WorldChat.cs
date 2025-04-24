using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldChat : MonoBehaviour
{
    public PhotonNetworkManager networkManager;
    public bool isOpen = false;
    public GameObject chatOpt;
    public TMP_InputField textToSend;
    public TMP_Text message;
    public TMP_Text username;
    public GameObject chatBox;
    public Transform spawnPos;

    [Header("Troll Message")]

    public GameObject spawnPos2;
    public TMP_Text message2;
    public TMP_Text username2;


    private void Start()
    {
        networkManager = FindObjectOfType<PhotonNetworkManager>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            chatOpt.SetActive(true);
            isOpen = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            chatOpt.SetActive(false);
            isOpen = false;
        }

        if (isOpen)
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                string text = networkManager.playerNickname;
                string msg= textToSend.text;
                username.text = text;
                message.text = msg;
                chatBox.SetActive(true);
                StartCoroutine(SendTrollMessage(3f));
            }
        }
    }

    private IEnumerator SendTrollMessage(float time)
    {
        yield return new WaitForSeconds(time);
        username2.text = "kiwami";
        message2.text = "nahhhhh gtfo";
        spawnPos2.SetActive(true);
    }
}
