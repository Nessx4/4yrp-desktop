using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MonoBehaviour {

    private Rigidbody2D block;
    private int timer;
    public float gravScale;
    public FallingType type;
	// Use this for initialization
	void Start () {
        block = this.GetComponent<Rigidbody2D>();
        block.gravityScale = gravScale;
	}
	
	// Update is called once per frame
	void Update () {

	}

    // Block falls after a fixed delay upon being landed on
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == FallingType.INSTANT && collision.transform.position.y > block.position.y + 0.8f)
        {
            Invoke("Fall", 1);
        }
    }

    void Fall()
    {
        block.bodyType = RigidbodyType2D.Dynamic;
        block.gameObject.layer = 10;
    }

    // Block falls if it has been in contact for a fixed amount of time
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (type == FallingType.WAIT && collision.transform.position.y > block.position.y + 0.8f)
        {
            timer++;
            Debug.Log(timer);
        }

        if (timer > 100)
            Fall();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("reset");
        timer = 0;
    }

    public enum FallingType
    {
        INSTANT, WAIT
    }

}
