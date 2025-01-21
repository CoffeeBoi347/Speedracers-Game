using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemyOne : MonoBehaviour
{
    [Header("Destinations")]

    public Transform pointA;
    public Transform pointB;

    [Header("Player")]

    public GameObject player; [Tooltip("Player Reference")]

    [Header("Detection Values")]

    public float lookRadius = 10f;
    public float attackRadius;
    public float patrollingSpeed = 0.5f;
    public float chasingSpeed = 8f;

    [Header("AI Reference")]

    public NavMeshAgent enemyMesh;
    public Vector3 currentPos;
    public Transform currentTarget;

    [Header("Booleans")]

    public bool IsPatrolling = true;
    public bool IsChasing = false;
    void Start()
    {
        if(enemyMesh != null)
        {
            enemyMesh = GetComponent<NavMeshAgent>();
            currentPos = transform.position;
            enemyMesh.speed = patrollingSpeed;
        }
        currentTarget = pointA;
    }

    void Update()
    {
        currentPos = transform.position;

        if (IsPatrolling)
            MoveTowardsTarget();
        DetectPlayer();
        ChasePlayer();
            

        if(Vector3.Distance(currentPos, currentTarget.transform.position) <= 0.1f)
        {
            SwitchTarget();
        }
    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(currentPos, currentTarget.transform.position, patrollingSpeed);
    }

    void SwitchTarget()
    {
        currentTarget = (currentTarget == pointA) ? pointB : pointA;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void DetectPlayer()
    {
        float distanceBetweenPlayer = Vector3.Distance(currentPos, player.transform.position);

        if (distanceBetweenPlayer <= lookRadius)
        {
            IsChasing = true;
            IsPatrolling = false;
        }
    }


    void ChasePlayer()
    {
        if(IsChasing)
            transform.position = Vector3.MoveTowards(currentPos, player.transform.position, chasingSpeed);
    }
}
