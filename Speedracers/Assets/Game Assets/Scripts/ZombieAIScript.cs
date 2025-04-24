using System.Collections;
using UnityEngine;

public class ZombieAIScript : MonoBehaviour
{
    public GameObject target;

    [Header("Prediction Values")]

    public float currentMoveSpeed;
    public float attackThreshold;
    public float velocity;
    public float velocityRun;
    public float detectionRadius;
    public float count;

    public float attackTimer;

    [Header("Other Components")]

    public Animator zombieAnim;
    public ZombieState enemyState;
    public Rigidbody2D enemyRB;
    public Vector2 distanceFromPlayer;

    [Header("Booleans")]

    public bool isWalking;
    public bool isRunning;
    public bool isAttacking;
    public bool hasDamagedPlayer;
    public bool isFlipped = false;
    public bool playerDetected;
    public bool canAttack;

    [Header("Phases")]

    public bool phase1;
    public bool phase2 = false;
    public bool phase3 = false;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        zombieAnim = GetComponent<Animator>();
        enemyState = ZombieState.Idle;
    }

    private void Update()
    {
        if (phase2)
        {
            attackThreshold *= 0.6f;
            velocityRun *= 1.1f;
            velocity *= 1.1f;
        }
        FlipPlayer();
        attackTimer += Time.deltaTime;
        distanceFromPlayer = transform.position - target.transform.position;
        playerDetected = HasDetectedPlayer();
        canAttack = AllowedToAttack();
        
        if(!canAttack)
            count += Time.deltaTime;

        if (!playerDetected && !canAttack && !isRunning && !isWalking && !isAttacking)
        {
            Debug.Log("WALKING NOW!");
            isWalking = true;
            isRunning = false;
            isAttacking = false;
            WalkPlayer();
        }

        if (playerDetected && !canAttack && !isRunning && isWalking && !isAttacking)
        {
            isRunning = true;
            isWalking = false;
            isAttacking = false;
            RunPlayer();
        }

        if (count >= attackThreshold && canAttack && isRunning && !isAttacking)
        {
            if(attackTimer >= attackThreshold)
            {
                attackTimer = 0f;
                isRunning = false;
                isWalking = false;
                isAttacking = true;
                AttackPlayer();
            }
        }

        if(count < attackThreshold && canAttack && isRunning && !isAttacking)
        {
            enemyState = ZombieState.Walk;
            RunPlayer();
        }

        if (isAttacking && !isRunning && !isWalking)
        {
            Debug.Log("DONT RUN ANYMORE!");
            count += Time.deltaTime;
            if(count >= 1.5f)
            {
                count = 0f;
                float value = Random.Range(0f, 1f);
                if(value < 0.5f)
                {
                    enemyState = ZombieState.Walk;
                    WalkPlayer();
                }
                else
                {
                    enemyState = ZombieState.Walk;
                    RunPlayer();
                }
            }
            isAttacking = false;
        }

    }

    private void FixedUpdate()
    {
        enemyRB.velocity = new Vector2(currentMoveSpeed, 0f);
    }

    public bool AllowedToAttack()
    {
        if(distanceFromPlayer.x < attackThreshold && distanceFromPlayer.x < detectionRadius)
        {
            Debug.Log("CAN ATTACK!");
            return true;
        }

        else 
        {
            Debug.Log("CANT ATTACK!");
            return false;
        }

    }

    public bool HasDetectedPlayer()
    {
        if(distanceFromPlayer.x < detectionRadius)
        {
            Debug.Log("DETECTED THE PLAYER!");
            return true;
        }

        if(distanceFromPlayer.x > detectionRadius)
        {
            return false;
        }

        else
        {
            return false;
        }
    }

    public void FlipPlayer()
    {
        if(distanceFromPlayer.x < 0 && !isFlipped)
        {
            ChangeBoolsToNegative();
            isFlipped = true;
        }

        if (distanceFromPlayer.x > 0 && isFlipped)
        {
            ChangeBoolsToNegative();
            isFlipped = false;
        }
    }

    public void WalkPlayer()
    {
        zombieAnim.SetBool("isWalk", true);
        enemyState = ZombieState.Walk;
        currentMoveSpeed = velocity;
    }

    public void RunPlayer()
    {
        zombieAnim.SetBool("isWalk", true);
        enemyState = ZombieState.Walk;
        currentMoveSpeed = velocityRun;
    }

    public void AttackPlayer()
    {
        zombieAnim.SetBool("isWalk", false);
        zombieAnim.SetBool("isAttack", true);
        enemyState = ZombieState.Attack;
        enemyRB.velocity = Vector2.zero;
        count = 0;
        StartCoroutine(GoBackToIdle(0.75f));
        
    }

    public void ChangeBoolsToNegative()
    {
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        currentMoveSpeed *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("COLLIDED WITH PLAYER!");
        }
    }

    private IEnumerator GoBackToIdle(float time)
    {
        Debug.Log("GOING BACK TO IDLE....!");
        yield return new WaitForSeconds(time);
        enemyState = ZombieState.Idle;
        zombieAnim.SetBool("isAttack", false);
        currentMoveSpeed = 0f;
    }
}

public enum ZombieState
{
    Idle,
    Hurt,
    Dead,
    Attack,
    Walk
}