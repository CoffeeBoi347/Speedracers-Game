using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform upperLimit;
    public Transform lowerLimit;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, 3f);  
    }

    void SpawnEnemy()
    {
        float randomY = Random.Range(upperLimit.position.y, lowerLimit.position.y);

        float spawnX = Camera.main.transform.position.x + 12.5f;

        int randomEnemyIndex = Random.Range(0, enemies.Length);
        GameObject selectedEnemy = enemies[randomEnemyIndex];

        Instantiate(selectedEnemy, new Vector3(spawnX, randomY, 0f), Quaternion.identity);
    }

}
