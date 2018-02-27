using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    public Rigidbody2D bone;
    public Transform boneSpawnPoint;
    public Transform colCheck;

    private float skeleSpeed = 4f;
    private float boneSpeedX = 4f;
    private float boneSpeedY = 8f;
    private int forward = 1;
    private bool linecastDown;
    private bool linecastUp;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        linecastDown = Physics2D.Linecast(colCheck.position, new Vector3(colCheck.position.x, colCheck.position.y - 2.0f));
        linecastUp = Physics2D.Linecast(colCheck.position, new Vector3(colCheck.position.x, colCheck.position.y + 3.0f));

        Debug.Log(linecastUp);
        Move();
        if (Input.GetKeyDown(KeyCode.B))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.T) || linecastDown == false || linecastUp == true)
        {
            Flip();
        }
    }

    //Turn around when reaching an edge
    public void Move()
    {
        rigidBody2D.velocity = new Vector2(skeleSpeed * forward, rigidBody2D.velocity.y);
    }

    //Add cooldown timer
    public void Fire()
    {
        Rigidbody2D boneClone = (Rigidbody2D)Instantiate(bone, new Vector3(boneSpawnPoint.position.x, boneSpawnPoint.position.y + 1.4f), transform.rotation);
        boneClone.velocity = new Vector2(boneSpeedX * forward, boneSpeedY);
    }


    public void Flip()
    {
        forward = forward * -1;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
