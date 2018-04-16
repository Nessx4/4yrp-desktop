using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EditorCamera : MonoBehaviour 
{
	[SerializeField]
	private Camera mainCamera;

	private readonly Vector2 lowerLeft = new Vector2(0.5f, 0.0f);
	private readonly Vector2 upperRight = new Vector2(10.5f, 10.5f);

	private Vector2 movement;
	private const float moveSpeed = 2.5f;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		movement = new Vector2(Input.GetAxis("Horizontal"), 
			Input.GetAxis("Vertical")) * moveSpeed;
	}

	private void FixedUpdate()
	{
		rigidbody.velocity = movement;
		BoundPosition();
	}

	private void BoundPosition()
	{
		Vector2 pos = transform.position;
		float size  = mainCamera.orthographicSize;

		Vector2 ll = new Vector2(size * mainCamera.aspect - 0.5f, size - 0.5f);
		Vector2 ur = new Vector2(99.5f - size * mainCamera.aspect, 99.5f - size);

		pos.x = Mathf.Clamp(pos.x, ll.x, ur.x);
		pos.y = Mathf.Clamp(pos.y, ll.y, ur.y);

		transform.position = new Vector3(pos.x, pos.y, -10.0f);
	}
}
