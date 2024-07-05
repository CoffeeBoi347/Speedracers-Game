using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovementPlayer : MonoBehaviour
{
    public WordManager wordManager;
    public float JumpPower;
    public Rigidbody2D rb;
    public ParticleSystem SmokeFX;
    private bool AllowedToJump = true;
    void Start()
    {
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
                rb.velocity = new Vector2(rb.velocity.x, JumpPower);
                SmokeFX.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pipe")
        {
            AllowedToJump = false;
        }
    }
}
