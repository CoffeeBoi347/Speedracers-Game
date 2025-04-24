using UnityEngine;

public class ContinousShooting : MonoBehaviour
{
    public Transform spawnArea;
    public GameObject bulletObj;
    public ParticleSystem burstFX;
    public float timerToSpawn;
    public float timer;
    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if(timer >= timerToSpawn)
        {
            SpawnBullet();
            timer = 0;
        }
    }

    void SpawnBullet()
    {
        burstFX.Play();
        Instantiate(bulletObj, spawnArea.transform.position, spawnArea.transform.rotation);
    }
}