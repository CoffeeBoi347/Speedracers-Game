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
    public GameObject damagedPlayerPrefab;
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
    public GameObject bulletObj;
    public HingeJoint2D hingeJoint;
    public LineRenderer lineRenderer;
    public GameObject[] allHinges;
    public GameObject closestHinge;
    public bool damagedByBoss = false;
    float closestDistance = Mathf.Infinity;
    public ParticleSystem impactFX;
    void Start()
    {
        boomFx.Stop();
        coinSc = FindObjectOfType<CoinScript>();
        blastStart.Stop();
        CollidedBoundary = false;
        CollidedWithPipe = false;
        HasCollided = false;
        hingeJoint.enabled = false;
        SmokeFXAudio.Stop();
        SmokeFX.Stop();
        playerShoot = FindObjectOfType<PlayerShoot>();
        playerShoot = GetComponent<PlayerShoot>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(damagedPlayerPrefab != null)
        {
            damagedPlayerPrefab.transform.position = transform.position;
        }
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
        
        closestDistance = Mathf.Infinity;
        closestHinge = null;

       foreach(GameObject hinge in allHinges)
        {
            float distance = Vector2.Distance(transform.position, hinge.transform.position);
            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestHinge = hinge;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        impactFX.Play();
            Debug.Log("Collision detected with: " + collision.gameObject.name);
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

        if (collision.gameObject.CompareTag("BossEnemyOne"))
        {
            damagedByBoss = true;
            Instantiate(damagedPlayerPrefab, transform.position, transform.rotation);
            StartCoroutine(SetBossAttackToFalse(0.02f));
            StartCoroutine(RemoveDamagedPlayerPrefab(0.05f));
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

        bulletObj.transform.localScale = new Vector2(-bulletObj.transform.localScale.x, bulletObj.transform.localScale.y);
        var findBulletShoot = bulletObj.GetComponent<BulletShoot>();
        if(speed > 0)
        {
            findBulletShoot.velocity = findBulletShoot.velocity * -1;
        }
        else if(speed < 0)
        {
            findBulletShoot.velocity = findBulletShoot.velocity * 1;
        }
    }

    public void MovePlayer()
    {
        
        BossWordManager wordManager = new BossWordManager();
        wordManager.canJump = true;
        rb.gravityScale = 1;
        transform.localEulerAngles = new Vector3(0f, 0f, 0f);
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
        Vector3 finalPos = new Vector2(transform.position.x + 20f, transform.position.y);
        transform.position = finalPos;
        Debug.Log("Rushing Forward!");
        boomFx.Play();
        playerShoot.allowToShoot = false;

    }

    public void GrapplePlayer()
    {
        Debug.Log("Grappling");

        if (closestHinge != null)
        {
            Debug.Log("THERE IS CLOSEST HINGE NEAR!");

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, closestHinge.transform.position);
            lineRenderer.enabled = true;
            hingeJoint.enabled = true;
            hingeJoint.connectedBody = null; 
            hingeJoint.connectedAnchor = closestHinge.transform.position;
            float distance = Vector2.Distance(closestHinge.transform.position ,transform.position);
            float timeTaken = (distance / speed);
            
            Debug.Log("Setting grapple hinge!");
            StartCoroutine(pullLineTowardsEnd(transform.position, closestHinge.transform.position, timeTaken));
            StartCoroutine(PullPlayerToHinge(transform.position, closestHinge.transform.position, timeTaken));

        }
        else
        {
            Debug.Log("CLOSEST HINGE NOT THERE!");
        }
    }

    private IEnumerator pullLineTowardsEnd(Vector3 startPos, Vector3 hingePos, float time)
    {
        yield return new WaitForSeconds(time);
        float elapsedTime = 0f;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            Vector3 endPos = Vector3.Lerp(startPos, hingePos, (elapsedTime / time) * 3f);
            lineRenderer.SetPosition(1, endPos);
            yield return null;
        }
    }

    private IEnumerator PullPlayerToHinge(Vector3 startPos, Vector3 hingePos, float time)
    {
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newPos = Vector3.Lerp(startPos, hingePos, (elapsedTime / time) * 3f);
            transform.position = newPos; 
            lineRenderer.SetPosition(0, newPos);
            yield return null;
        }

        lineRenderer.enabled = false;
        hingeJoint.enabled = false;
    }

    private IEnumerator RemoveDamagedPlayerPrefab(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(damagedPlayerPrefab);
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

    private IEnumerator SetBossAttackToFalse(float time)
    {
        yield return new WaitForSeconds(time);
        damagedByBoss = false;
    }
}
