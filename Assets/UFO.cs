using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    public Rigidbody2D bullet;
    private float ufoSpeed = 1f;
    private float bulletSpeed = 5f;

    // Use this for initialization
    void Awake () {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
	}

    void FixedUpdate()
    {
        float x_dir = 1f; //Input.GetAxis("Horizontal");
        float y_dir = 1f;
        rigidBody2D.velocity = new Vector2(x_dir * ufoSpeed, y_dir * ufoSpeed);
    }

    void Fire()
    {
        Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2), transform.rotation);
        bulletClone.velocity = new Vector2(0f, -3f * bulletSpeed);
    }
}
