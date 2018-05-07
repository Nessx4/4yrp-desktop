using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class RuntimeCamera : MonoBehaviour
{
	[SerializeField]
	private Transform player;

	private float xSnapPosLeft = -3.5f;
	private float xSnapPosRight = 3.5f;
	private float xBoundLeft = -7.0f;
	private float xBoundRight = 7.0f;

	private float ySnapPosLower = -2.5f;
	private float ySnapPosUpper = 2.5f;
	private float yBoundLower = -5.0f;
	private float yBoundUpper = 5.0f;

	private bool facingRight = true;

	private bool facingUp = true;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector3 targetPosition = transform.position;

		float playerX = player.position.x;
		float playerY = player.position.y;
		float cameraX = targetPosition.x;
		float cameraY = targetPosition.y;

		if (facingRight)
		{
			if (playerX > cameraX + xSnapPosLeft)
				targetPosition.x = playerX - xSnapPosLeft;
			else if (playerX < cameraX + xBoundLeft)
				facingRight = false;
		}
		else
		{
			if (playerX < cameraX + xSnapPosRight)
				targetPosition.x = playerX - xSnapPosRight;
			else if (playerX > cameraX + xBoundRight)
				facingRight = true;
		}

		/*
		if (facingUp)
		{
			if (playerY > cameraY + ySnapPosLower)
				targetPosition.y = playerY - ySnapPosLower;
			else if (playerY < cameraY + yBoundLower)
				facingUp = false;
		}
		else
		{
			if (playerY < cameraY + ySnapPosUpper)
				targetPosition.y = playerY - ySnapPosUpper;
			else if (playerY > cameraY + yBoundUpper)
				facingUp = true;
		}
		*/

		rigidbody.velocity = (targetPosition - transform.position) * 2.5f;
	}
}
