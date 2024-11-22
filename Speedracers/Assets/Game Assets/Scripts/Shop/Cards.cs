using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Character", menuName = "ShopMenu", order = 1)]

public class Cards : ScriptableObject
{
    public string name;
    public int coins;

}