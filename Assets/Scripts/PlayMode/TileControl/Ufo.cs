using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ufo : MonoBehaviour, IControllable
{
	[SerializeField]
	private Rigidbody2D bullet;

	private float count;
	private Vector2 moveDir;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	public void Control()
	{
		Debug.Log("Under control");
	}

	public void Move(Vector2 moveAmount)
	{
		moveDir = moveAmount;
	}

	private void FixedUpdate()
	{
		rigidbody.velocity = moveDir;
	}

	public void Action()
	{
		Fire();
	}

	private void Fire()
	{
		Rigidbody2D clone = Instantiate<Rigidbody2D>(bullet,
			new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity);

		bullet.velocity = new Vector2(0.0f, -15.0f);
	}

	public void Release()
	{
		moveDir = new Vector2(0.1f, 0.0f);
	}
}
