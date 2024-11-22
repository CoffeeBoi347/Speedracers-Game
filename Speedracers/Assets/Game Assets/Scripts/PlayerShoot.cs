using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    public Transform placeToShoot;
    public float delay = 0.25f; // Change delay to 0.25 seconds
    private float timer = 0f;
    private bool allowToShoot = true;

    void Update()
    {
        if (allowToShoot)
        {
            timer += Time.deltaTime;

            if (timer >= delay)
            {
                Shoot();
                timer = 0f; // Reset timer after shooting
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, placeToShoot.transform.position, placeToShoot.transform.rotation);
    }
}
