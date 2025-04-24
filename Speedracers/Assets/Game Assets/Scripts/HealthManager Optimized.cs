using System.Collections.Generic;
using UnityEngine;

public class HealthManagerOptimized : MonoBehaviour
{
    [Header("Health Manager Essentials")]

    public List<Sprite> healthSprites = new List<Sprite>();
    public SpriteRenderer healthSpriteRenderer;
    
    [Header("Health Values")]

    public float healthOne;
    public float healthTwo;
    public float healthThree;
    public float healthFour;

    private void Start()
    {
        healthSpriteRenderer = GetComponent<SpriteRenderer>();
        healthSpriteRenderer.sprite = healthSprites[0];
    }

    public void TakeDamage()
    {

    }
}