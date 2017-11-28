using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    public Rigidbody2D bullet;
    private float ufoSpeed = 10f;
    private float bulletSpeed = 5f;
    public Vector2 direction;

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
        rigidBody2D.velocity = direction * ufoSpeed;
    }

    public void Move(Vector2 direction)
    {
        this.direction = direction;
    }

    public void Fire()
    {
        Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 2), transform.rotation);
        bulletClone.velocity = new Vector2(0f, -3f * bulletSpeed);
    }
}
