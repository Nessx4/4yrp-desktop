using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Ufo : Controllable
{
	[SerializeField]
	private Rigidbody2D bullet;

	private float count;
	private Vector2 moveDir = new Vector2(0.1f, 0.0f);

	private bool captured = false;
	private bool left = false;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	public override void Control()
	{
		Debug.Log("Under control");

		captured = true;
	}

	public override void Move(Vector2 moveAmount)
	{
		moveDir = moveAmount;
	}

	private void FixedUpdate()
	{
		if(!captured)
			MoveDefault();

		rigidbody.velocity = moveDir;
	}

	private void MoveDefault()
	{
		if(count > 2.5f)
		{
			moveDir = left ? new Vector2(0.1f, 0.0f) : new Vector2(-0.1f, 0.0f);
			left = !left;
			count = 0.0f;
		}

		count += Time.fixedDeltaTime;
	}

	public override void Action()
	{
		Fire();
	}

	private void Fire()
	{
		Rigidbody2D clone = Instantiate<Rigidbody2D>(bullet,
			new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity);

		bullet.velocity = new Vector2(0.0f, -15.0f);
	}

	public override void Release()
	{
		moveDir = new Vector2(0.1f, 0.0f);

		captured = false;
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
			Fire();
	}
}
