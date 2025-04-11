using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager4 : MonoBehaviour
{
    [Header("Scene Indexes")]

    public int sceneIndexPlayMenu = 1;
    public int sceneIndexMultiplayerMenu = 5;
    public int sceneIndexShopMenu = 12;
    public GameObject transitionImage;

    public void PlayButton()
    {
        transitionImage.SetActive(true);
        StartCoroutine(OpenSceneIndex(sceneIndexPlayMenu, 1f));
    }

    public void MultiplayerButton()
    {
        transitionImage.SetActive(true);
        StartCoroutine(OpenSceneIndex(sceneIndexMultiplayerMenu, 1f));
    }

    public void ShopButton()
    {
        transitionImage.SetActive(true);
        StartCoroutine(OpenSceneIndex(sceneIndexShopMenu, 1f));
    }

    IEnumerator OpenSceneIndex(int sceneIndex, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneIndex);
    }
}
