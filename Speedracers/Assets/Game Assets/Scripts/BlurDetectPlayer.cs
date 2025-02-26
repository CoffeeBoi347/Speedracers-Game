using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlurDetectPlayer : MonoBehaviour
{
    public Animation closeFadeClip;
    public GameObject closeFadeObj;

    private void Start()
    {
        closeFadeObj.SetActive(false);
        closeFadeClip.Stop();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            closeFadeObj.SetActive(true);
            closeFadeClip.Play();
            StartCoroutine(Cutscene(2f));
        }
    }

    private IEnumerator Cutscene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Cutscene");
    }
}
