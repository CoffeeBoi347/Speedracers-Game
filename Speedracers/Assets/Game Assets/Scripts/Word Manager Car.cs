using System.Collections;
using TMPro;
using UnityEngine;

public class WordManagerCar : MonoBehaviour
{
    public static WordManagerCar instance;
    [Header("Numeric Values")]

    public int charactersTyped;
    public int wordsTyped;
    public float time;
    public int index;
    public float timeSinceLastType;

    [Header("Text References")]

    public TextMeshProUGUI textBeingTyped;
    public TextMeshProUGUI wordToDisplay;
    public string currentWord;
    public string remainingWord;

    [Header("Boolean Checks")]

    public bool allowToAdd = false;
    public bool newWord = false;
    public bool hasTyped = false;
    public bool canMove = false;
    public bool hasGameBegan = false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        GenerateWord();
    }

    private void Update()
    {
        InputControls();
        time += Time.deltaTime;
    }

    void GenerateWord()
    {
        WordGenerator wordGenerator = new WordGenerator();
        currentWord = wordGenerator.GetRandomWord();
        remainingWord = currentWord;
        wordToDisplay.text = currentWord;
        newWord = false;
    }

    void InputControls()
    {
        if (Input.inputString.Length > 0)
        {
            hasGameBegan = true;
            char c = Input.inputString[0];
            if (char.IsLetter(c))
            {
                if (allowToAdd)
                {
                    charactersTyped += 1;
                }
                hasTyped = true;
                timeSinceLastType += Time.deltaTime;
                TypeLetter(c.ToString());
            }
        }
    }


    void TypeLetter(string letterTyped)
    {
        if (CheckForCorrectLetter(letterTyped))
        {
            Time.timeScale = 1f;
            timeSinceLastType = 0f;
            textBeingTyped.text += letterTyped;
            RemoveLetter();
        }
    }

    bool CheckForCorrectLetter(string letterTyped)
    {
        if (index < remainingWord.Length && char.ToUpper(remainingWord[index]) == char.ToUpper(letterTyped[0]))
        {
            return true;
        }
        return false;
    }


    void RemoveLetter()
    {   
        if(remainingWord.Length > 0)
        {
            remainingWord = remainingWord.Substring(1);
            wordToDisplay.text = remainingWord;
        }

        if (hasCompleted())
        {
            textBeingTyped.text = string.Empty;
            GenerateWord();
        }
    }

    public bool hasCompleted()
    {
        canMove = true;
        StartCoroutine(SetCanMoveToFalse(0.1f));
        return remainingWord.Length == 0;
    }

    IEnumerator SetCanMoveToFalse(float time)
    {
        yield return new WaitForSeconds(time);
        canMove = false;
    }
}