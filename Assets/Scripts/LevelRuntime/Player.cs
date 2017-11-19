using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private bool facingRight = true;
    private bool canJump = false;

    private bool verticalMove = false;
    private float climbVelocity;
    private float climbDist;
    public float climbSpeed = 10f;
    public float maxSpeed = 10f;
    public float moveForce = 365f;
    public float jumpForce = 700f;
    public float playerGrav = 200f;
    public float playerDrag = 0.985f;
    public Transform groundCheck;

    public bool grounded = false;
    public bool onLadder = false;
    private Rigidbody2D rigidBody2D;
    //private Animator anim;

    //float groundRadius = 0.2f;
    public LayerMask whatsGround;

    void Awake()
    {

        //anim = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        rigidBody2D.gravityScale = playerGrav;

    }

    void Update()
    {
        if (grounded && Input.GetButtonDown("Jump"))
        {
            canJump = true;
            //anim.SetBool("Ground", false);
        }
    }

    void FixedUpdate()
    {

        //Is the character on the ground?
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, whatsGround);
        //anim.SetBool("Ground", grounded);

        //Way of accessing the vertical speed of the character in Inspector, for dealing with animations
        //anim.SetFloat("vSpeed", rigidBody2D.velocity.y);

        //Character movement left and right, ensuring that the character's speed does not surpass a max speed
        float h = Input.GetAxis("Horizontal");

        //anim.SetFloat("Speed", Mathf.Abs(h));

        //rigidBody2D.AddForce(h * maxSpeed * Vector2.right, ForceMode2D.Impulse);
        //rigidBody2D.velocity = new Vector2(h * maxSpeed, rigidBody2D.velocity.y);

        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x * playerDrag, rigidBody2D.velocity.y);
        if (h * rigidBody2D.velocity.x < maxSpeed & !onLadder)
            rigidBody2D.AddForce(Vector2.right * h * moveForce);
            
        if (Mathf.Abs(rigidBody2D.velocity.x) > maxSpeed)
            rigidBody2D.velocity = new Vector2(Mathf.Sign(rigidBody2D.velocity.x) * maxSpeed, rigidBody2D.velocity.y);
            
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        //If the character is on the ground then the "jump" boolean will be true and the character can jump
        if (canJump)
        {
            if (onLadder)
            {
                rigidBody2D.AddForce(new Vector2(h * moveForce, jumpForce*0.5f));
                onLadder = false;
            }
            else rigidBody2D.AddForce(new Vector2(0f, jumpForce));

            canJump = false;
        }
        
        if (verticalMove == true)
        {

        }
        else
        {
             verticalMove = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow));
             climbDist = 5;
        }

        
        if (onLadder && verticalMove)
        {
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
            grounded = true;
            verticalMove = true;
            //anim.SetBool("OnLadder", true);
            rigidBody2D.gravityScale = 0;
            climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");
            rigidBody2D.velocity = new Vector2(0, climbVelocity);
        }
        else
        {
            //anim.SetFloat("climbDist", 0);
            verticalMove = false;
            rigidBody2D.gravityScale = playerGrav;
            //anim.SetBool("OnLadder", false);
        }


    }

    //Flip the character's perspective on the world when facing left instead of right
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}