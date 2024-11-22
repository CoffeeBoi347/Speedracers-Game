using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public Transform bulletPos;
    public GameObject player;
    private float timer = 0.2f;
    private int index = 0;
    public ParticleSystem particleFX;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        gameObject.SetActive(true); 
        transform.Translate(speed * Time.deltaTime, 0f, 0f);
        
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if(distance >= 0)
        {
            float i = Time.deltaTime;

            if(i >= timer)
            {
                Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
                i = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            particleFX.gameObject.SetActive(true);
            index++;
            Destroy(collision.gameObject);

            if(index >= 3)
                Destroy(gameObject);
        }
    }
}
