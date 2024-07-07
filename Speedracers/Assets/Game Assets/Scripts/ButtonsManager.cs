using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject typeYourText;
    public TextMeshProUGUI StartText;
    public bool SetTime;
    void Start()
    {
        SetTime = false;
        Time.timeScale = 0f;
    }

    public void NextScene()
    {
        SetTime = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void ClickButton()
    {
        Time.timeScale = 1f;
        SetTime = true;
        float Timer = Time.timeSinceLevelLoad;
        typeYourText.SetActive(false);
        StartText.text = "Time: " + Timer.ToString();
    }
}
