using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovementPlayer : MonoBehaviour
{
    public WordManager wordManager;
    public float JumpPower;
    public Rigidbody2D rb;
    public ParticleSystem SmokeFX;
    public bool AllowedToJump = true;
    public AudioSource SmokeFXAudio;
    public bool HasCollided = false;
    public bool CollidedWithPipe = false;
    public bool CollidedBoundary;
    void Start()
    {
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
                rb.velocity = new Vector2(rb.velocity.x, JumpPower);
                SmokeFX.Play();
                SmokeFXAudio.Play();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        wordManager.AllowToAdd = false;
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Pipe")
        {
            HasCollided = true;
            AllowedToJump = false;
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
}
