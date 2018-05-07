using UnityEngine;
using System.Collections;

public class Ladder : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && Input.GetKey(KeyCode.UpArrow))
        {
            other.GetComponent<PlayerTEST>().onLadder = true;
        }
        /*
        if (other.tag=="Player")
        {
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
            climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, climbVelocity);
        }
        /*
        if(other.tag=="Player" && Input.GetKey(KeyCode.UpArrow))  && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, climbspeed);
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else if (other.tag == "Player" && Input.GetKey(KeyCode.DownArrow))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -climbspeed);
            other.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            //other.GetComponent<Rigidbody2D>().gravityScale = 0;
            //other.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);
        }*/
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag=="Player")
        {
            other.GetComponent<PlayerTEST>().onLadder = false;
        }
    }
}
