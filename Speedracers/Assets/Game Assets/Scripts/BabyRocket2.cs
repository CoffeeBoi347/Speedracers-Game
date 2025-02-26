using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class BabyRocket2 : MonoBehaviour
{
    [Header("Paths")]
    public Transform pathObjParent;
    public List<Transform> pathPoints = new List<Transform>();
    private int index = 0;
    public Transform targetObj;

    [Header("Velocity Values")]
    public float targetSpeed;
    public float acceleration = 0.15f;
    private float currentSpeed = 0f;
    public Rigidbody2D rb;

    private void Awake()
    {
        foreach (Transform pathObjChild in pathObjParent)
        {
            pathPoints.Add(pathObjChild);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (pathPoints.Count > 0)
        {
            targetObj = pathPoints[index];
        }
    }

    private void Update()
    {
        if (targetObj != null)
        {
            MoveTowardsTarget();
        }
    }

    void MoveTowardsTarget()
    {
        currentSpeed += acceleration;
        if (currentSpeed > targetSpeed)
        {
            currentSpeed = targetSpeed;
        }

        Vector2 direction = (targetObj.position - transform.position).normalized;
        rb.velocity = direction * currentSpeed;
        if (Vector2.Distance(transform.position, targetObj.position) < 0.1f)
        {
            index++;
            if (index < pathPoints.Count)
            {
                targetObj = pathPoints[index];
            }
            else
            {
                rb.velocity = Vector2.zero; 
            }
        }
    }
}
