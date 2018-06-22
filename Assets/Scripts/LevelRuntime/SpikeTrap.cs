using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    private GameObject spike;
    public Sprite normal;
    public Sprite active;
    public bool status = false;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D col;

    private bool captured = false;
    private bool up = false;

    public void Control()
    {
        captured = true;
    }

    /*
    public void Move(Vector2 moveAmount)
    {
        
    }
    */

	void Awake () {
        spike = this.gameObject;
        spriteRenderer = spike.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = normal;
        col = spike.GetComponentInChildren<BoxCollider2D>();
    }
	
	// Update is called once per frame
	void Update () {
        if (captured && status)
        {
            Debug.Log("woah");
            spike.tag = "Enemy";
            spriteRenderer.tag = "Enemy";
            spriteRenderer.sprite = active;
            col.offset = new Vector2(0, -0.2f);
            col.size = new Vector2(1, 0.7f);
        }
        else
        {
            spike.tag = "Untagged";
            spriteRenderer.tag = "Untagged";
            spriteRenderer.sprite = normal;
            col.offset = new Vector2(0, -0.35f);
            col.size = new Vector2(1, 0.3f);
        }
	}
    
    public void Activate()
    {
        StartCoroutine(StartSpikes());
    }

    public IEnumerator StartSpikes()
    {
        status = true;
        yield return new WaitForSeconds(2f);
        status = false;
    }
}
