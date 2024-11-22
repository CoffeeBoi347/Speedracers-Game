using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknamePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inpField;
    [SerializeField] private Button inpButton;
    private int interactableAllowed = 4;
    
    void Start()
    {
        inpButton.interactable = false;
        inpButton.onClick.AddListener(ClickCreateNickname);
        inpField.onValueChanged.AddListener(ValueChanged);
    }

    void ValueChanged(string nickname)
    {
        inpButton.interactable = nickname.Length > interactableAllowed;
    }
    
    void ClickCreateNickname()
    {
        var nickname = inpField.text;
        if (nickname.Length > interactableAllowed)
        {
            inpButton.interactable = true;
        }
    }

}
