using System.Collections;
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
    public bool endCollide = false;
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
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Building" || collision.gameObject.tag == "Road")
        {
            wordManager.AllowToAdd = false;
            Explosion.gameObject.SetActive(true);
            Explosion.Play();
            HasCollided = true;
            AllowedToJump = false;
            StartCoroutine(ReloadScene(2f));

        }

        if(collision.gameObject.tag == "BossFight")
        {
            GameObject wordManagerObj = GameObject.FindObjectOfType<WordManager>()?.gameObject;
            wordManagerObj.SetActive(false);

            GameObject bossWordManagerObj = GameObject.FindObjectOfType<BossFightWordGenerator>()?.gameObject;
            bossWordManagerObj.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Pipe"))
        {
                wordManager.AllowToAdd = false;

                CollidedWithPipe = true;
        }

        if(collision.gameObject.CompareTag("Boundary"))
        {
            wordManager.AllowToAdd = false;

            CollidedBoundary = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Start"))
        {
            blastStart.Play();
        }

        if(collision.gameObject.CompareTag("Coin"))
        {
            coinSc.coins += 1;
        }
        if(collision.gameObject.CompareTag("PowerUpBoost"))
        {
            Debug.Log("Nice");
            rb.AddForce(new Vector2(speed * 35f, speed * 35f));
            CollidedWithPPowerFX = true;
        }
        if(collision.gameObject.CompareTag("EndPoint"))
        {
            endCollide = true;
        }
    }

    private IEnumerator ReloadScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneIndex);
    }
}
