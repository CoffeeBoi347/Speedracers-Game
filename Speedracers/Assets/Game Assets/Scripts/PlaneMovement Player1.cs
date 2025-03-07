using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaneMovementPlayer1 : MonoBehaviour
{
    public int sceneIndex;
    public BossWordManager wordManager;
    public float JumpPower;
    public Rigidbody2D rb;
    public ParticleSystem SmokeFX;
    public float speed;
    public bool AllowedToJump = true;
    public AudioSource SmokeFXAudio;
    public PlayerShoot playerShoot;
    public bool HasCollided = false;
    public bool CollidedWithPipe = false;
    public bool CollidedBoundary;
    public CoinScript coinSc;
    public ParticleSystem blastStart;
    public bool CollidedWithPPowerFX = false;
    public ParticleSystem Explosion;
    public bool endCollide = false;
    public ParticleSystem boomFx;
    void Start()
    {
        boomFx.Stop();
        coinSc = FindObjectOfType<CoinScript>();
        blastStart.Stop();
        CollidedBoundary = false;
        CollidedWithPipe = false;
        HasCollided = false;
        SmokeFXAudio.Stop();
        SmokeFX.Stop();
        playerShoot = FindObjectOfType<PlayerShoot>();
        playerShoot = GetComponent<PlayerShoot>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        wordManager = FindObjectOfType<BossWordManager>(); // to access the components of the word manager script. 
        if (AllowedToJump)
        {
            if (wordManager.canJump == true)
            {
                rb.velocity = new Vector2(speed, JumpPower);
                SmokeFX.Play();
                SmokeFXAudio.Play();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pipe" || collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Building" || collision.gameObject.tag == "Road")
        {
            wordManager.allowToAdd = false;
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
                wordManager.allowToAdd = false;

                CollidedWithPipe = true;
        }

        if(collision.gameObject.CompareTag("Boundary"))
        {
            wordManager.allowToAdd = false;

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

    public void DodgePlayer()
    {
        rb.MovePosition(new Vector2(transform.position.x,transform.position.y + 2f));
        boomFx.Play();
        Debug.Log("Dodge UP!");
    }


    public void FreezePlayer()
    {
        BossWordManager wordManager = new BossWordManager();
        wordManager.canJump = false;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void UnfreezePlayer()
    {
        BossWordManager wordManager = new BossWordManager();
        wordManager.canJump = true;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    public void StrikePlayer()
    {
        Debug.Log("Strike");
    }

    public void FlipPlayer()
    {
        float scaleX = transform.localScale.x;
        transform.localScale = new Vector2(-scaleX, transform.localScale.y);
        speed = -speed;
    }

    public void MovePlayer()
    {
        BossWordManager wordManager = new BossWordManager();
        wordManager.canJump = true;
        rb.gravityScale = 1;
    }

    public void BulletPlayer()
    {
        playerShoot.allowToShoot = true;
    }

    public void FirePlayer()
    {
        Debug.Log("Fire");
    }

    public void RushPlayer()
    {
        rb.MovePosition(new Vector2(transform.position.x + 15f, transform.position.y));
        Debug.Log("Rushing Forward!");
        boomFx.Play();
        playerShoot.allowToShoot = false;

    }

    public void SlowPlayer()
    {
        speed -= 1;
    }

    private IEnumerator ReloadScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneIndex);
    }
}
