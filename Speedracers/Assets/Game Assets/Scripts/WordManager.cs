using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public float charactersTyped;
    [Header("Word Operations")]
    public TextMeshProUGUI WordToDisplay; // the text we want to continously update
    private string CurrentWord; // stores the new text value
    private string RemainingWord; // stores how many letters we gotta type more
    private int Index = 0; // indexing each character of the generated text
    private bool NewWord = true; // a bool to ensure that we can generate a new word, in start obviously we can
    public bool CanJump = false;

    private void Start()
    {
        CanJump = false;
        GenerateWord(); // Generating a random word at start
    }

    private void Update()
    {
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
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))) // for each values we press in all types of input
        {
            if (Input.GetKeyDown(keyCode)) // if we pass any value from the keyboard
            {
                charactersTyped += 1;
                TypeLetter(keyCode.ToString()); // Pass the key pressed as a string
            }
        }
    }

    void TypeLetter(string nextLetter) // method used for typing letter, only IF we are writing the correct letter
    {
        if (CheckForCorrectLetter(nextLetter)) // nextLetter is the keyCode converted to string
        {
            RemoveLetter(); // Remove the correct letter from the word
        }
        // Optionally, handle incorrect letter typing here
    }

    public bool CheckForCorrectLetter(string checkingLetter) // a bool which returns a true/false, dependent on if we are writing the correct letter
    {
        Debug.Log(checkingLetter);
        if (Index < RemainingWord.Length && RemainingWord[Index].ToString().ToLower() == checkingLetter.ToLower())
        //if the character we are checking upon, if we calculate its position and ensure that it has to be lesser than the string's final position in number. 
        // AND the index of the actual word in lowercase if its equal to what we type
        {
            return true; // return true
        }
        else if (Index > RemainingWord.Length || RemainingWord[Index].ToString().ToLower() != checkingLetter.ToLower()) // else if the opposite
        {
            return false; // false

        }
        return false; // at start, its always false.
    }

    void RemoveLetter() // the process of removing the letter which we have typed correctly
    {

        RemainingWord = RemainingWord.Remove(0, 1); // substring the 1st letter
        WordToDisplay.text = RemainingWord; // update the text variable
        if (HasCompleted()) // if all the characters of the word is 0, means if we typdd all of them correctly
        {
            NewWord = true;
            GenerateWord(); // generate the new word 
            CanJump = true;
            StartCoroutine(Revoking(0.2f));
        }
    }

    public bool HasCompleted()
    {
        return RemainingWord.Length == 0; // If we written the entire word, then return a bool to it.
    }

    IEnumerator Revoking(float time)
    {
        yield return new WaitForSeconds(time);
        CanJump = false;
    }
}