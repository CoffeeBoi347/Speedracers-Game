using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundRotate : MonoBehaviour
{
    public float ZSpeed;
    void Update()
    {
        transform.Rotate(0f, 0f, ZSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        ZSpeed += 0.01f;
    }
}
