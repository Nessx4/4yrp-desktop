using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    public Transform colCheck;
    public Transform colCheckUp;
    public Transform colCheckDown;
    public Transform colCheckB;
    public Transform colCheckBDown;
    public LayerMask whatsGround;

    private float slimeSpeed = 1.5f;
    private bool linecastDown;
    private bool linecastUp;
    private bool BColliders = false;

    private int forward = 1;
    // Use this for initialization
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update () {

        CollisionCheck();
        //Move();
        if (linecastUp)
        {
            rigidBody2D.transform.Rotate(Vector3.forward * 90 * forward);
            BColliders = false;
        }
        else
        {
            BColliders = true;
        }

        if (!linecastDown)
        {
            rigidBody2D.transform.Rotate(Vector3.forward * -90 * forward);
            rigidBody2D.transform.position += transform.right * forward * 0.8f;
            BColliders = !BColliders;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Flip();
        }

        Move();
    }

    public void CollisionCheck()
    {
        linecastUp = Physics2D.Linecast(colCheck.position, colCheckUp.position, whatsGround);
        linecastDown = Physics2D.Linecast(colCheckB.position, colCheckBDown.position, whatsGround);
    }

    public void Move()
    {
        rigidBody2D.velocity = rigidBody2D.transform.right * slimeSpeed * forward;
    }

    void Flip()
    {
        forward = forward * -1;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
