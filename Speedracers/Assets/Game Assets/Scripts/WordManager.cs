using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    [Header("Word Operations")]
    public TextMeshProUGUI WordToDisplay;
    private string CurrentWord;
    private string RemainingWord;
    private int Index = 0;
    private bool NewWord = true;

    private void Start()
    {
        GenerateWord(); // Generating a random word at start
    }

    private void Update()
    {
        Debug.Log(Index);
        Debug.Log(RemainingWord[Index]);
        InputControls(); // Checking for player input
    }

    void GenerateWord()
    {
        WordGenerator wordGenerator = new WordGenerator(); // Assuming WordGenerator is correctly implemented
        CurrentWord = wordGenerator.GetRandomWord(); // Get a new random word
        RemainingWord = CurrentWord; // Set RemainingWord to the current word
        WordToDisplay.text = RemainingWord; // Display the word in the UI
        Index = 0; // Reset index to start typing from the beginning
        NewWord = false; // Reset NewWord flag
    }

    void InputControls()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    TypeLetter(keyCode.ToString()); // Pass the key pressed as a string
                }
            }
        }
    }

    void TypeLetter(string nextLetter)
    {
        if (CheckForCorrectLetter(nextLetter))
        {
            RemoveLetter(); // Remove the correct letter from the word
        }
        // Optionally, handle incorrect letter typing here
    }

    public bool CheckForCorrectLetter(string checkingLetter)
    {
        Debug.Log(checkingLetter);
        if (Index < RemainingWord.Length && RemainingWord[Index].ToString().ToLower() == checkingLetter.ToLower())
        {
            return true;
        }
        else if(Index > RemainingWord.Length || RemainingWord[Index].ToString().ToLower() != checkingLetter.ToLower())
        {
            return false;
            
        }
        return false;
    }

    void RemoveLetter()
    {
        RemainingWord = RemainingWord.Remove(0, 1);
        WordToDisplay.text = RemainingWord; 
        if (Index < RemainingWord.Length)
        {
            Index++;
        }
        if (HasCompleted())
        {
            NewWord = true;
            GenerateWord(); 
        }
    }

    public bool HasCompleted()
    {
        return RemainingWord.Length == 0; // If we written the entire word, then return a bool to it.
    }
}
