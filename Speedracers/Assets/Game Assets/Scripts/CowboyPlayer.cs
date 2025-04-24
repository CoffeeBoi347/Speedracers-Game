using System.Collections;
using UnityEngine;
public class CowboyPlayer : MonoBehaviour
{
    [Header("Word Manager")]

    public CowboyFightWordManager cowboyWordManager;

    [Header("Speed Values")]

    public float attackBoost;
    public float walkSpeed;
    public float runSpeed;
    public float jumpVelocity;

    [Header("Component References")]

    public Animator anim;
    public Rigidbody2D rb;
    public CurrentAction action;
    public ParticleSystem groundBrstFX;

    [Header("Layers & Bools")]

    public Transform ground;
    public float groundRadius = 0.2f;
    public bool isGrounded;
    public bool canJump = true;
    public LayerMask groundLayer;
    public bool isAttacking = false;
    [Header("Storing Values")]

    private float scaleX;
    private float scaleY;

    private void Start()
    {
        action = CurrentAction.Idle;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
    }

    private void Update()
    {
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        cowboyWordManager = FindObjectOfType<CowboyFightWordManager>();

        isGrounded = Physics2D.OverlapCircle(ground.position, groundRadius, groundLayer);
        if (isGrounded && !canJump)
        {
            canJump = true;
            groundBrstFX?.Play();
            anim.SetBool("isJump", true);
            Debug.Log("Player has landed!");
        }

        if (canJump)
        {
            transform.localScale = new Vector2(scaleX, scaleY);
        }
    }

    public void RunPlayer()
    {
        isAttacking = false;
        action = CurrentAction.Run;
        Debug.Log("RUNNING!");
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            anim.SetBool("isRunning", true);
            string[] bools =
            {
                "isWalk", "isCombat", "isJump"
            };

            foreach (var b in bools)
            {
                anim.SetBool(b, false);
            }
    }

    public void FlipPlayer()
    {
        float scaleX = -transform.localScale.x;
        transform.localScale = new Vector2(scaleX, transform.localScale.y);
        runSpeed *= -1;
        walkSpeed *= -1;
        attackBoost *= -1;
    }

    public void JumpPlayer()
    {
        isAttacking = false;
        canJump = false;
        action = CurrentAction.Jump;
        anim.SetBool("isJump", true);
        rb.velocity = new Vector2(runSpeed, jumpVelocity);

        string[] bools =
        {
            "isWalk", "isCombat", "isRunning"
        };

        foreach (var b in bools)
        {
            anim.SetBool(b, false);
        }   
    }

    public void DiePlayer()
    {
        isAttacking = false;
        action = CurrentAction.Dead;
        anim.SetBool("isDead", true);
        string[] bools =
        {
            "isWalk", "isCombat", "isRunning", "isJump"
        };

        foreach (var b in bools)
        {
            anim.SetBool(b, false);
        }
    }

    public void WalkPlayer()
    {
        isAttacking = false;
        action = CurrentAction.Walk;
        Debug.Log("IS WALKING NOW!");
        anim.SetBool("isWalk", true);
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
    }

    public void PlayerCombat()
    {
        isAttacking = true;
        rb.velocity = new Vector2(attackBoost, 0f);
        anim.SetBool("isCombat", true);

        string[] bools =
        {
            "isWalk", "isRunning", "isJump"
        };

        foreach (var b in bools)
        {
            anim.SetBool(b, false);
        }

        StartCoroutine(GoBackToIdle(0.5f));                                              
    }

    public void StopPlayer()
    {
        isAttacking = false;
        Debug.Log("WE ARE AT STOP NOW!");
        string[] bools =
        {
            "isWalk", "isCombat", "isRunning", "isJump"
        };

        foreach(var b in bools)
        {
            anim.SetBool(b, false);
        }
    }

    public IEnumerator HasNotType(float time)
    {
        string oldWord = cowboyWordManager.wordBeingTyped;
        if (string.IsNullOrEmpty(oldWord)) yield break;
        yield return new WaitForSeconds(time);
        if(oldWord == cowboyWordManager.wordBeingTyped)
        {
            StopPlayer();
            cowboyWordManager.isTyping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = true;
            anim.SetBool("isJump", false);
        }

        if (collision.gameObject.CompareTag("Enemy") && action == CurrentAction.Attack)
        {
            cowboyWordManager.attackManager.TakeHit();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            canJump = false;
        }
    }

    private IEnumerator GoBackToIdle(float time)
    {
        yield return new WaitForSeconds(time);
        StopPlayer();
    }
}

public enum CurrentAction
{
    Idle,
    Walk,
    Run,
    Attack,
    Jump,
    Dead
}