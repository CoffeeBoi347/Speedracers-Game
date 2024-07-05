using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningScript : MonoBehaviour
{
    public List<GameObject> listOfPrefabsToSpawn = new List<GameObject>();
    public float maxUp;
    public float minDown;
    public float deleteTime;
    private bool allowedToSpawn;
    public bool NewObject;
    public bool Destroyed;
    private GameObject spawnedObject = null;

    void Start()
    {
        Destroyed = false;
        NewObject = false;
        allowedToSpawn = false;
    }

    void Update()
    {
        if (!allowedToSpawn && spawnedObject == null)
        {
            SpawnObject();
        }

        else if(NewObject == true)
        {

            float timer = Time.deltaTime;
            if(timer >= 2f)
            {
                SpawnObject();
                NewObject = false;
                timer = 2f;

                if (spawnedObject != null)
                {
                    Destroy(spawnedObject);
                }

            }

        }
    }

    public void SpawnObject()
    {
        float randomPosition = Random.Range(minDown, maxUp);
        int randomObjectIndex = Random.Range(0, listOfPrefabsToSpawn.Count);
        GameObject objectToSpawn = listOfPrefabsToSpawn[randomObjectIndex];
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + randomPosition, transform.position.z);
        spawnedObject = Instantiate(objectToSpawn, newPosition, transform.rotation, transform);
        allowedToSpawn = true;
    }

    IEnumerator ChangeBoolean(float time, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(time);

        Destroy(objectToDestroy);
        Destroyed = true;
        allowedToSpawn = false;
        spawnedObject = null;
    }

    public void Delete()
    {
        StartCoroutine(ChangeBoolean(deleteTime, spawnedObject));
    }
}