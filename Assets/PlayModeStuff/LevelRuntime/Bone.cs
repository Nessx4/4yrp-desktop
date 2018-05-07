using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    private float spinspeed = 200f;

    // Use this for initialization
    void Awake () {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        rigidBody2D.MoveRotation(rigidBody2D.rotation - spinspeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(this.gameObject);
    }
}
