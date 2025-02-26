using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinScript : MonoBehaviour
{
    public float speed;
    public int coins;
    public TextMeshProUGUI coinsText;
    public ParticleSystem blast;

    private void Start()
    {
   //     coinsText.text = coins.ToString();

        blast.Stop();
    }
    void Update()
    {
        Debug.Log(coins);
    //    coinsText.text = coins.ToString();
        transform.Rotate(0f, speed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            ParticleSystem newBlast = Instantiate(blast, collision.transform.position, collision.transform.rotation);
            newBlast.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            newBlast.Play();
        }
    }

}
