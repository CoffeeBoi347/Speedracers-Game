using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneMovementPlayer : MonoBehaviour
{
    public int sceneIndex;
    public WordManager wordManager;
    public float JumpPower;
    public Rigidbody2D rb;
    public ParticleSystem SmokeFX;
    public float speed;
    public bool AllowedToJump = true;
    public AudioSource SmokeFXAudio;
    public bool HasCollided = false;
    public bool CollidedWithPipe = false;
    public bool CollidedBoundary;
    public CoinScript coinSc;
    public ParticleSystem blastStart;
    public bool CollidedWithPPowerFX = false;
    public ParticleSystem Explosion;
    void Start()
    {
        coinSc = FindObjectOfType<CoinScript>();
        blastStart.Stop();
        CollidedBoundary = false;
        CollidedWithPipe = false;
        HasCollided = false;
        SmokeFXAudio.Stop();
        SmokeFX.Stop();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        wordManager = FindObjectOfType<WordManager>(); // to access the components of the word manager script. 
        if (AllowedToJump)
        {
            if (wordManager.CanJump == true)
            {
                rb.velocity = new Vector2(speed, JumpPower);
                SmokeFX.Play();
                SmokeFXAudio.Play();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        wordManager.AllowToAdd = false;
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Building" || collision.gameObject.tag == "Road")
        {
            Explosion.gameObject.SetActive(true);
            Explosion.Play();
            HasCollided = true;
            AllowedToJump = false;
            StartCoroutine(ReloadScene(2f));

        }

        if(collision.gameObject.tag == "Pipe")
        {
            CollidedWithPipe = true;
        }

        if(collision.gameObject.tag == "Boundary")
        {
            CollidedBoundary = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Start")
        {
            blastStart.Play();
        }

        if(collision.gameObject.tag == "Coin")
        {
            coinSc.coins += 1;
        }
        if(collision.gameObject.tag == "PowerUpBoost")
        {
            Debug.Log("Nice");
            rb.AddForce(new Vector2(speed * 35f, speed * 35f));
            CollidedWithPPowerFX = true;
        }
    }

    private IEnumerator ReloadScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(2);
    }
}
