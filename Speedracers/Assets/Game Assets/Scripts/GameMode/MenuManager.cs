using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup[] menuNames;
    private const int STORY_MODE_INDEX = 2;
    private const int PRACTICE_MODE_INDEX = 1;
    private const int CHAPTER_ONE_LEVELS_INDEX = 3;

    [Header("Buttons")]

    public Button storyMode;
    public Button chapterOneBtn;
    public Button practiceMode;
    private void Start()
    {
        for(int i = 0; i <= menuNames.Length; i++)
        {
            SetCanvasGroupState(menuNames[i], i==0);
        }

        storyMode.onClick.AddListener(OnStoryModeClicked);
        practiceMode.onClick.AddListener(OnPracticeModeClicked);
        chapterOneBtn.onClick.AddListener(OnChapterOneBtnClicked);
    }

    private void SetCanvasGroupState(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }

    void OnStoryModeClicked()
    {
        for(int i = 0; i <= menuNames.Length;i++)
        {
            SetCanvasGroupState(menuNames[i], i == STORY_MODE_INDEX);
        }
    }

    void OnPracticeModeClicked()
    {
        for (int i = 0; i <= menuNames.Length; i++)
        {
            SetCanvasGroupState(menuNames[i], i == PRACTICE_MODE_INDEX);
        }
    }

    public void OnChapterOneBtnClicked()
    {
        for (int i = 0; i <= menuNames.Length; i++)
        {
            SetCanvasGroupState(menuNames[i], i == CHAPTER_ONE_LEVELS_INDEX);
        }
    }
}