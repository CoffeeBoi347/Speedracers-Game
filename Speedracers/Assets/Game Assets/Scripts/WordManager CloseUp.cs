using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordManagerCloseUp : MonoBehaviour
{
    [Header("Word Generator")]

    public WordGenerator wordGenerator;

    [Header("Attack Manager")]

    public AttackManager attackManager;

    [Header("Player Script")]

    public CloseUpShootPlayer closeUpShootPlayer;
    public List<GameObject> keys;

    [Header("Strings")]

    public string wordBeingTyped = "";
    public string targetWord;
    public string wordGenerated;
    public string letterTyped;

    [Header("Text References")]

    public TMP_Text wordBeingTypedText;
    public TMP_Text targetWordTxt;
    public TMP_Text bulletsShot;
    public GameObject textToTypeObj;

    [Header("Booleans")]

    public bool hasGameBegun;
    public bool correctWord;
    public bool wordCompleted = false;
    public bool canShoot;
    public bool isTyping = false;

    [Header("Hands")]

    public GameObject leftHand;
    public GameObject rightHand;
    private GameObject currentKeyPressed;

    [Header("Count")]

    public int lettersTyped;
    public int wordsTyped;
    private void Start()
    {
        canShoot = true;
        wordGenerator = FindObjectOfType<WordGenerator>();
        attackManager = FindObjectOfType<AttackManager>();
        closeUpShootPlayer = FindObjectOfType<CloseUpShootPlayer>();
        wordGenerated = wordGenerator.GetRandomWord();
        targetWord = wordGenerated;
        textToTypeObj.SetActive(false);
    }

    private void Update()
    {
        targetWordTxt.text = targetWord;

        if(lettersTyped % 30 == 0 && lettersTyped != 0)
        {
            canShoot = false;
            Debug.Log("RELOAD!");
            closeUpShootPlayer.PlayReloadAnimation();
            closeUpShootPlayer.StartCoroutine(closeUpShootPlayer.Reloading());
        }

        if (canShoot)
        {
            foreach (char letter in Input.inputString)
            {
                if (char.IsLetter(letter))
                {
                    isTyping = true;
                    letterTyped = letter.ToString().ToUpper();
                    wordBeingTyped += letterTyped;
                    wordBeingTypedText.text = wordBeingTyped;
                    hasGameBegun = true;
                    CheckForCorrectWord(wordBeingTyped);
                }
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (wordBeingTyped.Length > 0)
                {
                    wordBeingTyped = wordBeingTyped.Substring(0, wordBeingTyped.Length - 1);
                    wordBeingTypedText.text = wordBeingTyped;
                }
            }

            foreach (var key in keys)
            {
                if (key.name.ToString().ToUpper() == letterTyped.ToUpper())
                {
                    currentKeyPressed = key;
                }
            }

            float distanceA = Vector3.Distance(leftHand.transform.position, currentKeyPressed.transform.position);
            float distanceB = Vector3.Distance(rightHand.transform.position, currentKeyPressed.transform.position);

            if (distanceA < distanceB)
            {
                leftHand.transform.position = new Vector2(currentKeyPressed.transform.position.x, currentKeyPressed.transform.position.y - 80f);
            }

            else if (distanceB < distanceA)
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
        }
    }

    void CheckForCorrectWord(string wordTyped)
    {
        foreach(string word in wordGenerator.words)
        {
            if (word.Length >= wordTyped.Length && word.Substring(0, wordTyped.Length).ToUpper() == wordTyped)
            {
                lettersTyped += 1;
                bulletsShot.text = $"{lettersTyped} / 75";
                Debug.Log("CORRECT WORD!");
                correctWord = true;
                CheckCompleteWord(wordTyped);
                RemoveLastWord(wordTyped, targetWord);
                closeUpShootPlayer.PlayShootAnimation();
                attackManager.TakeHit();
                break;
            }
        }
    }

    public void RemoveLastWord(string wordTyped, string targetWord)
    {
        foreach (string word in wordGenerator.words)
        {
            if (word.Length >= wordTyped.Length + 1 && wordTyped == word.Substring(0, wordTyped.Length + 1))
            {
                if (targetWord.Length >= wordTyped.Length)
                {
                    targetWord = targetWord.Substring(wordTyped.Length, targetWord.Length - wordTyped.Length);
                    targetWordTxt.text = targetWord;
                }

            }
        }
    }

    void CheckCompleteWord(string wordTyped)
    {
        foreach (string word in wordGenerator.words)
        {
            if(wordTyped == word.ToUpper())
            {
                wordsTyped += 1;
                wordCompleted = true;
                StartCoroutine(SetWordCompletedToFalse(0.1f));
                ResetEverything();
            }
        }
    }

    void ResetEverything()
    {
        isTyping = false;
        wordBeingTyped = "";
        wordBeingTypedText.text = wordBeingTyped;
        targetWord = wordGenerator.GetRandomWord();
        targetWordTxt.text = targetWord;
    }

    private IEnumerator SetWordCompletedToFalse(float time)
    {
        yield return new WaitForSeconds(time);
        wordCompleted = false;
    }
}