using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBurst : MonoBehaviour
{
    private PlaneMovementPlayer player;
    public ParticleSystem burstFX;

    private void Start()
    {
        player = FindObjectOfType<PlaneMovementPlayer>();
    }
    void Update()
    {
        if(player.CollidedWithPPowerFX == true)
        {
            if(burstFX != null )
            {
                burstFX.gameObject.SetActive(true);
                burstFX.Play();
            }
            DestroyObject(1.5f);
        }
    }

    private IEnumerator DestroyObject(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
