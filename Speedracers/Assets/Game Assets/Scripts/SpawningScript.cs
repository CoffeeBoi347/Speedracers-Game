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

    void Start()
    {
        allowedToSpawn = false;
    }

    void Update()
    {
        if (!allowedToSpawn)
        {
            float randomPosition = Random.Range(minDown, maxUp);
            int randomObjectIndex = Random.Range(0, listOfPrefabsToSpawn.Count);
            GameObject objectToSpawn = listOfPrefabsToSpawn[randomObjectIndex];
            Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + randomPosition, transform.position.z);
            GameObject ObjectSpawned = Instantiate(objectToSpawn, newPosition, transform.rotation, transform);
            allowedToSpawn = true;
            StartCoroutine(ChangeBoolean(deleteTime, ObjectSpawned));
        }
    }

    IEnumerator ChangeBoolean(float time, GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(time);
        Destroy(objectToDestroy, 4f);
        allowedToSpawn = false;
    }
}
