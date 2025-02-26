using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BabyRocket1 : MonoBehaviour
{
    [Header("Paths")]

    public Transform pathObjParent;
    public List<GameObject> rocketList = new List<GameObject>();
    private int index = 0;
    public GameObject targetObj;

    [Header("Velocity Values")]

    public float target;
    public float velocity;
    public float quaternionSpeed;
    public Rigidbody2D rb; 
    private void Awake()
    {
        foreach (Transform pathObjChild in pathObjParent)
        {
            rocketList.Add(pathObjChild.gameObject);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        velocity += 0.15f;
        if (velocity >= target)
            velocity = target;

        targetObj = rocketList[index].gameObject;
        Vector3 distance = (targetObj.transform.position - transform.position);
        float rotationAmount = Vector3.Cross(distance, transform.up).z; // cross product 
        rb.velocity = transform.up * velocity;
        rb.angularVelocity = -rotationAmount * quaternionSpeed; 

        if (distance.x <= 0.1f)
        {
            index += 1;
            targetObj = rocketList[index].gameObject;
        }
    }
}