using System.Collections;
using UnityEngine;

public class TrainScript : MonoBehaviour
{
    public bool IsTrainMoving = false;
    private GameObject collidedObject; // To store the reference to the collided object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsTrainMoving = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Station")
        {
            IsTrainMoving = false;
            collidedObject = collision.gameObject; // Store the object that collided
            StartCoroutine(StopTrain(2.5f)); // Start the coroutine to handle stopping and removing Rigidbody
        }
    }

    private void Update()
    {
        if (IsTrainMoving)
        {
            transform.Translate(0.1f, 0f, 0f);
        }
    }

    public IEnumerator StopTrain(float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified duration
        IsTrainMoving = true;

        if (collidedObject != null) // Check if the collided object exists
        {
            Rigidbody2D rb = collidedObject.GetComponent<Rigidbody2D>();
            BoxCollider2D boxCollider = collidedObject.GetComponent<BoxCollider2D>();

            if (rb != null)
            {
                Destroy(rb); // Remove the Rigidbody from the collided object
            }

            if(boxCollider != null)
            {
                Destroy(boxCollider);
            }
        }

        
    }
}
