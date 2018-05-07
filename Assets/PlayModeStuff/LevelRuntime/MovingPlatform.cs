using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    private int moveLimit = 0;
    private int flip = 1;
    private int maxSpeed = 10;
    //private int moveSpeed = 5;

    // Use this for initialization
    void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (moveLimit < 200)
        {
            if (rigidBody2D.velocity.x < maxSpeed)
            {
                rigidBody2D.AddForce(Vector2.right * flip);
                moveLimit++;
            }
        }
        else
        {
            flip = flip * -1;
            moveLimit = -200;
        }

    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }
}
