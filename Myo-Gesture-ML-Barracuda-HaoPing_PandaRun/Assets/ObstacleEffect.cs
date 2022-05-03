using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEffect : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name);
            other.gameObject.transform.Translate(new Vector3(0f, 0f, 0f));
        }
    }
}
