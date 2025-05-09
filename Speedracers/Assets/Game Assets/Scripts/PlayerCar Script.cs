using TMPro;
using UnityEngine;

public class PlayerCarScript : MonoBehaviour
{
    [Header("Movement Values")]
    public float speed;
    public float wpm;
    public Rigidbody2D playerRB;
    public bool allowedToMove = true;
    public TextMeshProUGUI wpmTEXT;
    [Header("Word Manager")]

    public WordManagerCar wordManagerCar;
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        WordManagerCar wordManagerCar = FindObjectOfType<WordManagerCar>();
        wpm = (wordManagerCar.charactersTyped / 5) * (60 / wordManagerCar.time);

        if(wordManagerCar.timeSinceLastType > 1f)
        {
            wpm *= 0.3f;
        }
        wpmTEXT.text = "WPM: " + wpm.ToString();
        if (allowedToMove)
        {
            if (wordManagerCar.canMove)
            {
                playerRB.velocity = new Vector2(speed, playerRB.velocity.y);
            }
        }

        speed = wpm switch
        {
            < 10 => 1f,
            < 20 => 1.65f,
            < 30 => 2.35f,
            < 40 => 3.15f,
            < 50 => 3.85f,
            < 60 => 4.5f,
            < 70 => 5.5f,
            < 80 => 7f,
            < 90 => 8.5f,
            _ => 10f
        };

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            allowedToMove = false;
        }
    }
}