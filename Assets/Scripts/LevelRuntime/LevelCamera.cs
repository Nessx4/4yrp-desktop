using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class LevelCamera : MonoBehaviour
{
	[SerializeField]
	private Transform target;

	private float startZ;

	private new Rigidbody rigidbody;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		startZ = rigidbody.position.z;
	}

	// Move towards the target transform, keeping z-pos constant.
	private void FixedUpdate()
	{
		Vector3 targetPosition = new Vector3(target.position.x, target.position.y, startZ);
		rigidbody.velocity = targetPosition - transform.position;
	}
}