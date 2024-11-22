using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopPurchase : MonoBehaviour
{
    public CoinScriptUI CoinScriptUI;
    public List<Sprite> shopImages;
    public GameObject[] shopItems;
    public TextMeshProUGUI[] coinText;
    public ScriptableObject[] shopScriptables;
    public Sprite shopPurchasedImg;
    public Cards[] ShopItem;
    public bool HasPurchased;
    public GameObject shopItemPurchased;

    private void Awake()
    {
        HasPurchased = false;
        for (int i = 0; i < shopImages.Count; i++)
        {
            shopItems[i].SetActive(true);
            if (shopItems[i].transform.childCount >= 4)
            {
                coinText[i].text = ShopItem[i].coins.ToString();
                Transform TargetChild = shopItems[i].transform.GetChild(3);
                Image ImageComponent = TargetChild.GetComponent<Image>();
                ImageComponent.color = Color.black;
      

                if(ImageComponent != null)
                {
                    if (shopImages[i] != null)
                    {
                        ImageComponent.sprite = shopImages[i];

                    }
                }
            }
        }
    }


    private void Start()
    {
        CoinScriptUI = FindObjectOfType<CoinScriptUI>();
        Debug.Log(CoinScriptUI.coins);
    }

    public void ButtonClick()
    {
        for(int i = 0; i < shopScriptables.Length; i++)
        {
            if(CoinScriptUI.coins >= ShopItem[i].coins)
            {
                Debug.Log($"Purchased {shopScriptables[i].name}");
                CoinScriptUI.coins = CoinScriptUI.coins - ShopItem[i].coins;
                shopItemPurchased = shopItems[i];
                HasPurchased = true;

                if(shopItemPurchased.transform.childCount >= 4)
                {
                    Transform target = shopItemPurchased.transform.GetChild(3);
                    Image ImageComponent = target.GetComponent<Image>();

                    if(ImageComponent != null)
                    {
                        ImageComponent.color = Color.white;
                    }

                    if(HasPurchased == true)
                    {
                        Transform targetPurchased = shopItemPurchased.transform.GetChild(1);
                        Image imageComponent = targetPurchased.GetComponent<Image>();

                        if(imageComponent != null)
                        {
                            imageComponent.sprite = shopPurchasedImg;
                        }
                    }
                }
            }
        }
    }
}
