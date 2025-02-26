using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class WordManager : MonoBehaviour
{
    public float charactersTyped; // for storing score
    public PlaneMovementPlayer playerMovementPlayer;
    [Header("Word Operations")]
    public TextMeshProUGUI WordToDisplay; // the text we want to continously update
    public TextMeshProUGUI WordsBeingTyped; // words you are typing
    public string CurrentWord; // stores the new text value
    public string RemainingWord; // stores how many letters we gotta type more
    private int Index = 0; // indexing each character of the generated text
    private bool NewWord = true; // a bool to ensure that we can generate a new word, in start obviously we can
    public bool CanJump = false;
    public int JumpsCount = 0;
    public float TimeSurvived;
    public float WPM;
    public AudioSource TypingSFX;
    private int WordsCompleted;
    public float Rating;
    public int actualRating;
    public bool AllowToAdd;
    public bool HasTyped;
    public string currentLetter;
    private void Start()
    {
        AllowToAdd = false;
        TypingSFX.Stop();
        playerMovementPlayer = FindObjectOfType<PlaneMovementPlayer>();
        CanJump = false; // you cant jump in starting
        GenerateWord(); // Generating a random word at start
    }

    private void Update()
    {
        Debug.Log(RemainingWord[Index]);
        currentLetter = RemainingWord[Index].ToString();
        InputControls(); // Checking for player input

        if(!playerMovementPlayer.HasCollided == true)
        {
            StartCoroutine(CountText(1));
        }

        WPM = WordsCompleted / (TimeSurvived / 120);
        CalculateRating();
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
                HasTyped = true;
                if(AllowToAdd == true)
                    charactersTyped += 1;
                TypeLetter(keyCode.ToString()); // Pass the key pressed as a string
            }
        }
    }

    void TypeLetter(string nextLetter) // method used for typing letter, only IF we are writing the correct letter
    {
        if (CheckForCorrectLetter(nextLetter)) // nextLetter is the keyCode converted to string
        {
            WordsBeingTyped.text += nextLetter; // for typing off the words you're writing
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
            WordsBeingTyped.text = string.Empty;
            NewWord = true;
            WordsCompleted++;
            GenerateWord(); // generate the new word 
        }
    }

    public bool HasCompleted()
    {
        JumpsCount++;
        CanJump = true;
        TypingSFX.Play();
        StartCoroutine(Revoking(0.05f));
        return RemainingWord.Length == 0; // If we written the entire word, then return a bool to it.
    }

    IEnumerator Revoking(float time)
    {
        yield return new WaitForSeconds(time);
        CanJump = false;
    }

    IEnumerator CountText(int time)
    {
        yield return new WaitForSeconds(time);
        TimeSurvived += Time.deltaTime;
    }

    void CalculateRating()
    {
        if (TimeSurvived > 0)
        {
            // Calculate accuracy as a percentage
            float accuracy = charactersTyped > 0 ? ((float)WordsCompleted / charactersTyped) * 100 : 0;
            // if characters typed are more than 0, then we divide the no. of words we write by the characters and
            // multiply it by 100, else... if characters typed are 0 then multiply by 0. ofc result will be 0 -_-

            // Calculate WPM
            float wpm = WordsCompleted / (TimeSurvived / 120);

            // Normalize each metric to a scale of 0 to 10
            float accuracyScore = Mathf.Clamp(accuracy / 10, 0, 10);
            float wpmScore = Mathf.Clamp(wpm / 10, 0, 10);
            float wordsCompletedScore = Mathf.Clamp(WordsCompleted / 10, 0, 10);

            // Calculate the average of the three scores
            Rating = (int)((accuracyScore + wpmScore + wordsCompletedScore) / 3);
        }
        else
        {
            Rating = 0;
        }
    }
}