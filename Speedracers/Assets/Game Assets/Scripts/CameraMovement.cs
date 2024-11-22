using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float speed;
    public Vector3 position;
    void Update()
    {
        Vector3 newPosition = player.transform.position + position; // position is to avoid overlapping the camera's position from player perspective
        // as its a 2D game, so the z position of ground and player lie in same axis. Henceforth.
        Vector3 desiredPosition = Vector3.MoveTowards(transform.position, newPosition, speed);
        // constantly update the camera by making it move towards its desired position by a given speed
        transform.position = desiredPosition;
    }
}
