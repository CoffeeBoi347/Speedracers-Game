using System;
using UnityEngine;

public class AICarScript : MonoBehaviour
{
    [Header("Speed Values")]
    public float carAccelerationForce = 0.3f;
    public float minSpeed = 2f;
    public float maxSpeed = 5.75f;
    public float deacceleration = 0.1f;
    private Vector2 distanceBtwnPlayer;
    public GameObject player;
    public Rigidbody2D rb;
    public float currentSpeed;

    [Header("Behavioural Values")]
    [Range(0, 100f)] public float failureChance = 10f;
    [Range(0, 100f)] public float aggressionChance = 60f;
    [Range(0, 100f)] public float passiveChance = 20f;
    [Range(0, 100f)] public float failureRate = 5f;
    public CarStates carStates;
    public bool isAhead;
    public bool isBehind;

    private void Start()
    {
        Debug.Log("Do i even work?");
        currentSpeed = minSpeed;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
            distanceBtwnPlayer = player.transform.position - transform.position;
            isAhead = transform.position.x > player.transform.position.x;
            isBehind = transform.position.x < player.transform.position.x;
            Debug.Log($"isAhead: {isAhead}. isBehind: {isBehind}.");
            if (isBehind)
            {
                Debug.Log("I AM BEHIND THE PLAYER...!");
                if (UnityEngine.Random.Range(0, 100) < failureChance)
                {
                    Debug.Log("FAILURE RATE..I WILL BE UNABLE TO MATCH THE PLAYER!");
                    carStates = CarStates.Failure;
                    currentSpeed = Mathf.Max(minSpeed, currentSpeed - deacceleration);
                }
                else
                {
                    Debug.Log("GOING BERSERK.");
                    maxSpeed = UnityEngine.Random.Range(maxSpeed, maxSpeed + 0.5f);
                    currentSpeed = Mathf.Min(maxSpeed, currentSpeed + carAccelerationForce * Time.deltaTime);
                }
            }

            if (distanceBtwnPlayer.x <= 3f && isBehind)
            {
                Debug.Log("PLAYER IS VERY NEAR!!!");
                if (UnityEngine.Random.Range(0, 100) < aggressionChance)
                {
                    Debug.Log("GOING AGGRESSIVE MODE!");
                    carStates = CarStates.Aggressive;
                    maxSpeed = UnityEngine.Random.Range(maxSpeed, maxSpeed + 0.5f);
                    currentSpeed = Mathf.Min(maxSpeed, currentSpeed + carAccelerationForce * 2 * Time.deltaTime);
                }
                else
                {
                    Debug.Log("NAH.. I rather match the player's speed.");
                    currentSpeed = Mathf.Lerp(currentSpeed, maxSpeed, Time.deltaTime);
                }
            }

            if (isAhead)
            {
                int randomValue = UnityEngine.Random.Range(0, 100);
                Debug.Log(" I AM AHEAD OF THE PLAYER !!! ");
                if (randomValue < passiveChance)
                {
                    Debug.Log("I AM IN PASSIVE MODE.. CHILLING!");
                    carStates = CarStates.Passive;
                    currentSpeed = Mathf.Max(minSpeed, currentSpeed - deacceleration);
                }
                else if (randomValue < aggressionChance)
                {
                    Debug.Log("NAH I RATHER GO AGGRESSSIVE EVEN IF I AM AHEAD!");
                    carStates = CarStates.Aggressive;
                    currentSpeed = Mathf.Min(maxSpeed, currentSpeed + carAccelerationForce * 0.7f * Time.deltaTime);
                }
            }
    }
}

public enum CarStates
{
    Aggressive,
    Passive, 
    Failure
}
