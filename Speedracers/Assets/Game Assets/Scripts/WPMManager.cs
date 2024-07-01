using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WPMManager : MonoBehaviour
{
    public float timer;
    public TextMeshProUGUI textOfWPS;
    public WordManager wordManager;
    void Start()
    {
        wordManager = FindObjectOfType<WordManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer = Time.timeSinceLevelLoad;
        var WPS = wordManager.charactersTyped / (timer);
        textOfWPS.text = "WPS: " + Mathf.RoundToInt(WPS).ToString();
    }
}
