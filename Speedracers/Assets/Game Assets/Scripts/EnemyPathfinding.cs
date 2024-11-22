using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    public BasicEnemyScript BasicEnemyScript;
    public List<Transform> targets = new List<Transform>();
    private int index = 0;
    void Start()
    {
        targets = BasicEnemyScript.GetWavePoints();
        transform.position = targets[index].position;
    }

    void Update()
    {
        FollowPath();
    }

    void FollowPath()
    {
        if(index < targets.Count)
        {
            Vector3 targetPos = targets[index].position;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, BasicEnemyScript.speed * Time.deltaTime);
            if(transform.position == targetPos)
            {
                index++;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
