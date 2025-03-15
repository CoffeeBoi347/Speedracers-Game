using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GoToNextSceneAfterCutscene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public int sceneIndex;
    void Start()
    {
        if(playableDirector != null)
        {
            double duration = playableDirector.duration;
            StartCoroutine(GoToNextScene(duration));
        }
    }

    IEnumerator GoToNextScene(double time)
    {
        yield return new WaitForSeconds(time.ConvertTo<float>());
        SceneManager.LoadScene(sceneIndex);
    }
}
