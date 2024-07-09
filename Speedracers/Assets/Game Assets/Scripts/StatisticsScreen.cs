using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsScreen : MonoBehaviour
{
    public Transform NewPosition;
    public PlaneMovementPlayer player;
    public float speed;
    public WordManager wordManager;
    [Header("TextValues")]

    public TextMeshProUGUI jumpText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI WPMtext;
    public TextMeshProUGUI TimeSurvivedText;
    public TextMeshProUGUI RatingText;
    void Start()
    {
        player = FindObjectOfType<PlaneMovementPlayer>();
        wordManager = FindObjectOfType<WordManager>();

        jumpText.text = wordManager.JumpsCount.ToString();
        scoreText.text = wordManager.charactersTyped.ToString();
        TimeSurvivedText.text = wordManager.TimeSurvived.ToString();
        WPMtext.text = wordManager.WPM.ToString();
        RatingText.text = wordManager.Rating.ToString();
    }

    private void FixedUpdate()
    {
        UpdateValues();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.HasCollided == true && player.AllowedToJump == false || player.CollidedWithPipe == true || player.CollidedBoundary == true)
        {
            float Distance = Vector3.Distance(transform.position, NewPosition.transform.position);

            if(Distance >= 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, NewPosition.transform.position, speed);
            }
        }
    }

    void UpdateValues()
    {
        jumpText.text = wordManager.JumpsCount.ToString();
        scoreText.text = wordManager.charactersTyped.ToString();
        TimeSurvivedText.text = wordManager.TimeSurvived.ToString();
        WPMtext.text = wordManager.WPM.ToString();
        RatingText.text = wordManager.Rating.ToString();
    }
}
