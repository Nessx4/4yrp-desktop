using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    private Vector2 defaultDir = new Vector2(5.0f, 0.0f);
    public Rigidbody2D bullet;
    private float ufoSpeed = 8f;
    private float bulletSpeed = 3f;
    private int count;
    private int dir = 1;
    public bool captured;
    public Vector2 direction;
    public GameObject spawn;

    void Awake () {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
	
	void Update () {

		if (Input.GetKeyDown(KeyCode.F))
        {
            Fire();
        }
	}

    void FixedUpdate()
    {
        if (!captured)
            MoveDefault();
        rigidBody2D.velocity = direction * ufoSpeed;
    }

    public void MoveDefault()
    {
        direction = defaultDir;
        count += dir;
        if (count > 100)
        {
            dir = -1;
            defaultDir = new Vector2(-0.1f, 0.0f);
        }
        if (count < 10)
        {
            dir = 1;
            defaultDir = new Vector2(0.1f, 0.0f);
        }
    }
    public void Move(Vector2 direction)
    {
        this.direction = direction;
    }

    public void Fire()
    {
        Rigidbody2D bulletClone = (Rigidbody2D)Instantiate(bullet, new Vector3(spawn.transform.position.x, spawn.transform.position.y - 1.5f), spawn.transform.rotation);
        bulletClone.velocity = new Vector2(0f, -3f * bulletSpeed);
    }

}
