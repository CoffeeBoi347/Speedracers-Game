using System.Collections;
using UnityEngine;
public class CowboyPlayer : MonoBehaviour
{
    [Header("Word Manager")]

    public CowboyFightWordManager cowboyWordManager;

    [Header("Speed Values")]

    public float walkSpeed;
    public float runSpeed;
    public float jumpVelocity;

    [Header("Component References")]

    public Animator anim;
    public Rigidbody2D rb;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cowboyWordManager = FindObjectOfType<CowboyFightWordManager>();
    }

    public void RunPlayer()
    {
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
    }

    public void JumpPlayer()
    {
        rb.velocity = new Vector2(runSpeed, jumpVelocity);
        anim.SetBool("isJump", true);

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
        Debug.Log("IS WALKING NOW!");
        anim.SetBool("isWalk", true);
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
    }

    public void PlayerCombat()
    {
        anim.SetBool("isCombat", true);
    }

    public void StopPlayer()
    {
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
}