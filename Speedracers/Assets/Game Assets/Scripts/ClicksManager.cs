using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClicksManager : MonoBehaviour
{
    public bool EventGoingOn;
    public bool AllowedToClick;
    public int clicks;
    public float timerVar;
    public bool HasClicked;
    public bool CanClick = true;
    public float CPS;
    public TextMeshProUGUI cpsText;
    public Animator anim;
    private void Start()
    {
        EventGoingOn = false;
        cpsText.text = "CPS : " + CPS.ToString("F3"); // the input takes a string, f3 denotes the first 3 numbers after the decimal point
        HasClicked = false;
        CanClick = true;
        Time.timeScale = 0f;
    }

    public void Click()
    {
        if(EventGoingOn == false)
        {
            if (CanClick == true)
            {
                clicks++;
                AllowedToClick = true;
                HasClicked = true;
            }
        }
    }
    void Update()
    {
        if(AllowedToClick == true)
        {
            Time.timeScale = 1f;
            timerVar += Time.deltaTime;
            if(timerVar <= 5f)
            {
                // Check if the player has pressed the button, if he has pressed the button then increase the click by clicks++ else false.
                if(HasClicked == true)
                {
                    CPS = clicks / timerVar;
                    cpsText.text = "CPS : " + CPS.ToString("F3");
                    Debug.Log(CPS);
                }
            }
            else if(timerVar >= 5f)
            {
                timerVar = 5f;
                CanClick = false;
                EventGoingOn = false;
            }
        }
    }
}
