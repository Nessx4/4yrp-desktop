using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Bullet : MonoBehaviour 
{
	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		rigidbody.velocity = new Vector2(0.0f, -15.0f);
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		Destroy(gameObject);
	}
}
