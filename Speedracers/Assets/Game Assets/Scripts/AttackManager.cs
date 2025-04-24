using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [Header("Screen Pause Settings")]

    public float screenPauseTime;

    [Header("Screen Shake Settings")]

    public float magnitude;
    public float shakeDuration;
    public Camera mainCam;

    public void TakeHit()
    {
        StartCoroutine(SetScreenPause(screenPauseTime));
        StartCoroutine(SetScreenShake(shakeDuration));
    }

    private IEnumerator SetScreenPause(float time)
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(time);
        Time.timeScale = 1f;
    }

    private IEnumerator SetScreenShake(float time)
    {
        Vector3 originalCamPos = mainCam.transform.localPosition;
        float elapsedTime = 0f;
        while(time > elapsedTime)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            mainCam.transform.localPosition = new Vector3(x, y, originalCamPos.z);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        mainCam.transform.localPosition = originalCamPos;
    }
}