using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = false;
    public float speed;
    Rigidbody rb;
    Vector3 horizontalMove;
    float horizontalInput;
    public float horizontalMultiplier = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (canMove)
        { 
            horizontalInput = Input.GetAxis("Horizontal");

            Vector3 forwardMove = transform.forward * speed * Time.deltaTime;

            Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.deltaTime * horizontalMultiplier;

            if (horizontalInput != 0)
                rb.MovePosition(rb.position + forwardMove + horizontalMove);
            else
            {   
                // MovePosition doesn't use rb.velocity 
                rb.MovePosition(rb.position + forwardMove);

                // prevents between bouncing from obstacles 
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
        }
    }
}
