using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Player;
    Vector3 Offset;
    // Start is called before the first frame update
    void Start()
    {
        Offset = transform.position - Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = Player.position + Offset;
        targetPos.x = 3.5f;
        transform.position = targetPos;
    }
}
