using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject Bamboo;

    int ObstacleNum = 100;
    int BambooNum = 100;
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        for (int i = 0; i < ObstacleNum; i++)
        { 
            Instantiate(Obstacle, new Vector3(Random.Range(-10, 10), 0, Random.Range(0, 470)), Quaternion.identity);
        }

        for (int i = 0; i < BambooNum; i++)
        {
            Instantiate(Bamboo, new Vector3(Random.Range(-10, 10), 0, Random.Range(0, 470)), Quaternion.identity);
        }
    }

    void Update()
    {
        
    }


}
