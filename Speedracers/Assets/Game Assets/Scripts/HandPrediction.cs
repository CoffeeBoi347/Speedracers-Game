using System.Collections.Generic;
using UnityEngine;

public class HandPrediction : MonoBehaviour
{
    [Header("Hand References")]

    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Keys List")]

    public List<Transform> keys;

    [Header("Boss Word Manager")]

    public BossWordManager wordManager;

    private void Start()
    {
        wordManager = FindObjectOfType<BossWordManager>();
    }

    private void Update()
    {
        
    }
}