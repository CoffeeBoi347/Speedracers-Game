using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public bool ItsOn = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ItsOn = true;
        }
    }

    private void Update()
    {
        if(ItsOn)
            transform.Translate(0.1f, 0f, 0f);
    }
}
