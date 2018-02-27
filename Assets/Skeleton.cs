using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

    private Rigidbody2D rigidBody2D;
    public Rigidbody2D bone;
    private float boneSpeedX = 4f;
    private float boneSpeedY = 8f;

    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            Fire();
        }
    }

    public void Fire()
    {
        Rigidbody2D boneClone = (Rigidbody2D)Instantiate(bone, new Vector3(transform.position.x + 3.5f, transform.position.y + 2.3f), transform.rotation);
        boneClone.velocity = new Vector2(1f * boneSpeedX, 1f * boneSpeedY);
    }
}
