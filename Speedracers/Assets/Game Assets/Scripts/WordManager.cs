using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossWordManager : MonoBehaviour
{
    public static BossWordManager instance;
    [Header("Strings")]

    public string wordBeingTyped = ""; // the word which we are currently typing
    public string remainingWord; // remaining word 
    public string targetWord; // target word, locking the final word
    private string letterRef;

    [Header("Word Operations")]

    public List<string> possibleWords; // once list has only 1 element. then we assign target word to it
    public PlaneMovementPlayer1 playerObj; // player object
    public PlayerActionsManager playerActionsManager;
    public bool canJump = false;
    public bool allowToAdd = false;
    public bool foundRemainingWord = false;
    public bool hasTyped = false;
    public bool hasGameBegan = false;
    public AudioSource jumpSFX;

    [Header("Numericals")]

    public int charactersTyped;
    public int index = 0;
    public int wordCheckEligibility = 0;
    [Header("Texts")]

    public TMP_Text wordsBeingTypedTxt;
    public TMP_Text remainingWordTxt;
    public GameObject pressTypeTxt;
    public TMP_Text possibleGuessesTxt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(remainingWordTxt != null)
        {
            remainingWordTxt.text = "";
        }
        if (playerObj == null)
        {
            playerObj = FindObjectOfType<PlaneMovementPlayer1>();
        }

        if (jumpSFX != null)
        {
            jumpSFX.Stop();
        }
        allowToAdd = true;
        pressTypeTxt.SetActive(false);
    }


    private void Update()
    {
        InputControls();
    }

    void InputControls()
    {
        possibleGuessesTxt.text = possibleWords.Count > 0 ? string.Join(", ", possibleWords) : "";

        if (Input.inputString.Length > 0)
        {
            foreach (char typedChar in Input.inputString)
            {
                if (char.IsLetter(typedChar))
                {
                    string letter = typedChar.ToString().ToUpper();
                    charactersTyped++;
                    index++;

                    wordsBeingTypedTxt.text += letter;
                    wordBeingTyped += letter; 
                    hasTyped = true;
                    canJump = true;
                    hasGameBegan = true;
                    if (charactersTyped >= wordCheckEligibility)
                    {
                        if (possibleWords.Count <= 0)
                        {
                            ResetWords();
                        }
                        else if (possibleWords.Count == 1)
                        {
                            targetWord = possibleWords[0]; 
                            RemoveRemainingWords(targetWord);
                        }
                    }

                    GeneratePossibleGuesses(letter, wordBeingTyped);
                    RemoveWord(wordBeingTyped);
                }
            }
        }
        else
        {
            canJump = false;
        }

        if (Input.GetKeyDown(KeyCode.Backspace) && wordBeingTyped.Length > 0)
        {
            wordBeingTyped = wordBeingTyped.Substring(0, wordBeingTyped.Length - 1);
            wordsBeingTypedTxt.text = wordBeingTyped;
            charactersTyped = Mathf.Max(0, charactersTyped - 1);
            RemoveWord(wordBeingTyped);
        }
    }

    void GeneratePossibleGuesses(string inputLetter, string inputWord)
    {
        WordGenerator wordGenerator = new WordGenerator();
        possibleWords.Clear();
        foreach (var values in wordGenerator.bossFightWords)
        {
            if (inputWord.Length < values.Length &&
                inputLetter == values[inputWord.Length - 1].ToString().ToUpper() &&
                inputWord == values.Substring(0, inputWord.Length).ToUpper())
            {
                possibleWords.Add(values);
            }
        }
    }

    void RemoveWord(string inputWord)
    {
        Debug.Log("REMOVE WORD IS CALLED!");
        for (int i = possibleWords.Count - 1; i >= 0; i--)
        {
            if (possibleWords[i].ToUpper() == inputWord)
            {
                Debug.Log($"Matched word: {inputWord}");
                possibleWords.RemoveAt(i); 
                wordsBeingTypedTxt.text = ""; 
                wordBeingTyped = ""; 
                remainingWordTxt.text = ""; 
                charactersTyped = 0; 
                index = 0; 
            }
        }
    }

    void RemoveRemainingWords(string targettedWord)
    {
        if (!string.IsNullOrEmpty(targettedWord))
        {
            foundRemainingWord = true;
            remainingWord = targettedWord.Substring(wordBeingTyped.Length);
            remainingWordTxt.text = remainingWord;

            if (wordBeingTyped.Length >= targettedWord.Length)
            {
                playerActionsManager.ExecuteAction(targettedWord.ToUpper());
                wordsBeingTypedTxt.text = "";
                wordBeingTyped = "";
                remainingWord = "";
                foundRemainingWord = false;
            }
        }
    }

    void ResetWords()
    {
        if (string.IsNullOrEmpty(remainingWord) && foundRemainingWord)
        {
            foundRemainingWord = false;
            remainingWord = "";
            wordBeingTyped = "";
            targetWord = "";
        }
    }

    bool SetGameBeganToTrue()
    {
        return hasGameBegan = true;
    }
}