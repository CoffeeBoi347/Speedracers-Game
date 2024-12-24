using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour, IMenu 
{
    [SerializeField] private EMenuName menuName;
    public EMenuName MenuName { get => menuName; set => menuName = value; }
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component is missing on this GameObject!");
        }
    }
    
    private void Start()
    {
        GameManagerMultiplayer.Instance.AddMenu(this);
    }

    public void OpenMenu()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void CloseMenu()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

}
