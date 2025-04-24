using System.Collections.Generic;
using UnityEngine;

public class MisileTrajectory : MonoBehaviour
{
    public GameObject Misile;
    public int HowManyMisiles = 5;
    public int CurrentMisiles = 0;
    public int MaxMisiles;
    public Vector3 Distance;
    private bool HasLauched;
    public List<GameObject> misiles = new List<GameObject>();

    private void Start()
    {
        Time.timeScale = 0f;
        MaxMisiles = HowManyMisiles;
        HasLauched = false;
    }

    private void Update()
    {
        if (!HasLauched && Input.GetMouseButtonDown(0) && MaxMisiles > CurrentMisiles)
        {
            HasLauched = true;
            if (HasLauched)
            {
                BabyRocketMovement player = FindObjectOfType<BabyRocketMovement>();
                CurrentMisiles++;
                HowManyMisiles--;
                Vector3 MousePosition = Input.mousePosition;
                MousePosition.z = 1f;
                Vector3 CameraMousePos = Camera.main.ScreenToWorldPoint(MousePosition);
                GameObject Misiles = Instantiate(Misile, CameraMousePos, Quaternion.identity);
                player.currentTarget = Misiles.transform;
                misiles.Add(Misiles);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Time.timeScale = 1f;
            HasLauched = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (Misile != null)
        {
            for (int i = 0; i < misiles.Count - 1; i++)
            {
                Gizmos.DrawLine(misiles[i].transform.position, misiles[i + 1].transform.position);
            }

        }
    }
}
