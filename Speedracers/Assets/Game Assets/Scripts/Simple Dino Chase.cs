using UnityEngine;

public class SimpleDinoChase : MonoBehaviour
{
    [Header("Speed Values")]

    public float velocity;

    [Header("Targets")]

    public GameObject target;

    private void Update()
    {
        transform.Translate(velocity * Time.deltaTime, 0f, 0f);
    }
}