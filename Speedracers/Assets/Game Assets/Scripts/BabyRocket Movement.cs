using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BabyRocketMovement : MonoBehaviour
{
    [Header("Scene Index")]

    public int sceneIndex;

    [Header("Speed Values")]

    public Rigidbody2D rb;
    public float Speed;
    public float RotateSpeed;

    [Header("Untouched Values")]

    public float DistanceNew;
    public List<Transform> Targets = new List<Transform>();
    public Transform currentTarget;
    public Transform posOfeachTarget;

    [Header("Effects")]

    public ParticleSystem collisionBurst;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        if (currentTarget == null)
        {
            return;
        }

        Vector2 Distance = (Vector2)currentTarget.position - rb.position; // calculate the x and y distance between the closest and locked target and our player's rigidbody position
        rb.velocity = transform.up * Speed * Time.deltaTime; // our rocket should mostly aim upwards
        float RotateAmount = Vector3.Cross(Distance, transform.up).z;
        rb.angularVelocity = -RotateAmount * RotateSpeed * Time.deltaTime;
        ClosestPath();
    }

    void ClosestPath()
    {
        if (Targets.Count == 0)
        {
            return;
        }

        Transform closestTarget = Targets[0];
        float ClosestDistance = Vector2.Distance(transform.position, closestTarget.position); // Closest distance to the target

        for (int i = 0; i < Targets.Count; i++)
        {
            posOfeachTarget = Targets[i];
            DistanceNew = Vector2.Distance(transform.position, Targets[i].transform.position);

            if (DistanceNew < ClosestDistance)
            {
                ClosestDistance = DistanceNew;
                closestTarget = Targets[i]; // We assign the closest target
            }
        }

        currentTarget = closestTarget;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionBurst.Play();
        StartCoroutine(WaitForEffectAndReload());
    }

    private IEnumerator WaitForEffectAndReload()
    {
        yield return new WaitForSeconds(collisionBurst.main.duration);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null; // wait till load completes
        }
    }
}
