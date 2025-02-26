using System.Linq;
using TMPro;
using UnityEngine;

public class WordManagerBossFight : MonoBehaviour
{
    public BossFightWordGenerator fightWordGenerator;
    public TMP_Text wordToDisplay;
    public TMP_Text charactersTyped;

    public string targettedWord = "";
    public string currentTargetWord = "";
    public string wordBeingTyped = "";

    public int lettersTyped = 0;
    public int wordsTyped = 0;
    public long WPM;
    public int ratingSystem;

    public bool allowToAdd = false;
    public AudioSource typingSFX;
    private void Start()
    {
        typingSFX.Stop();
    }

    private void Update()
    {
        InputControls();
    }

    public void InputControls()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                lettersTyped += 1;
                wordBeingTyped += keyCode.ToString().ToLower();

                if (targettedWord == "" && wordBeingTyped.Length >= 3)
                {
                    targettedWord = FindMatchingWord(wordBeingTyped);
                }
            }
        }

    }

    string FindMatchingWord(string input)
    {
        return fightWordGenerator.words.FirstOrDefault(word => word.StartsWith(input));
    }
}