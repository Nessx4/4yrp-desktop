using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour {

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "obj_Bullet(Clone)")
        {
            Debug.Log("wah");
            Destroy(this.gameObject);
        }
    }


}
