using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public int JumpPower;
    private bool AllowJump;
    private bool HasWrittenWholeWord;
    private bool IsActiveWord;
    public TextMeshProUGUI WordToDisplay;

    private void Start()
    {
        DisplayTheWord();
    }

    void DisplayTheWord()
    {
        WordGenerator wordGenerator = new WordGenerator();
        WordToDisplay.text = wordGenerator.GetRandomWord().ToString();
    }
}
