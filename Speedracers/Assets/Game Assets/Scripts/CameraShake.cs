using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public BossAI bossAI;
    public Transform myCam;
    private Vector3 originalPosition;
    public PlaneMovementPlayer1 player1;
    public float shakeAmount = 0.1f;  // The intensity of the shake
    public float shakeDuration = 0.5f; // The duration of the shake
    public bool shakeX = true;  // Option to shake on the X axis
    public bool shakeY = true;  // Option to shake on the Y axis

    private void Update()
    {
        player1 = FindObjectOfType<PlaneMovementPlayer1>();
        bossAI = FindObjectOfType<BossAI>();

        if (bossAI.isAttacking || player1.damagedByBoss)
        {
            StartCoroutine(ShakeCamera(shakeDuration));
        }
    }

    IEnumerator ShakeCamera(float duration)
    {
        originalPosition = myCam.transform.position;
        float elapsedTime = 0f;

        // Shake the camera for the given duration
        while (elapsedTime < duration)
        {
            // Shake on the X axis (if enabled)
            float x = shakeX ? Random.Range(-shakeAmount, shakeAmount) : 0f;

            // Shake on the Y axis (if enabled)
            float y = shakeY ? Random.Range(-shakeAmount, shakeAmount) : 0f;

            // Apply the shake to the camera's local position
            myCam.transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset the camera position after the shake
        myCam.transform.position = originalPosition;
    }
}
