using UnityEngine;

public class HealthManagerBattle : MonoBehaviour
{
    [Header("Health Attributes")]
    public float health;
    public Sprite[] healthBox;
    public SpriteRenderer healthImage;
    private float maxHealth;
    private float collisionDamage = 4f;

    private void Start()
    {
        maxHealth = health;
    }
    private void Update()
    {
        if(health >= 334 && health <= maxHealth)
        {
            healthImage.sprite = healthBox[0];
        }

        else if(health >= 267 && health < 333)
        {
            healthImage.sprite = healthBox[1];
        }

        else if(health >= 200 && health < 266)
        {
            healthImage.sprite = healthBox[2];
        }

        else if(health >= 133 && health < 199)
        {
            healthImage.sprite = healthBox[3];
        }

        else if(health >= 67 && health < 132)
        {
            healthImage.sprite = healthBox[4];
        }

        else if(health >= 15 && health < 66)
        {
            healthImage.sprite= healthBox[5];
        }
        else if(health < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        health -= collisionDamage;
    }
}
