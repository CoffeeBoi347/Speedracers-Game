using System.Collections;
using UnityEngine;

public class CloseUpShootPlayer : MonoBehaviour
{
    public WordManagerCloseUp wordManagerCloseUp;

    [Header("Animation Clips")]
    public AnimationClip shoot;
    public AnimationClip reload;
    private Animation animationObj;

    [Header("Shooting Essentials")]
    public GameObject shootingPos;
    public GameObject bulletObj;

    private void Start()
    {
        animationObj = GetComponent<Animation>();
        wordManagerCloseUp = FindObjectOfType<WordManagerCloseUp>();
    }

    public void PlayShootAnimation()
    {
        animationObj.clip = shoot;   
        animationObj.Play();         
        StartCoroutine(Shoot());
    }

    public void PlayReloadAnimation()
    {
        animationObj.clip = reload;
        animationObj.Play();
    }

    public IEnumerator Shoot()
    {
        Instantiate(bulletObj, shootingPos.transform.position, shootingPos.transform.rotation);
        yield return null;
    }

    public IEnumerator Reloading()
    {
        float animationLength = reload.length;
        Debug.Log(animationLength);
        yield return new WaitForSeconds(animationLength);
        Debug.Log("CAN SHOOT!");
        animationObj.clip = shoot;
        animationObj.Play();
        wordManagerCloseUp.canShoot = true;

    }
}
