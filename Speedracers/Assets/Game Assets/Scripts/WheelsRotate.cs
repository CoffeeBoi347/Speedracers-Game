using UnityEngine;

public class WheelsRotate : MonoBehaviour
{
    public PlayerCarScript carScript;

    private void Update()
    {
        carScript = FindObjectOfType<PlayerCarScript>();

        transform.Rotate(new Vector3(0f, 0f, carScript.speed * 5f));
    }
}