using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayCamera : MonoBehaviour 
{
	[SerializeField] 
	private GameObject player;

	private Vector3 offset = new Vector3(0.0f, 0.0f, -20.0f);

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		rigidbody.velocity = ((player.transform.position + offset) - transform.position) * 5.0f;
	}
}
