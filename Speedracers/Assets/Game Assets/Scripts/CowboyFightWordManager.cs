using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CowboyFightWordManager : MonoBehaviour
{
    [Header("Instance")]

    public static CowboyFightWordManager instance;

    [Header("Strings")]

    public string wordBeingTyped = "";
    public string targetWord;
    public string letterTyped;
    public List<string> possibleWords;

    [Header("Text References")]

    public List<string> wordsTyped = new List<string>();
    public TMP_Text wordBeingTypedTxt;
    public TMP_Text possibleWordsTxt;

    [Header("Other Scripts")]

    public CowboyPlayer player;
    public CowboyActionManager actionManager;
    public WordGenerator wordGenerator;
    public AttackManager attackManager;

    [Header("Booleans")]

    public bool isActionRunning = false;
    public bool canMove;
    public bool canJump;
    public bool allowToAdd;
    public bool gameBegan;
    public bool isTyping = false;

    [Header("WPM References")]

    public int charactersTyped;

    [Header("Audio Source")]

    public AudioSource audioSourcePlayer;
    public AudioClip jumpPlayer;
    public AudioClip attackPlayer;

    [Header("Keyboard Settings")]

    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject[] keys;
    private GameObject currentKeyPressed;
    private int index = 0;

    [Header("Positions")]

    private Vector3 startingPositionLeftHand;
    private Vector3 startingPositionRightHand;

    private void Awake()
    {
        if(instance == null)
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
        Time.timeScale = 0f;
        attackManager = FindObjectOfType<AttackManager>();
        if(audioSourcePlayer != null)
        {
            audioSourcePlayer.Stop();
        }

        if(wordGenerator != null)
        {
            wordGenerator = FindObjectOfType<WordGenerator>();
        }

        if(player != null)
        {
            player = FindObjectOfType<CowboyPlayer>();
        }

        if(actionManager != null)
        {
            actionManager = FindObjectOfType<CowboyActionManager>();
        }

        allowToAdd = false;
        gameBegan = false;
        possibleWordsTxt.text = "";
    }

    private void Update()
    {
        InputHandling();

        if (isTyping && wordBeingTyped != "JUMP" && wordBeingTyped == targetWord)
        {
            actionManager.ExecuteAction(targetWord);
        }

        if (targetWord == "RUN")
        {
            Debug.Log("RUN!");
            actionManager.ExecuteAction("RUN");
        }

        if (targetWord == "WALK")
        {
            Debug.Log("WALK!");
            actionManager.ExecuteAction("WALK");
        }

        if (targetWord == "FLIP" && player.action == CurrentAction.Run)
        {
            player.action = CurrentAction.Idle;
            player.StopPlayer();
        }

    }

    void InputHandling()
    {
        Debug.Log(wordBeingTyped);
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                letterTyped = c.ToString();
                StartCoroutine(player.HasNotType(1f));
                isTyping = true;
                string charLetter = c.ToString().ToUpper();
                wordBeingTyped += charLetter;
                wordBeingTypedTxt.text = wordBeingTyped;
                charactersTyped++;
                Time.timeScale = 1f;
                CheckForPossibleGuesses(wordBeingTyped);
                index++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (wordBeingTyped.Length > 0)
            {
                wordBeingTyped = wordBeingTyped.Substring(0, wordBeingTyped.Length - 1);
                wordBeingTypedTxt.text = wordBeingTyped;
            }
        }

        foreach(var key in keys)
        {
            if(key.name.ToString().ToUpper() == letterTyped.ToUpper())
            {
                currentKeyPressed = key;
            }
        }

        float distanceA = Vector3.Distance(leftHand.transform.position, currentKeyPressed.transform.position);
        float distanceB = Vector3.Distance(rightHand.transform.position, currentKeyPressed.transform.position);

        if(distanceA < distanceB)
        {
            leftHand.transform.position = new Vector2(currentKeyPressed.transform.position.x, currentKeyPressed.transform.position.y - 80f);
        }

        else if(distanceB < distanceA)
        {
            rightHand.transform.position = new Vector2(currentKeyPressed.transform.position.x, currentKeyPressed.transform.position.y - 80f);
        }

        else
        {
            if (UnityEngine.Random.value > 0.5f)
            {
                leftHand.transform.position = new Vector2(currentKeyPressed.transform.position.x, currentKeyPressed.transform.position.y - 80f);
            }
            else
            {
                rightHand.transform.position = new Vector2(currentKeyPressed.transform.position.x, currentKeyPressed.transform.position.y - 80f);
            }
        }

        HasCompletedWord();
    }

    void CheckForPossibleGuesses(string wordTyped)
    {
        possibleWords.Clear();

        foreach (var word in wordGenerator.cowboyFightWord)
        {
            var wordUpper = word.ToUpper();
            if (wordUpper.StartsWith(wordTyped.ToUpper()))
            {
                possibleWords.Add(wordUpper);
            }
        }

        possibleWordsTxt.text = possibleWords.Count > 0 ? string.Join(", ", possibleWords) : string.Empty;
    }


    void HasCompletedWord()
    {
        Debug.Log("HAS COMPLETED WORD IS CALLED!");
        for(int i = possibleWords.Count - 1; i >= 0; i--)
        {
            Debug.Log(i);
            if (possibleWords[i].ToUpper() == wordBeingTyped.ToUpper())
            {
                Debug.Log("WORD IS COMPLETED!");
                string matchedWord = possibleWords[i];
                wordBeingTyped = "";
                isTyping = false;
                wordBeingTypedTxt.text = "";
                targetWord = possibleWords[i];
                possibleWords.RemoveAt(i);
                index = 0;
                actionManager.ExecuteAction(matchedWord);
                wordsTyped.Add(wordBeingTyped.ToUpper());
            }
        }
    }

}