using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageButtons : MonoBehaviour
{
    public Animator anim;
    public GameObject Wipe;

    public GameObject CreditsGObj;
    public bool HasClicked = false;
    // i agree its bad practice of code i could simply do it in buttonmanagers.cs smh but my smol brain wont be able to comprehend how to fix time.deltatime

    private void Update()
    {
        DontDestroyOnLoad(anim);
    }
    public void LoadNextScene()
    {
        Wipe.SetActive(true);
        HasClicked = true;
        NextScene();

    }

    public void Credits()
    {
        CreditsGObj.SetActive(true);
    }
    public void TutorialPage()
    {
        CreditsGObj.SetActive(true);
    }

    public void NextScene()
    {
        StartCoroutine(NextScene(1f));
    }

    IEnumerator NextScene(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("EndAnim", true);
        anim.SetBool("StartAnim", false);
        yield return new WaitForSeconds(0.45f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }

    public void Close()
    {
        CreditsGObj.SetActive(false);
    }
}
