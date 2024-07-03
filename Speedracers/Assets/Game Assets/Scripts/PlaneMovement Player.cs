using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovementPlayer : MonoBehaviour
{
    public WordManager wordManager;
    public float JumpPower;
    public Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        wordManager = FindObjectOfType<WordManager>();
        if(wordManager.CanJump == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
        }
    }
}
