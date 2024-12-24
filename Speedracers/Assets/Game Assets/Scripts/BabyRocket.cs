using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BabyRocket : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Rigidbody2D rb;
    public Transform shootPoint;
    public GameObject bullet;
    public float rotateSpeed;
    private float timer;
    public ParticleSystem burstFX;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        speed += 0.15f;
        if (speed >= 10f)
            speed = 10f;
        Vector2 direction = target.transform.position - transform.position;

        float rotationAmount = Vector3.Cross(direction, transform.up).z; // cross product 
        rb.velocity = transform.up * speed;
        rb.angularVelocity = -rotationAmount * rotateSpeed; // 

        timer += 0.01f;
        if (timer >= 0.2f)
            Instantiate(bullet, shootPoint.transform.position, Quaternion.identity);
            timer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            burstFX.gameObject.SetActive(true);
            burstFX.Play();
        }
    }

}
