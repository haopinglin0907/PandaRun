using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject Bamboo;
    public GameObject Player;

    int ObstacleNum = 30;
    int BambooNum = 100;
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate at position (0, 0, 0) and zero rotation.
        for (int i = 0; i < ObstacleNum; i++)
        { 
            Instantiate(Obstacle, new Vector3(Random.Range(-8, 17), -1.7f, Random.Range(20, 470)), Quaternion.identity);
        }

        for (int i = 0; i < BambooNum; i++)
        {
            Instantiate(Bamboo, new Vector3(Random.Range(-10, 17), 0, Random.Range(20, 470)), Quaternion.identity);
        }

        Player.GetComponent<PlayerMovement>().canMove = false;

        StartCoroutine(MovableAfter3Coroutine());
    }


    IEnumerator MovableAfter3Coroutine()
    {
        
        yield return new WaitForSeconds(3);
        Player.GetComponent<PlayerMovement>().canMove = true;

    }


}
