using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public List<GameObject> cars;
    public Transform spawnObjMax;
    public bool CanSpawn = false;
    public Transform spawnObjMin;
    public float SpawnTime;
    public bool AllowedToSpawn = true;
    private MoveCar moveCar;
    void Start()
    {
        CanSpawn = false;
        moveCar = FindObjectOfType<MoveCar>();
        moveCar = GetComponent<MoveCar>();  
        InvokeRepeating("Spawn", 0f, 6f);
    }

    // Update is called once per frame
    void Spawn()
    {
        if(AllowedToSpawn == true)
        {

            int randomCar = Random.Range(0, cars.Count);
            GameObject SpawnObj = cars[randomCar];
            float randomPos = Random.Range(spawnObjMax.transform.position.y, spawnObjMin.transform.position.y);
            Vector3 spawnPos = new Vector3(transform.position.x, randomPos, transform.position.z);
            Instantiate(SpawnObj, spawnPos, Quaternion.identity);
        }
    }
}
