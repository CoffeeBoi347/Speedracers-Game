using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public Transform playerObj;
    public BossWordManager wordManager;
    public HealthManagerBattle playerHealthManager;
    public HealthManagerBoss healthManagerBoss;
    public BossState enemyState;
    public Animator anim;
    public float health = 450f;
    public float velocity;
    public float attackRange = 3f;
    public float attackCooldown;
    public float teleportCooldown;
    public bool phase2 = false;
    public bool phase3 = false;
    public float lastAttackTime;
    public float lastTeleportTime;
    public float knockbackForce;
    public bool takeDamage = false;
    public ParticleSystem particlePrefab;
    public bool isAttacking = false;
    public float damagePlayer = 12f;
    public bool damagedToPlayer = false;
    private void Start()
    {
        healthManagerBoss = GetComponent<HealthManagerBoss>();  
        wordManager = FindObjectOfType<BossWordManager>();
        playerHealthManager = FindObjectOfType<HealthManagerBattle>();
    }
    private void Update()
    {
        if (enemyState == BossState.Death || enemyState == BossState.Teleport) return;
        float horizontalDistance = Mathf.Abs(transform.position.x - playerObj.position.x);
        float verticalDistance = playerObj.position.y - transform.position.y;

        if(health <= 370f && !phase2)
        {
            phase2 = true;
            attackCooldown *= 0.6f;
            teleportCooldown *= 0.7f;
        }

        if(health <= 200f && phase2 && !phase3)
        {
            phase3 = true;
            phase2 = false;
            attackCooldown *= 0.6f;
            teleportCooldown *= 0.85f;
        }

        if(takeDamage && health > 0f)
        {
            ChangeState(BossState.TakeHit);
            Debug.Log("Taking damage..!");
        }

        if (Random.value >= 0.7f && Time.time - lastTeleportTime > teleportCooldown)
        {
            ChangeState(BossState.Teleport);
            Debug.Log("Teleporting..");
        }
        else if(Random.value < 0.6f && (horizontalDistance < attackRange && verticalDistance < 2.5f) && Time.time - lastAttackTime > attackCooldown)
        {
            ChangeState(BossState.Attack);
            Debug.Log("Attacking");
        }
        else if(Random.value < 0.5f && horizontalDistance > attackRange)
        {
            ChangeState(BossState.Walk);
            Debug.Log("Walking");
        }
        else if(horizontalDistance <= 1f && horizontalDistance <= attackRange)
        {
            ChangeState(BossState.Attack);
        }
        else if(wordManager.wordBeingTyped == "BULLET" && Random.value > 0.6f)
        {
            ChangeState(BossState.Teleport);
        }
        
        Flip();

    }
    public void ChangeState(BossState newState)
    {
        if (enemyState == newState) return;

        enemyState = newState;
        StopAllCoroutines();

        switch (newState)
        {
            case BossState.Idle:
                isAttacking = false;
                anim.SetBool("isIdle", true);
                break;
            case BossState.Walk:
                StartCoroutine(WalkToPlayer());
                break;
            case BossState.Attack:
                ApplyImpactDamage();
                break;
            case BossState.TakeHit:
                TakeDamage(6f);
                break;
            case BossState.Teleport:
                StartCoroutine(Teleport());
                break;
            case BossState.Death:
                anim.SetBool("isDeath", true);
                break;
        }
    }

    public void ApplyImpactDamage()
    {
        isAttacking = true;
        SetAttackToTrue();
        if (anim == null)
        {
            Debug.LogError("Animator is NULL!");
            return;
        }

        anim.SetBool("isAttack", true);
        anim.SetBool("isWalking", false);

        if (playerObj == null)
        {
            Debug.LogError("playerObj is NULL!");
            return;
        }

        float horizontalDistance = Mathf.Abs(transform.position.x - playerObj.position.x);
        float verticalDistance = playerObj.position.y - transform.position.y;

        if (horizontalDistance <= attackRange && verticalDistance < 2.25f)
        {
            Rigidbody2D playerRb = playerObj.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Debug.Log("APPLY DAMAGE TO PLAYER!");
                playerRb.velocity = new Vector2(playerRb.velocity.x, knockbackForce);
                playerRb.AddTorque(knockbackForce);
                damagedToPlayer = true;
                particlePrefab.Play();
                playerHealthManager.health -= damagePlayer;
                StartCoroutine(SetDamageToPlayerFalse(0.01f));
                if (particlePrefab != null)
                {
                    particlePrefab.Play();
                    StartCoroutine(SetParticlePrefabPlayFalse(1f));
                }
                else
                {
                    Debug.LogError("Particle System is NULL!");
                }
            }
            else
            {
                Debug.LogError("Player Rigidbody is NULL!");
            }
        }
    }



    public void TakeDamage(float damage)
    {
        isAttacking = false;
        if (enemyState == BossState.Death) return;
        takeDamage = true;
        anim.SetBool("isTakeHit", true);
        if(takeDamage)
        {
            health -= damage;
            takeDamage = false;
        }
        Debug.Log(health);
        if (health <= 0)
        {
            ChangeState(BossState.Death);
        }
        else
        {
            ChangeState(BossState.TakeHit);
            Invoke(nameof(RecoverFromHit), 0.5f);
        }
    }

    void RecoverFromHit()
    {
        isAttacking = false;
        ChangeState(BossState.Idle);
        anim.SetBool("isIdle", true);
        anim.SetBool("isTakeHit", false);
    }

    void Flip()
    {
        float direction = playerObj.position.x - transform.position.x;

        if ((direction > 0 && transform.localScale.x > 0) || (direction < 0 && transform.localScale.x < 0))
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }


    IEnumerator WalkToPlayer()
    {
        isAttacking = false;
        anim.SetBool("isWalking", true);
        anim.SetBool("isTeleport", false);
        float bossY = transform.position.y;
        while(Mathf.Abs(transform.position.x - playerObj.position.x) > attackRange)
        {
            Vector2 targetPosition = new Vector2(playerObj.transform.position.x, bossY);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, velocity * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Teleport()
    {
        isAttacking = false;
        lastTeleportTime = Time.time;
        anim.SetBool("isWalking", false);
        anim.SetBool("isTeleport", true);
        yield return new WaitForSeconds(0.4f);
        Vector2 newPosition = new Vector2(Random.Range(playerObj.position.x - 5f, playerObj.position.x + 5f), transform.position.y);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.position = newPosition;
        Debug.Log($"New Position: {newPosition}");
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<Rigidbody2D>().simulated = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        anim.SetBool("isTeleport", false);
        ChangeState(BossState.Idle); 

    }

    IEnumerator SetParticlePrefabPlayFalse(float time)
    {
        yield return new WaitForSeconds(time);
        particlePrefab.Stop();
    }

    IEnumerator SetDamageToPlayerFalse(float time)
    {
        yield return new WaitForSeconds(time);
        damagedToPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ChangeState(BossState.TakeHit);
        }
    }

    public bool SetAttackToTrue()
    {
        return isAttacking = true;
    }
}

public enum BossState 
{ 
    Idle,
    Walk,
    Attack,
    TakeHit,
    Teleport,
    Death
}