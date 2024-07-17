using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    public WordManager wordManager;
    public GameObject buttonPrefab;
    public GameObject typeYourText;
    public TextMeshProUGUI StartText;
    public bool SetTime;
    void Start()
    {
        wordManager = FindObjectOfType<WordManager>();  
        SetTime = false;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (wordManager.HasTyped == true)
        {
            wordManager.AllowToAdd = true;
            Time.timeScale = 1f;
            SetTime = true;
            float Timer = Time.timeSinceLevelLoad;
            typeYourText.SetActive(false);
            StartText.text = "Time: " + Timer.ToString();
        }
    }
    public void NextScene()
    {
        SetTime = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

    public void ClickButton()
    {
        wordManager.AllowToAdd = true;
        Time.timeScale = 1f;
        SetTime = true;
        float Timer = Time.timeSinceLevelLoad;
        typeYourText.SetActive(false);
        StartText.text = "Time: " + Timer.ToString();
    }

    public void Close()
    {
        Application.Quit();
    }
}
