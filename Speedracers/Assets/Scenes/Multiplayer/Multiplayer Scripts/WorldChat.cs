using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldChat : MonoBehaviour
{
    public GameObject chatOpt;
    public void ClickedTrue()
    {
        chatOpt.gameObject.SetActive(true);
    }
}
