using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public float velocity;
    public Vector3 startPos;
    public float damagePower = 60f;
    private void Start()
    {
        startPos = transform.position;
        StartCoroutine("DestroyGameObj", 0.27f);
    }

    void Update()
    {
        transform.Translate(velocity * Time.deltaTime, 0f, 0f);

        if ((startPos.x - transform.position.x) >= 600f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 forceDirection = (collision.transform.position - transform.position).normalized;
            float forceMagnitude = 5f;
            rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        }

        Destroy(gameObject);
    }

    private IEnumerator DestroyGameObj(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);
    }
}
