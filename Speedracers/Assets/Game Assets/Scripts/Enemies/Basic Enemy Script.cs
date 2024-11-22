using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Config", menuName = "Wave")]
public class BasicEnemyScript : ScriptableObject
{
    public Transform wavePrefab;
    public float speed;

    public Transform GetStartWavePoint()
    {
        return wavePrefab.GetChild(0);
    }

    public List<Transform> GetWavePoints()
    {
        List<Transform> WavePoints = new List<Transform>();
        foreach(Transform wave in wavePrefab)
        {
            WavePoints.Add(wave);
        }
        return WavePoints;
    }

    public float GetMoveSpeed()
    {
        return speed;
    }
}
