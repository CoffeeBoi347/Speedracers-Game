using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour
{
    public float rotationSpeed;
    private int hit = 0;
    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            hit++;
            Destroy(collision.gameObject);

            if(hit >= 5)
            {
                Destroy(gameObject);
            }
        }
    }
}
