using UnityEngine;

public class HealthManagerBoss : MonoBehaviour
{
    [Header("Health Attributes")]
    public float bossHealth;
    public Sprite[] healthBox;
    public SpriteRenderer healthImage;
    private float maxHealth;
    public BossAI bossAI;

    void Start()
    {
        bossAI = GetComponent<BossAI>();
        maxHealth = bossHealth;
    }
    void Update()
    {
        bossHealth = bossAI.health;

        if (bossHealth >= 725 && bossHealth < maxHealth)
        {
            healthImage.sprite = healthBox[0];
        }

        else if(bossHealth >= 580 && bossHealth < 724)
        {
            healthImage.sprite = healthBox[1];
        }

        else if(bossHealth >= 435 && bossHealth < 579)
        {
            healthImage.sprite = healthBox[2];
        }
        else if(bossHealth >= 290 && bossHealth < 434)
        {
            healthImage.sprite = healthBox[3];
        }
        else if(bossHealth >= 145 && bossHealth < 289)
        {
            healthImage.sprite = healthBox[4];
        }
        else if(bossHealth >= 70 && bossHealth < 144)
        {
            healthImage.sprite = healthBox[5];
        }

        else if(bossHealth < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
