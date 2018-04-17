using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EditorCamera : MonoBehaviour 
{
	[SerializeField]
	private Camera mainCamera;

	private Vector2 movement;
	private const float moveSpeed = 5.0f;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();

		LevelEditor.instance.mainCamera = mainCamera;
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

		Vector2 ll = new Vector2(size * mainCamera.aspect, size);
		Vector2 ur = new Vector2(100.0f - size * mainCamera.aspect, 100.0f - size);

		pos.x = Mathf.Clamp(pos.x, ll.x, ur.x);
		pos.y = Mathf.Clamp(pos.y, ll.y, ur.y);

		transform.position = new Vector3(pos.x, pos.y, -10.0f);
	}
}
