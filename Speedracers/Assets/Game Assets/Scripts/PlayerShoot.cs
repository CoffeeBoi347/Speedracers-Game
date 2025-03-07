using System.Collections;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform placeToShoot;
    public float delay = 0.25f;
    private float timer = 0f;
    public bool allowToShoot = false;
    public Rigidbody2D playerRb;

    void Update()
    {
        if (allowToShoot)
        {
            timer += Time.deltaTime;

            if (timer >= delay)
            {
                Shoot();
                timer = 0f; 
            }
        }
    }

    void Shoot()
    {
        Debug.Log("SPAWNING BULLET!");
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
