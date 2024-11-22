using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public GameObject Instantiated;
    public int randomGoBehind;
    public float speed;
    public bool CanAttack;
    public bool canSpawn;
    public ParticleSystem particleBurst;
    public bool AllowedToBurst = false;
    private void Start()
    {
        canSpawn = true;
    }
    void Update()
    {
        if (AllowedToBurst)
        {
            particleBurst.Play();
        }
        transform.Translate(-speed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            int timer = 0;
            timer++;
            while (timer >= 0.3f && canSpawn == true)
            {
                AllowedToBurst = true;
                Instantiate(Instantiated.gameObject, transform.position, transform.rotation);
                speed = 0;
                timer = 0;
            }
        }
        else if(collision.gameObject.tag != "Player")
        {
            canSpawn = false;
        }

    }
}
