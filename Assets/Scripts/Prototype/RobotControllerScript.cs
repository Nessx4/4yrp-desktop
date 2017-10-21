using UnityEngine;
using System.Collections;

public class RobotControllerScript : MonoBehaviour
{
    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = true;

    public float maxSpeed = 10f;
    public float moveForce = 365f;
    public float jumpForce = 700f;
    public Transform groundCheck;

    private bool grounded = false;
    private Rigidbody2D rigidBody2D;

    float groundRadius = 0.2f;
    public LayerMask whatsGround;

    void Awake () {

	    rigidBody2D = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    void FixedUpdate ()
    {

        //Is the character on the ground?
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, whatsGround);

        //Character movement left and right, ensuring that the character's speed does not surpass a max speed
        float h = Input.GetAxis("Horizontal");

        rigidBody2D.velocity = new Vector2(h * maxSpeed, rigidBody2D.velocity.y);

        if (h * rigidBody2D.velocity.x < maxSpeed)
            rigidBody2D.AddForce(Vector2.right * h * moveForce);

        if (Mathf.Abs(rigidBody2D.velocity.x) > maxSpeed)
            rigidBody2D.velocity = new Vector2(Mathf.Sign(rigidBody2D.velocity.x) * maxSpeed, rigidBody2D.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        //If the character is on the ground then the "jump" boolean will be true and the character can jump
        if (jump)
        {
            rigidBody2D.AddForce(new Vector2(0f, jumpForce));
            jump = false;   
        }
	}

    //Flip the character's perspective on the world when facing left instead of right
    void Flip ()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public void Jump()
	{
		Debug.Log("Jumping...");

		if (grounded)
			jump = true;
    }
}
