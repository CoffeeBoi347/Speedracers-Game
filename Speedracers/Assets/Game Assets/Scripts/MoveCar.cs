using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCar : MonoBehaviour
{
    public bool SpawnNewObject;
    public float speed;

    private void Start()
    {
        SpawnNewObject = true;
    }
    void Update()
    {
        SpawnNewObject = false;
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle2")
        {
            SpawnNewObject = true;
            Destroy(gameObject);
        }
    }
}
