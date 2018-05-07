using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerTEST : MonoBehaviour
{
    [SerializeField]
    private Transform savePoint;
    private bool facingRight;
    public bool onLadder;
    public bool verticalMove = false;
    [SerializeField]
    private float moveForce = 200.0f;
    [SerializeField]
    private float playerDrag = 0.985f;

    [Space(5)]
	[Header("Jump modifiers")]
    private float climbVelocity;
    private float climbDist;
    private float climbSpeed = 5.0f;
    // How fast the player jumps upwards.
    [SerializeField]
	private float jumpStrength = 5.0f;

	// The player falls faster after being in the air for longer after jumping.
	[SerializeField]
	private float timeFallModifier = 2.5f;

	private float jumpStartedTime = 0.0f;

	// How fast the player falls after reaching jump apex.
	[SerializeField]
	private float fallModifier = 2.5f;

	// How fast the player falls if the jump button is pressed for a short time.
	[SerializeField]
	private float lowJumpModifier = 2.0f;

	private bool isJumping = false;

	private bool isJumpBtnDown = false;
	private float jumpBtnDownTime = 0.0f;
	private float jumpBtnDownLeniency = 0.1f;
	private bool isJumpBtnHeld = false;

	[Space(5)]
	[Header("Ground checking")]

	// Which layers are ground?
	[SerializeField]
	private LayerMask groundMask;

	// The end point of the ground linecast.
	[SerializeField]
	private Transform groundCheckPoint;

	private bool isGrounded = false;
	private float groundedTime = 0.0f;
	private float groundedLeniency = 0.1f;



	[Space(5)]
	[Header("Horizontal movement")]

	// Speed the player moves in the x-direction.
	[SerializeField]
	private float horSpeed = 2.5f;

	// How easy it is to influence the player's x-velocity in mid-air.
	[SerializeField]
	private float airControlFactor = 0.06125f;

	// Axis horizontal input;
	private float horInput = 0.0f;

    private int score = 0;

	private new Rigidbody2D rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody2D>();
    }

	private void Update()
	{
		horInput = Input.GetAxis("Horizontal");

		// Pressing jump has some leniency, to allow pressing jump in mid-air just 
		// before actually landing on the ground ground.
		if(Input.GetButtonDown("Jump"))
		{
			isJumpBtnDown = true;
			jumpBtnDownTime = Time.time;
		}
		if (Time.time > jumpBtnDownTime + jumpBtnDownLeniency)
			isJumpBtnDown = false;

		isJumpBtnHeld = Input.GetButton("Jump");
	}

	private void FixedUpdate()
	{
		// Determine if the player is grounded. Has leniency so you can jump
		// just after going off an edge.
		if(Physics2D.OverlapBox(groundCheckPoint.position, new Vector2(0.9f, 0.1f), 0.0f, groundMask))
		{
			isGrounded = true;
			groundedTime = Time.time;
		}
		if (Time.time > groundedTime + groundedLeniency)
			isGrounded = false;

		// Stop a jump when the player lands on ground.
		if (rigidbody.velocity.y <= 0.0f && isJumping && isGrounded)
			isJumping = false;
		
		CalculateYVelocity();
		CalculateXVelocity();
	}

	// Without modifying x, calculate the y-velocity of the player.
	private void CalculateYVelocity()
	{
        if (!verticalMove)
            verticalMove = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow));

        float yMod = 0.0f;

        if (onLadder && verticalMove)
        {
            rigidbody.gravityScale = 0.0f;
            //anim.SetFloat("climbDist", climbDist);
            if (Input.GetKey(KeyCode.UpArrow))
            {
                climbDist++;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                climbDist--;
            }

            if (climbDist > 20)
            {
                climbDist = 5;
            }
            else if (climbDist < 0)
            {
                climbDist = 20;
            }
            isGrounded = true;
            verticalMove = true;
            //anim.SetBool("OnLadder", true);
            climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");
            rigidbody.velocity = new Vector2(0.0f, climbVelocity);
        }
        else
        {
            rigidbody.gravityScale = 1.0f;
            verticalMove = false;
            if (Mathf.Abs(rigidbody.velocity.y) > 0.1f)
            {
                if (rigidbody.velocity.y < 0.0f)
                    yMod += (fallModifier - 1.0f);
                else
                {
                    if (isJumping)
                        yMod += (Time.time - jumpStartedTime) * timeFallModifier;

                    if (!isJumpBtnHeld)
                        yMod += (lowJumpModifier - 1.0f);
                }

                yMod *= Physics2D.gravity.y * Time.fixedDeltaTime;
            }
            rigidbody.velocity += new Vector2(0.0f, yMod);
            //anim.SetFloat("climbDist", 0);
            //anim.SetBool("OnLadder", false);
        }

        if (isJumpBtnDown && !isJumping && isGrounded)
		{
            if (onLadder)
            {
                onLadder = false;
                isJumping = true;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
                yMod = jumpStrength * 0.6f;
                jumpStartedTime = Time.time;
            }
            else
            {
                isJumping = true;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0.0f);
                yMod = jumpStrength;
                jumpStartedTime = Time.time;
            }
            rigidbody.velocity += new Vector2(0.0f, yMod);
        }

	}

	// Without modifying y, calculate the x-velocity of the player.
	private void CalculateXVelocity()
	{
        /*
		Vector2 velocity = rigidbody.velocity;
		float xMod = horInput * horSpeed;

		// Ground movement is direct; air movement takes momentum into account.
		velocity.x = isGrounded ? xMod : Mathf.Lerp(velocity.x, xMod, airControlFactor);

		// x-movement on the ground has a dead zone so the player won't slide.
		if (isGrounded && Mathf.Abs(horInput) < 0.1f)
			velocity.x = 0.0f;

		rigidbody.velocity = velocity;
        */

        Vector2 velocity = rigidbody.velocity;
        //velocity = new Vector2(velocity.x * playerDrag, rigidbody.velocity.y);
        //float xMod = horInput * horSpeed;
        //velocity.x = isGrounded ? xMod : Mathf.Lerp(velocity.x, xMod, airControlFactor);

        velocity.x = velocity.x * playerDrag;
        if (horInput * velocity.x < horSpeed & !onLadder)
            rigidbody.AddForce(Vector2.right * horInput * moveForce);

        if (isGrounded && Mathf.Abs(horInput) < 0.1f)
            velocity.x = 0.0f;

        if (Mathf.Abs(velocity.x) > horSpeed)
            velocity.x = Mathf.Sign(velocity.x) * horSpeed;

        if (horInput > 0 && !facingRight)
            Flip();
        else if (horInput < 0 && facingRight)
            Flip();

        rigidbody.velocity = velocity;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.tag)
        {
            case "Sweets":
                Destroy(collider.transform.parent.gameObject);
                score += 50;
                UIController.controller.Score(score);
                break;
            case "Savepoint":
                savePoint = collider.transform.parent;
                break;
            case "Goal":
                UIController.controller.Goal();
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
                rigidbody.position = savePoint.position;
                break;
            default:
                break;
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Enemy":
                rigidbody.position = savePoint.position;
                break;
            default:
                break;
        }

    }
}
