using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithUIText : MonoBehaviour
{
    public GameObject UIButton;
    public GameObject PlayButton;
    public GameObject MultiplayerButton;
    public GameObject CreditsButton;
    public GameObject TutorialButton;
    public Transform startPos;
    public Transform endPos;
    public float speed;
    public ParticleSystem Explosion;
    public AudioSource explodeSFX;

    private float timer = 0f;  // Timer variable

    void Start()
    {
        Explosion.Stop();
        PlayButton.SetActive(false);
        MultiplayerButton.SetActive(false);
        CreditsButton.SetActive(false);
        TutorialButton.SetActive(false);
    }

    void Update()
    {
        // Increment the timer by Time.deltaTime each frame
        timer += Time.deltaTime;

        // Start the text animation after 2 seconds
        if (timer >= 2f)
        {
            TextAnimation();
        }
    }

    void TextAnimation()
    {
        // Calculate the distance between the current position and the end position
        float Distance = Vector3.Distance(transform.position, endPos.position);

        // Move the object towards the end position
        transform.position = Vector3.MoveTowards(transform.position, endPos.position, speed * Time.deltaTime);

        // Check if the object is close enough to the end position
        if (Distance <= 0.1f)
        {
            UIButton.SetActive(false);
            Explosion.Play();
            PlayButton.SetActive(true);
            MultiplayerButton.SetActive(true);
            TutorialButton.SetActive(true);
            CreditsButton.SetActive(true);
        }
    }
}
