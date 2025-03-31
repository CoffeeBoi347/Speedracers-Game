using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectorManager : MonoBehaviour
{
    public int buildIndex;

    public void OpenScene()
    {
        SceneManager.LoadScene(buildIndex);
    }
}
