using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   
public class CoinScriptUI : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public int coins;
    void Start()
    {
        
    }

    void Update()
    {
        coinText.text = coins.ToString();
    }
}
