using System.Collections;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public float velocity = 5f;
    public float damagePower = 60f;
    public Rigidbody2D rb;

    private void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // Apply force to move upwards
        rb.velocity = new Vector2(velocity, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Prevent bullets from instantly disappearing by ignoring "Player"
        if (collision.gameObject.CompareTag("Player")) return;

        Destroy(gameObject);
    }

    IEnumerator DestroyGameObj(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
