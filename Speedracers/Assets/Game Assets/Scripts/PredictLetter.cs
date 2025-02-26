using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PredictLetter : MonoBehaviour
{
    [Header("Top Bottom")]
    public WordManager wordManager;
    public TMP_Text upcLetterTxt;
    [Header("Top Left")]
    [SerializeField] List<GameObject> letters = new List<GameObject>();
    public GameObject bigLetter;
    public TMP_Text bigLetterTxt;
    [Header("Others")]

    public TMP_Text fpsText;
    public float[] framesArray;
    private int lastFrameIndex;
    private void Awake()
    {
        framesArray = new float[50];
        wordManager = FindAnyObjectByType<WordManager>();
    }

    private void Update()
    {
        framesArray[lastFrameIndex] = Time.deltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % framesArray.Length;
        fpsText.text = "FPS: " + calcFPS().ToString("F0");
        string firstLetter = wordManager.currentLetter;
        upcLetterTxt.text = firstLetter.ToUpper().ToString();

        foreach (var letter in letters)
        {
            if (letter.name.ToUpper().ToString() == upcLetterTxt.text)
            {
                //   Instantiate(bigLetter, letter.transform.position, letter.transform.rotation);
                bigLetter.transform.position = letter.transform.position;
                bigLetterTxt.text = upcLetterTxt.text.ToString();
            }
        }
    }

    float calcFPS()
    {
        float total = 0f;
        foreach(float deltaTime in  framesArray)
        {
            total += deltaTime;
        }

        return framesArray.Length / total;
    }
}
