using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGeneratorScript : MonoBehaviour
{
    public GameObject obstacleObj;

    public float[] generateYRange;
    public float generateDistance;

    private void Start()
    {
        GenerateObstacle();
    }

    public void GenerateObstacle()
    {
        int generateTime = Mathf.FloorToInt((generateYRange[0] - generateYRange[1]) / generateDistance) - 1;

        for (int i = 1; i <= generateTime; i++)
        {
            GameObject.Instantiate(obstacleObj, new Vector3(0, -generateDistance * i, 0),
                Quaternion.Euler(0, Random.Range(0, 360), 0));
        }
    }
}
