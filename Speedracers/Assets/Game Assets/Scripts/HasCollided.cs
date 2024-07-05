using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasCollided : MonoBehaviour
{
    public int count = 0;
    public bool AllowedToDelete = false;
    public SpawningScript spawningScript;
    public int oldCount;

    private void Start()
    {
        oldCount = count;
        spawningScript = FindObjectOfType<SpawningScript>();
        AllowedToDelete = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            count += 1;
            if(count > oldCount)
            {
                oldCount += 1;
                AllowedToDelete = true;
                spawningScript.Delete();
            }
        }
        else
        {
            AllowedToDelete = false;
        }
    }

    private void Update()
    {
        if(AllowedToDelete == true)
        {
            spawningScript.NewObject = true;
        }
    }
}
