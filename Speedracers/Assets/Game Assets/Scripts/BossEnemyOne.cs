using System.Collections;
using System.Linq;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    [Header("Base Enemy")]
    public EnemyState state;
    public GameObject enemyObj;
    public Animator anim;
    public Rigidbody2D rb;
    [Header("Enemy Velocity")]
    private Vector3 distance;
    public float detectionRange;
    public float enemyHealth;
    public float enemySpeed;
    public float attackRange;
    [Header("Others")]
    private bool isFlippingRight = true;
    public GameObject player;
    public ParticleSystem blastFX;
    public LayerMask playerLayer;
    void Start()
    {
        state = EnemyState.Idle;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (enemyHealth <= 0)
        {
            state = EnemyState.Death;
        }
        distance = (player.transform.position - transform.position);
        Debug.Log($"Distance: {distance.x}");
        switch (state)
        {
            case EnemyState.Idle:
                Debug.Log("I AM IN IDLE STATE");
                if (distance.x > detectionRange)
                    state = EnemyState.Walk;
                break;


            case EnemyState.Walk:
                Debug.Log("I AM IN WALKING STAGE");
                SetAnimation("isWalking");

                float moveDirection = Mathf.Sign(distance.x); 
                rb.velocity = new Vector2(moveDirection * enemySpeed, rb.velocity.y);

                if(moveDirection > 0 && !isFlippingRight)
                {
                    Flip();
                }

                if (Mathf.Abs(distance.x) < attackRange)
                {
                    state = EnemyState.Cleave;
                    rb.velocity = Vector2.zero;
                }
                break;

            case EnemyState.Cleave:
                Debug.Log("I AM IN ATTACKING STAGE");
                SetAnimation("isAttack");
                StopAnimation("isWalking");
                StartCoroutine(Attack());
                break;

            case EnemyState.TakeHit:
                StartCoroutine(HitReaction());
                break;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        enemyHealth -= damageAmount;
    }

    private IEnumerator HitReaction()
    {
        SetAnimation("isTakeHit");
        StopAnimation("isAttack");
        StopAnimation("isWalking");

        yield return new WaitForSeconds(0.6f); 

        StopAnimation("isTakeHit");
        state = EnemyState.Idle;
    }


    private IEnumerator Attack()
    {
        SetAnimation("isAttack");
        StopAnimation("isWalking");
        StopAnimation("isTakeHit");

        Collider[] hitPoints = (Physics.OverlapSphere(transform.position, 25f, playerLayer));

        if(hitPoints.Length > 0)
        {
            Debug.Log("Player DETECTED!");
            blastFX.Play();
        }

        yield return new WaitForSeconds(0.6f);
        if(distance.x < attackRange)
        {
            StopAnimation("isAttack");
            state = EnemyState.Idle;
        }
    }

    void SetAnimation(string boolName)
    {
        anim.SetBool(boolName, true);
    }

    void StopAnimation(string boolName)
    {
        anim.SetBool(boolName, false);
    }

    void Flip()
    {
        isFlippingRight = !isFlippingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}

public enum EnemyState
{
    Idle,
    Walk,
    Cleave,
    TakeHit,
    Death
}

